using DG.Tweening;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    [SerializeField] float bounceSpeed = 8;
    [SerializeField] float bounceHeight = 1;
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        transform.DOMoveZ(startPosition.z + bounceHeight, bounceSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
