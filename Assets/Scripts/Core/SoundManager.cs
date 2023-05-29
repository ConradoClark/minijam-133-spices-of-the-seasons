using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager: BaseGameObject
{
    [field:SerializeField]
    public AudioSource CollectAudioSource { get; private set; }

    [field: SerializeField]
    public AudioSource SFXAudioSource { get; private set; }

    [field: SerializeField]
    public AudioSource SeasonAudioSource { get; private set; }

    [field: Header("Default Sounds")]
    [field: SerializeField]
    public AudioClip DefaultCollectSound { get; private set; }

    [field: SerializeField]
    public AudioClip DefaultJumpSound { get; private set; }

    [field: SerializeField]
    public AudioClip DefaultSeasonChangeSound { get; private set; }
    [field: SerializeField]
    public AudioClip DefaultThrowSound { get; private set; }

    public void PlayCollectSound(AudioClip clip)
    {
        var clipToPlay = clip != null ? clip : DefaultCollectSound;
        CollectAudioSource.pitch = 0.95f + Random.value * 0.1f;
        CollectAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayJumpSound(AudioClip clip)
    {
        var clipToPlay = clip != null ? clip : DefaultJumpSound;
        SFXAudioSource.pitch = 0.95f + Random.value * 0.1f;
        SFXAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlaySeasonChangeSound()
    {
        var clipToPlay = DefaultSeasonChangeSound;
        SeasonAudioSource.pitch = 0.95f + Random.value * 0.1f;

        SeasonAudioSource.PlayOneShot(clipToPlay);
    }

    public void PlayThrowSound()
    {
        var clipToPlay = DefaultThrowSound;
        SFXAudioSource.pitch = 0.95f + Random.value * 0.1f;
        SFXAudioSource.PlayOneShot(clipToPlay);
    }

}
