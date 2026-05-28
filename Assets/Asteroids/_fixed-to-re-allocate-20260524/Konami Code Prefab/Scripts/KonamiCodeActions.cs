using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This activates whatever actions you like once the Konami code is entered.
/// In the demonstration prefab it loads a bouncing menu. 
/// You could look at other loading or bouncing menus e.g. <see href="https://lottiefiles.com/free-animations/google-loading">Example Google Loading menus</see> and use those
/// alternatively, you should just load your cheats menu so that the user can activate your custom cheats
/// Note: If wanting to use the [ProPlayButton] attribute - You should install the 'Inspector Button Pro' package to allow this to be easily testing in the Editor (when not in Play mode). Otherwise just delete this line.
/// <para>
/// For more about 'Inspector Button Pro' see:
/// <see href="https://cyborgassets.weebly.com/inspector-button-pro.html">How to use</see>.
/// <see href="https://assetstore.unity.com/packages/slug/151474">Unity Asset Store link.</see>
/// </para>
/// </summary>
public class KonamiCodeActions : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    private float canvasGroupAlpha = 1;
    [SerializeField] private Image sequenceImage;
    [SerializeField] private float totalAnimationTime = 2;

    [SerializeField] private int numberOfLoopSteps = 10;
    private float startDuration = 0;
    private float loopStepDuration = 0;
    private float endDuration = 0;

    [SerializeField] private bool fadeOutAtEnd = false;

    private void Start()
    {
        startDuration = totalAnimationTime * 0.15f;
        loopStepDuration = (totalAnimationTime * 0.7f) * (1f / numberOfLoopSteps);
        endDuration = totalAnimationTime * 0.15f;
        canvas.enabled = false;
        
        sequenceImage.DOFade(0f, 0f);
        sequenceImage.rectTransform.localScale = Vector3.zero;
    }

    [ProPlayButton]
    public void ShowKonamiCodeCanvas()
    {
        canvasGroup.alpha = 1;
        canvas.enabled = true;
        FadeUp();
    }

    private void FadeUp()
    {
        sequenceImage.rectTransform.DOScale(Vector2.one, startDuration);
        sequenceImage.DOFade(1f, startDuration).OnComplete(PulseImage);
    }

    void PulseImage()
    {
        sequenceImage.rectTransform.DOScale(Vector2.one * 0.8f, loopStepDuration).SetLoops(numberOfLoopSteps, LoopType.Yoyo);
        sequenceImage.DOFade(0.05f, loopStepDuration).SetLoops(numberOfLoopSteps, LoopType.Yoyo).OnComplete(fadeOutAtEnd ? FadeDown : null);
    }

    private void FadeDown()
    {
        //sequenceImage.rectTransform.DOScale(Vector2.zero, endDuration);
        //sequenceImage.DOFade(0f, endDuration);

       Tween tween = DOTween.To(() => canvasGroupAlpha, x => canvasGroupAlpha = x, 0, 0.5f)
                         .SetEase(Ease.Linear)
                         .OnUpdate(() => canvasGroup.alpha = canvasGroupAlpha)
                         .OnComplete(() => canvas.enabled = false);
        
    }
}
