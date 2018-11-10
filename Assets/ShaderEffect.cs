using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffect : MonoBehaviour
{
    [SerializeField] private AnimationCurve zoom;
    [SerializeField] private Texture2D tex;
    private float timer;


	// Use this for initialization
	void Start () {
        timer = 0f;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Renderer>().material.SetTexture("_PerlinTex", GetPerlinTexture());
        timer += Time.deltaTime;
	}

    Texture GetPerlinTexture()
    {
        tex = new Texture2D(128, 128);

       
        float val = 0.0f;


        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                val = Mathf.PerlinNoise(i * 0.02f + timer, j * 0.02f + timer);
                tex.SetPixel(i, j, new Color(val, val, val));
                //tex.SetPixel(i, j, new Color(1, 0, 0));
                //tex.SetPixel(i, j, new Color(1, 1, 1));
            }
        }
        tex.Apply();

        return tex;
    }
}
