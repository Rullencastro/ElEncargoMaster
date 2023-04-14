using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteraction : MonoBehaviour
{
    public Light spotLight;
    public Material detectionMaterial;
    public GameObject[] model;

    private Material defaultMaterial;

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
}
