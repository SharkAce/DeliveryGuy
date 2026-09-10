using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip skidSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioSource engineAudio;
    [SerializeField] private AudioSource skidAudio;
    [SerializeField] private AudioSource crashAudio;
    [SerializeField] private float maxSkidVolume = 0.9f;
    [SerializeField] private float crashVolume = 0.9f;
    [SerializeField] private float skidFadeSpeed = 10f;

    private CarController car;
    private float currentCarSpeed = 0f;
    private float targetSkidVolume = 0f;
    void Start()
    {
        car = GetComponent<CarController>();
        engineAudio.clip = engineSound;
        engineAudio.Play();

        skidAudio.clip = skidSound;
        skidAudio.Play();
        skidAudio.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentCarSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;

        float carSpeedRate = Mathf.InverseLerp(0f, 40f, currentCarSpeed);
        engineAudio.pitch = Mathf.Lerp(0.85f, 1.3f, carSpeedRate);

        targetSkidVolume = car.GetControls().slideInput ? maxSkidVolume : 0;

        skidAudio.volume = Mathf.Lerp(skidAudio.volume, targetSkidVolume, skidFadeSpeed * Time.deltaTime);
    }

    public void StartCrashSound()
    {
        crashAudio.PlayOneShot(crashSound, crashVolume);
    }
}
