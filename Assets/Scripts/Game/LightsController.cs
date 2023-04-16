using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    public Light[] lights;
    public float duration = 1f;
    public float endValue = 0.5f;

    //private void OnEnable()
    //{
    //    foreach (Light light in lights) 
    //    {
    //        light.DOIntensity(endValue, duration);
    //    }
    //}

    //private void OnDisable()
    //{
    //    foreach (Light light in lights)
    //    {
    //        light.DOIntensity(0f, duration);
    //    }
    //}

    private IEnumerator AnimateLights(bool show)
    {
        if (show)
        {
            foreach (Light light in lights)
            {
                light.DOIntensity(endValue, duration);
            }
        }
        else
        {
            foreach (Light light in lights)
            {
                light.DOIntensity(0f, duration);
            }

            yield return new WaitForSeconds(duration);

            gameObject.SetActive(false);
        }
    }

    public void ShowLights()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateLights(true));
    }

    public void DisableLights()
    {
        if (gameObject.activeSelf)
            StartCoroutine(AnimateLights(false));
    }
}
