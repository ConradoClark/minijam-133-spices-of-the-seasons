using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics.CollisionDetection;
using Licht.Unity.Physics.Forces;
using Licht.Unity.Pooling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FinishLevel : BaseGameRunner
{
    [field: SerializeField]
    public LichtPhysicsCollisionDetector CollisionDetector { get; private set; }

    [field: SerializeField]
    public ScriptPrefab CauldronThrowPrefab { get; private set; }

    [field: SerializeField]
    public ScriptPrefab PepperSplashPrefab { get; private set; }

    [field:SerializeField]
    public InputActionReference ConfirmAction { get; private set; }

    [field: SerializeField]
    public string LevelSelectionScene { get; private set; }

    private Player _player;
    private Gravity _gravity;
    private SceneCollectionData _sceneData;
    private Cauldron _cauldron;
    private bool _finished;
    private WinScreen _winScreen;
    private SoundManager _soundManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _gravity = _gravity.FromScene();
        _sceneData = _sceneData.FromScene();
        _cauldron = _cauldron.FromScene(true);
        _winScreen = _winScreen.FromScene(true);
        _soundManager = _soundManager.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (_finished)
        {
            yield return TimeYields.WaitOneFrameX;
            yield break;
        }
        
        var triggers = CollisionDetector.FindTriggersAsObjects<Cauldron>();
        foreach (var _ in triggers)
        {
            _player.JumpController.BlockMovement(this);
            _player.MoveController.BlockMovement(this);
            _gravity.BlockForceFor(this, _player.MoveController.Target);

            var collectables = FindObjectsOfType<CollectableUI>(true)
                .Where(c => _sceneData.CollectablesData.IsCollected(c.Collectable)).ToArray();

            var effects = new List<IPoolableComponent>();

            foreach (var collectable in collectables)
            {

                if (CauldronThrowPrefab.TrySpawnEffect(_player.transform.position,
                        comp => { comp.Component.Props<Sprite>()["Sprite"] = collectable.CollectedSprite; },
                        out var effect))
                {
                    effects.Add(effect);
                }

                _soundManager.PlayThrowSound();
                yield return TimeYields.WaitMilliseconds(GameTimer, Random.Range(200, 400));
            }

            while (effects.All(eff => eff.IsActive))
            {
                yield return TimeYields.WaitOneFrameX;
            }

            yield return TimeYields.WaitSeconds(GameTimer, 1);
            yield return PlayerIntoCauldron().AsCoroutine();
            SpawnEffect();

            _finished = true;
            Debug.Log("ShowWinScreen");
            _winScreen.gameObject.SetActive(true);

            yield return TimeYields.WaitOneFrameX;

            while (!ConfirmAction.action.WasPerformedThisFrame())
            {
                yield return TimeYields.WaitOneFrameX;
            }

            DefaultMachinery.FinalizeWith(() =>
            {
                SceneManager.LoadScene(LevelSelectionScene, LoadSceneMode.Single);
            });

            yield break;
        }

        yield return TimeYields.WaitOneFrameX;
    }

    private void SpawnEffect()
    {
        PepperSplashPrefab.TrySpawnEffect(_player.transform.position, out _);
    }

    private IEnumerable<IEnumerable<Action>> PlayerIntoCauldron()
    {
        var goUp = _player.transform.GetAccessor()
            .Position
            .Y
            .Increase(Random.Range(1.5f, 2f))
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Over(Random.Range(1f, 1.2f))
            .Build();

        var goDown = _player.transform.GetAccessor()
            .Position
            .Y
            .SetTarget(_cauldron.transform.position.y + _cauldron.SplashOffset.y)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Over(Random.Range(1f, 1.2f))
            .Build();

        var goToCauldron = _player.transform.GetAccessor()
            .Position
            .X
            .SetTarget(_cauldron.transform.position.x + _cauldron.SplashOffset.x)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Over(Random.Range(2f, 2.4f))
            .Build();

        yield return goToCauldron.Combine(goUp.Then(goDown));
    }
}
