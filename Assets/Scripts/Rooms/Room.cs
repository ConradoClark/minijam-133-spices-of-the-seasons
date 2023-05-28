using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Interfaces.Events;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Room : BaseGameObject
{
    [field:SerializeField]
    public bool Active { get; private set; }
    private RoomManager _roomManager;
    private IEventPublisher<GameEvents, Room> _eventPublisher;

    public event Action OnRoomEnter;
    public event Action OnRoomExit;

    protected override void OnEnable()
    {
        base.OnEnable();
        _eventPublisher = this.RegisterAsEventPublisher<GameEvents, Room>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.UnregisterAsEventPublisher<GameEvents, Room>();
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _roomManager = _roomManager.FromScene();
        _roomManager.AddRoom(this);
    }

    public void ActivateRoom()
    {
        if (Active) return;

        Active = true;
        _eventPublisher.PublishEvent(GameEvents.OnRoomEnter, this);
        OnRoomEnter?.Invoke();
        _roomManager.MarkAsActive(this);
    }

    public void DeactivateRoom()
    {
        if (!Active) return;
        Active = false;
        _eventPublisher.PublishEvent(GameEvents.OnRoomExit, this);
        OnRoomExit?.Invoke();
    }
}
