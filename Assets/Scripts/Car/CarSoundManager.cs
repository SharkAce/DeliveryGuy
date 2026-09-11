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
    private Rigidbody2D rb;
    void Start()
    {
        car = GetComponent<CarController>();
        rb = GetComponent<Rigidbody2D>();
        engineAudio.clip = engineSound;
        engineAudio.Play();

        skidAudio.clip = skidSound;
        skidAudio.Play();
        skidAudio.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float currentCarSpeed = rb.velocity.magnitude;

        float carSpeedRate = Mathf.InverseLerp(0f, 40f, currentCarSpeed);
        engineAudio.pitch = Mathf.Lerp(0.85f, 1.3f, carSpeedRate);

        float targetSkidVolume = car.GetControls().slideInput ? maxSkidVolume : 0f;

        skidAudio.volume = Mathf.Lerp(skidAudio.volume, targetSkidVolume, skidFadeSpeed * Time.deltaTime);
    }

    public void StartCrashSound()
    {
        crashAudio.PlayOneShot(crashSound, crashVolume);
    }
}
