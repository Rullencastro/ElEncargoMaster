using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    public float duration = 1;

    private CanvasGroup canvasGroup;

    private RectTransform panelToAnimate;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        panelToAnimate = GetComponent<RectTransform>();
        panelToAnimate.localScale = Vector3.zero;
    }

    //private void OnEnable()
    //{
    //    canvasGroup.DOFade(1f, duration);
    //    GetComponent<RectTransform>().DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    //}

    //public void DisablePanel()
    //{
    //    Sequence disableSequence = DOTween.Sequence();
    //    disableSequence.Append(canvasGroup.DOFade(0f, duration));
    //    disableSequence.Join(GetComponent<RectTransform>().DOScale(Vector3.zero, duration).SetEase(Ease.InBack));


    //    disableSequence.Play().OnComplete(() => gameObject.SetActive(false));
    //}

    private IEnumerator AnimatePanel(bool show)
    {
        if (show)
        {
            canvasGroup.alpha = 0f;
            panelToAnimate.localScale = Vector3.zero;

            canvasGroup.DOFade(1f, duration);
            panelToAnimate.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        }
        else
        {
            canvasGroup.DOFade(0f, duration);
            panelToAnimate.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);

            yield return new WaitForSeconds(duration);

            canvasGroup.alpha = 0f;
            panelToAnimate.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimatePanel(true));
    }

    public void DisablePanel()
    {
        if(gameObject.activeSelf)
            StartCoroutine(AnimatePanel(false));
    }
}
