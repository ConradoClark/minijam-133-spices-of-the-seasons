using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class CollectObjects : BaseGameRunner
{
    [field:SerializeField]
    public LichtPhysicsCollisionDetector CollisionDetector { get; private set; }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var triggers = CollisionDetector.FindTriggersAsObjects<Collectable>();
        foreach (var trigger in triggers)
        {
            if (trigger.Invisible || trigger.Collected) continue;

            trigger.Collect(Actor);
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
