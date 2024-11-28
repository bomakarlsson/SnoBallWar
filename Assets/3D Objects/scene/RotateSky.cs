using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotateSky : MonoBehaviour
{
    public float RotateSpeed = 1.2f; // Speed of rotation


    void Update()
    {
        // Ensure a skybox material is assigned in the Lighting settings
        if (RenderSettings.skybox != null)
        {
            // Rotate the skybox using the "_Rotation" property
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);

            
        }
    }
}

