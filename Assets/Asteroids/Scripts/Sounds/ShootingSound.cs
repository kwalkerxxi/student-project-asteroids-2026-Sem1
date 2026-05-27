using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootingSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Shooting[] shootingScripts;



    private void OnEnable()
    {
        foreach (var shootingScript in shootingScripts)
        {
            shootingScript.OnFiredShot.AddListener(OnShoot);
        }
    }
    private void OnDisable()
    {
        foreach (var shootingScript in shootingScripts)
        {
            shootingScript.OnFiredShot.RemoveListener(OnShoot);
        }
    }
    private void OnShoot()
    {
     
        audioSource.Play();
    }
}