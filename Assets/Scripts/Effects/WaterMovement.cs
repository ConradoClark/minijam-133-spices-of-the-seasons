using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class WaterMovement : BaseGameRunner
{
    [field:SerializeField]
    public float VerticalFrequencyInSeconds { get; private set; }

    [field: SerializeField]
    public float HorizontalFrequencyInSeconds { get; private set; }

    [field: SerializeField]
    public float VerticalAmount { get; private set; }

    [field: SerializeField]
    public float HorizontalAmount { get; private set; }

    private Vector3 _originalLocalPosition;

    protected override void OnAwake()
    {
        base.OnAwake();
        _originalLocalPosition = transform.localPosition;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var horizontalMovement = transform.GetAccessor()
            .LocalPosition
            .X
            .SetTarget(_originalLocalPosition.x + HorizontalAmount)
            .Easing(EasingYields.EasingFunction.CircularEaseInOut)
            .Over(HorizontalFrequencyInSeconds)
            .Build();

        var horizontalBack = transform.GetAccessor()
            .LocalPosition
            .X
            .SetTarget(_originalLocalPosition.x)
            .Easing(EasingYields.EasingFunction.CircularEaseInOut)
            .Over(HorizontalFrequencyInSeconds)
            .Build();

        var verticalMovement = transform.GetAccessor()
            .LocalPosition
            .Y
            .SetTarget(_originalLocalPosition.y + VerticalAmount)
            .Easing(EasingYields.EasingFunction.CircularEaseInOut)
            .Over(VerticalFrequencyInSeconds)
            .Build();

        var verticalBack = transform.GetAccessor()
            .LocalPosition
            .Y
            .SetTarget(_originalLocalPosition.y)
            .Easing(EasingYields.EasingFunction.CircularEaseInOut)
            .Over(VerticalFrequencyInSeconds)
            .Build();

        yield return horizontalMovement.Then(horizontalBack).RepeatUntil(()=>!ComponentEnabled)
            .Combine(verticalMovement.Then(verticalBack).RepeatUntil(() => !ComponentEnabled));
    }
}
