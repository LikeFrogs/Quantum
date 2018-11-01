using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PoweredLight : MonoBehaviour, IPoweredObject {

    private Light light;
    private float intensity;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();

        intensity = light.intensity;
        light.intensity = 0f;
	}

    public void Activate()
    {
        light.intensity = intensity;
    }

    public void Deactivate()
    {
        light.intensity = 0f;
    }
}
