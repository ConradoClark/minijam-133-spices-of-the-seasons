using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class CauldronThrowObject : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField]
    public ScriptPrefab SplashEffect { get; private set; }
    private PooledComponent _pooledComponent;
    private Cauldron _cauldron;

    protected override void OnAwake()
    {
        base.OnAwake();
        _cauldron = _cauldron.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Actor == null) return;
        Actor.TryGetCustomObject(out _pooledComponent);
        var sprite = _pooledComponent.Props<Sprite>().HasProp("Sprite")
            ? _pooledComponent.Props<Sprite>()["Sprite"]
            : null;

        if (sprite == null) return;

        SpriteRenderer.sprite = sprite;
        DefaultMachinery.AddBasicMachine(ThrowEffect());
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0,0, GameTimer.UpdatedTimeInMilliseconds * 0.5f));
    }

    private IEnumerable<IEnumerable<Action>> ThrowEffect()
    {
        var goUp = transform.GetAccessor()
            .Position
            .Y
            .Increase(Random.Range(1.5f, 2f))
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Over(Random.Range(1f, 1.2f))
            .Build();

        var goDown = transform.GetAccessor()
            .Position
            .Y
            .SetTarget(_cauldron.transform.position.y + _cauldron.SplashOffset.y)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Over(Random.Range(1f, 1.2f))
            .Build();

        var goToCauldron = transform.GetAccessor()
            .Position
            .X
            .SetTarget(_cauldron.transform.position.x + _cauldron.SplashOffset.x)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Over(Random.Range(2f, 2.4f))
            .Build();

        yield return goToCauldron.Combine(goUp.Then(goDown));

        SplashEffect.TrySpawnEffect(_cauldron.transform.position 
                                    + (Vector3)_cauldron.SplashOffset, out _);
        _pooledComponent.EndEffect();
    }
}
