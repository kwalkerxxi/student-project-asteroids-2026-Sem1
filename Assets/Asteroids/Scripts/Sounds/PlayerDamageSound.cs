using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerDamageSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerCollisions playerCollisionScript;
    private void OnEnable()
    {
        playerCollisionScript.OnDied.AddListener(SoundOnDeath);
    }
    private void OnDisable()
    {
        playerCollisionScript.OnDied.RemoveListener(SoundOnDeath);
    }
    private void SoundOnDeath()
    {
        transform.SetParent(null);
        audioSource.Play();
    }
}
