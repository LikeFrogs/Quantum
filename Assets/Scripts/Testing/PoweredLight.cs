using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PoweredLight : PoweredObject {

    private Light light;
    private float intensity;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();

        intensity = light.intensity;
        light.intensity = 0f;
	}

    public override void Activate()
    {
        light.intensity = intensity;
    }

    public override void Deactivate()
    {
        light.intensity = 0f;
    }
}
