using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
