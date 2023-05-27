using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class CreateEffectWhenCollected : BaseGameObject
{
    [field:SerializeField]
    public ScriptPrefab Effect { get; private set; }
    
    [field: SerializeField]
    public Vector3 SpawnOffset { get; private set; }

    private Collectable _collectable;

    protected override void OnAwake()
    {
        base.OnAwake();
        Actor.TryGetCustomObject(out _collectable);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _collectable.OnCollect += OnCollect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _collectable.OnCollect -= OnCollect;
    }


    private void OnCollect(Collectable.OnCollectEventArgs obj)
    {
        Effect.TrySpawnEffect(transform.position + SpawnOffset, out _);
    }
}

