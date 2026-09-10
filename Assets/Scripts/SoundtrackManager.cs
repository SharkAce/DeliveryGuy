using System.Collections.Generic;
using UnityEngine;
 
public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> playlist;
    [SerializeField] private AudioSource audioSource;
 
    private int currentTrack = 0;
 
    private void Start()
    {
        if(playlist.Count > 0)
        {
            audioSource.clip = playlist[0];
            audioSource.Play();
        }
    }
 
    private void Update()
    {
        if(!audioSource.isPlaying && playlist.Count > 0)
        {
            PlayNextTrack();
        }
    }
 
    public void PlayNextTrack()
    {
        currentTrack++;
        if(currentTrack >= playlist.Count)
        {
            currentTrack = 0;
        }
 
        PlayTrack(currentTrack);
    }
 
    public void PlayTrack(int index)
    {
        audioSource.clip = playlist[index];
        audioSource.Play();
    }
}