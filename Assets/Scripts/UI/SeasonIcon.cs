using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class SeasonIcon : BaseGameObject
{

    [field: SerializeField]
    public Seasons Season { get; private set; }

    [field:SerializeField]
    public SpriteRenderer IconSprite { get; private set; }

    public bool Active { get; private set; }

    private ChangeSeason _seasonManager;
    private SeasonUIDefaults _uiDefaults;

    protected override void OnAwake()
    {
        base.OnAwake();
        _seasonManager = _seasonManager.FromScene();
        _uiDefaults = _uiDefaults.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _seasonManager.OnSeasonStartedChanging += OnSeasonStartedChanging;

        SetActive(_seasonManager.CurrentSeason, true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _seasonManager.OnSeasonStartedChanging -= OnSeasonStartedChanging;
    }

    private void OnSeasonStartedChanging(Seasons obj)
    {
        var active = Active;
        SetActive(obj);

        if (active != Active)
        {
            DefaultMachinery.AddUniqueMachine($"seasonIcon_{GetInstanceID()}",
                UniqueMachine.UniqueMachineBehaviour.Replace,
                Animate());
        }

    }

    private IEnumerable<IEnumerable<Action>> Animate()
    {
        IconSprite.material = Active ? _uiDefaults.ActiveIconMaterial : _uiDefaults.InactiveIconMaterial;
        yield return transform.GetAccessor()
            .LocalPosition
            .Y
            .SetTarget(Active ? 0 : _uiDefaults.ActiveIconOffset)
            .Over(0.5f)
            .Easing(EasingYields.EasingFunction.CircularEaseInOut)
            .Build();
    }

    private void SetActive(Seasons obj, bool changeInstantly = false)
    {
        Active = obj == Season;

        if (!changeInstantly) return;

        transform.localPosition = new Vector3(transform.localPosition.x, Active ? 0 : _uiDefaults.ActiveIconOffset);
        IconSprite.material = Active ? _uiDefaults.ActiveIconMaterial : _uiDefaults.InactiveIconMaterial;
    }
}
