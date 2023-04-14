using DG.Tweening;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    public float duration = 1;

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        canvasGroup.DOFade(1f, duration);
        GetComponent<RectTransform>().DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    private void OnDisable()
    {
        canvasGroup.DOFade(0f, duration);
        GetComponent<RectTransform>().DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
