using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteraction : MonoBehaviour
{
    public Light spotLight;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        spotLight.enabled = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        spotLight.enabled = false;
    }
}
