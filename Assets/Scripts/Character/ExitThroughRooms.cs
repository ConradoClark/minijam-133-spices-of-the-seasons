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

internal class ExitThroughRooms : BaseGameRunner
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
        var triggers = CollisionDetector.FindTriggersAsObjects<RoomExit>();
        foreach (var trigger in triggers)
        {
            _player.transform.position = trigger.RoomSpawn.transform.position;
            _player.LatestSpawn = trigger.RoomSpawn;
            trigger.RoomSpawn.Room.ActivateRoom();

            _player.MoveController.BlockMovement(this);
            yield return TimeYields.WaitSeconds(GameTimer, 0.8f);
            _player.MoveController.UnblockMovement(this);
        }

        yield return TimeYields.WaitOneFrameX;
    }
}

