using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class RespawnOnTouch : BaseGameRunner
{
    [field: SerializeField]
    public LichtPhysicsCollisionDetector CollisionDetector { get; private set; }

    private Player _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var triggers = CollisionDetector.FindTriggersAsObjects<InstantDanger>();
        foreach (var _ in triggers)
        {
            _player.transform.position = _player.LatestSpawn.transform.position;
            break;
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
