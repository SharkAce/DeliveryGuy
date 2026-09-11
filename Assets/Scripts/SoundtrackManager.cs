using System.Collections;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip deliveries1To3;
    [SerializeField] private AudioClip deliveries4To6;
    [SerializeField] private AudioClip deliveries7To8;
    [SerializeField] private AudioClip policeTrack;

    [Header("Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float musicVolume = 0.6f;
    [SerializeField] private float fadeDuration = 1.5f;

    private AudioClip currentTrack;
    private Coroutine fadeCoroutine;

    public void PlayForDelivery(int deliveryNumber)
    {
        AudioClip nextTrack;

        if (deliveryNumber <= 3)
        {
            nextTrack = deliveries1To3;
        }
        else if (deliveryNumber <= 6)
        {
            nextTrack = deliveries4To6;
        }
        else
        {
            nextTrack = deliveries7To8;
        }

        if (nextTrack == null || nextTrack == currentTrack)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(ChangeTrack(nextTrack));
    }

    private IEnumerator ChangeTrack(AudioClip nextTrack)
    {
        if (audioSource.isPlaying)
        {
            while (audioSource.volume > 0f)
            {
                audioSource.volume -=
                    musicVolume * Time.deltaTime / fadeDuration;

                yield return null;
            }
        }

        currentTrack = nextTrack;
        audioSource.clip = currentTrack;
        audioSource.loop = true;
        audioSource.Play();

        while (audioSource.volume < musicVolume)
        {
            audioSource.volume +=
                musicVolume * Time.deltaTime / fadeDuration;

            yield return null;
        }

        audioSource.volume = musicVolume;
        fadeCoroutine = null;
    }

    public void PlayPoliceTrack()
    {
        if (policeTrack == null || policeTrack == currentTrack)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(ChangeTrack(policeTrack));
    }
}