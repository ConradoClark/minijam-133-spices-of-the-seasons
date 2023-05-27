using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Interfaces.Events;
using Licht.Unity.Objects;
using UnityEngine;

public class Collectable : BaseGameObject
{
    [field:SerializeField]
    public bool Invisible { get; private set; }

    [field: SerializeField]
    public bool Collected { get; private set; }

    [field: SerializeField]
    public ScriptValue Reference { get; private set; }

    private IEventPublisher<GameEvents, OnCollectEventArgs> _eventPublisher;
    public event Action<OnCollectEventArgs> OnCollect;

    public struct OnCollectEventArgs
    {
        public BaseActor Source { get; set; }
        public Collectable Target { get; set; }
        public ScriptValue CollectableReference { get; set; }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _eventPublisher = this.RegisterAsEventPublisher<GameEvents, OnCollectEventArgs>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.UnregisterAsEventPublisher<GameEvents, OnCollectEventArgs>();
    }

    public void Collect(BaseActor actor)
    {
        Collected = true;

        var args = new OnCollectEventArgs
        {
            Source = actor,
            Target = this,
            CollectableReference = Reference
        };

        _eventPublisher.PublishEvent(GameEvents.OnCollected, args);
        OnCollect?.Invoke(args);
    }
}
