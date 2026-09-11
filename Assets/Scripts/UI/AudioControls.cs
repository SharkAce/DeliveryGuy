using UnityEngine;
using UnityEngine.UI;

public class AudioControls : MonoBehaviour
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private AudioSource soundtrackSource = null;
    private Image image;
    private bool state = true;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }
    public void StateToggle()
    {
        state = !state;
    }

    public void AudioToggle()
    {
        AudioListener.volume = state ? 1f : 0f;            

    }

    public void MusicToggle()
    {
        soundtrackSource.volume = state ? 1f : 0f;
    }

    public void SpriteToggle()
    {
        image.sprite = state ? onSprite : offSprite;
    }
}
