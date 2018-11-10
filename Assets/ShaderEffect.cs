using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffect : MonoBehaviour
{
    [SerializeField] private AnimationCurve zoom;
    [SerializeField] private Texture2D tex;
    private float timer;
    private Vector2 offset;

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

        float scaleFactor = zoom.Evaluate((timer* 0.5f) % 1);
        float val = 0.0f;
        Vector2 dir = new Vector2(Mathf.PerlinNoise(timer, timer + Time.deltaTime), Mathf.PerlinNoise(timer + Time.deltaTime, timer));
        Vector2 pos = Vector2.zero;

        offset += new Vector2(Mathf.Sin(timer), Mathf.Cos(timer)).normalized * /*(Mathf.PerlinNoise(timer, timer)) **/ 0.05f;
        
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                pos = new Vector2(i, j);
                //offset = new Vector2((i * 0.02f) * scaleFactor + timer, (j * 0.02f) * scaleFactor + timer);

                val = Mathf.PerlinNoise(pos.x * 0.02f + offset.x, pos.y * 0.02f + offset.y);
                //val = Mathf.PerlinNoise(pos.x * 0.02f + timer, pos.y * 0.02f + timer);
                tex.SetPixel(i, j, new Color(val, val, val));
                //tex.SetPixel(i, j, new Color(1, 0, 0));
                //tex.SetPixel(i, j, new Color(1, 1, 1));
            }
        }
        tex.Apply();

        return tex;
    }
}
