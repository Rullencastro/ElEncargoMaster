using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndObject : MonoBehaviour
{
    private Sequence vibrateSequence;

    private void Start()
    {
        StartCoroutine(VibratingAnimation(1,GameManager.Instance.TotalGameTime()));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            transform.DOScale(2f, 0.5f).SetEase(Ease.OutBack);
            transform.DOMoveY(0.5f, 0.3f).SetEase(Ease.Linear).SetRelative(true).SetLoops(-1,LoopType.Yoyo);
            GameManager.Instance.LevelCompleted();
        }
    }

    private IEnumerator VibratingAnimation(float t, int times)
    {
        vibrateSequence = DOTween.Sequence();
                
        for (int i = 0; i < times; i++)
        {
            vibrateSequence = DOTween.Sequence();
            vibrateSequence.Append(transform.DOMoveX(0.1f, 0.025f).SetEase(Ease.Linear).SetRelative(true));
            vibrateSequence.Append(transform.DOMoveX(-0.2f, 0.05f).SetEase(Ease.Linear).SetRelative(true));
            vibrateSequence.Append(transform.DOMoveX(0.1f, 0.025f).SetEase(Ease.Linear).SetRelative(true));
            vibrateSequence.Play().SetLoops(3,LoopType.Restart);
          
            yield return new WaitForSeconds(t);
        }
    }
}
