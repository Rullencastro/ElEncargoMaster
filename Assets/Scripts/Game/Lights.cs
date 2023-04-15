using DG.Tweening;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public Light[] lights;
    public float duration = 1f;
    public float endValue = 0.5f;

    private void OnEnable()
    {
        foreach (Light light in lights) 
        {
            light.DOIntensity(endValue, duration);
        }
    }

    private void OnDisable()
    {
        foreach (Light light in lights)
        {
            light.DOIntensity(0f, duration);
        }
    }
}
