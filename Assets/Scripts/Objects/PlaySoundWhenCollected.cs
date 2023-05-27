using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class PlaySoundWhenCollected : BaseGameObject
{
    [field:SerializeField]
    public AudioClip OverrideAudioClip { get; private set; }

    private Collectable _collectable;
    private SoundManager _soundManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        Actor.TryGetCustomObject(out _collectable);
        _soundManager = _soundManager.FromScene();
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
        _soundManager.PlayCollectSound(OverrideAudioClip);
    }
}
