using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using UnityEngine;

public class RoomObject : BaseGameObject
{
    public Room Room { get; private set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        Room = GetComponentInParent<Room>(true);

        gameObject.SetActive(Room.Active);
        Room.OnRoomEnter += OnEnter;
        Room.OnRoomExit += OnExit;
    }

    private void OnDestroy()
    {
        Room.OnRoomEnter -= OnEnter;
        Room.OnRoomExit -= OnExit;
    }
    private void OnExit()
    {
        gameObject.SetActive(false);
    }

    private void OnEnter()
    {
        gameObject.SetActive(true);
    }
}
