using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager: BaseGameObject
{
    [field:SerializeField]
    public AudioSource CollectAudioSource { get; private set; }

    [field: SerializeField]
    public AudioSource SFXAudioSource { get; private set; }

    [field: Header("Default Sounds")]
    [field: SerializeField]
    public AudioClip DefaultCollectSound { get; private set; }

    public void PlayCollectSound(AudioClip clip)
    {
        var clipToPlay = clip != null ? clip : DefaultCollectSound;
        CollectAudioSource.pitch = 0.95f + Random.value * 0.1f;
        CollectAudioSource.PlayOneShot(clipToPlay);
    }

}
