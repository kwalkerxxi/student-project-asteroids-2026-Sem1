using DG.Tweening;
using UnityEngine;

public class QuickTweenScores : MonoBehaviour
{
    [SerializeField] Ease easeToUse = Ease.InOutSine;
    public void GrowScores()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScale(3, 1f).SetEase(easeToUse);
    }

}
