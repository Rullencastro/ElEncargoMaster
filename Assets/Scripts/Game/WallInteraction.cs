using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteraction : MonoBehaviour
{
    public Light spotLight;
    public Material detectionMaterial;
    public GameObject[] model;

    private Material defaultMaterial;
    private float breakDuration = 0.5f;

    private void Start()
    {
        defaultMaterial = model[0].GetComponent<MeshRenderer>().material;
    }

    public void WallDetected()
    {
        spotLight.enabled = true;

        foreach (var m in model) 
        {
            m.GetComponent<MeshRenderer>().material = detectionMaterial;
        }
    }

    public void WallNoDetected()
    {
        spotLight.enabled = false;
        foreach(var m in model)
        {
            m.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    public void BreakWall()
    {
        GetComponent<Collider>().enabled = false;
        transform.DORotate(new Vector3(0,359,0), breakDuration).SetEase(Ease.Linear).SetRelative(true);
        transform.DOScale(Vector3.zero, breakDuration).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));
    }
}
