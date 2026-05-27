using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WarpingSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private WarpObject warpScript;
    private void OnEnable()
    {
        warpScript.OnWarping.AddListener(SoundOnWarp);
    }
    private void OnDisable()
    {
        warpScript.OnWarping.RemoveListener(SoundOnWarp);
    }
    private void SoundOnWarp()
    {
        audioSource.Play();
    }
}