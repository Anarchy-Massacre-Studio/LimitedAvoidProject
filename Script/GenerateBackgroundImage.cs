using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateBackgroundImage : MonoBehaviour
{
    public Texture2D BackgroundImage_1;
    public RawImage Image;

    public Color Color_1;
    public Color Color_2;

    public float Scale = 1f;

    Color[] color_1;

    private void Awake()
    {
        BackgroundImage_1 = new Texture2D(Screen.width / 8, Screen.height / 8);
        color_1 = new Color[BackgroundImage_1.width * BackgroundImage_1.height];

        Image.texture = BackgroundImage_1;
        
    }

    private void Update()
    {
        {
            float y = .0f;
            while (y < BackgroundImage_1.height)
            {
                float x = .0f;
                while (x < BackgroundImage_1.width)
                {
                    float sample = Mathf.PerlinNoise(x / BackgroundImage_1.width * Scale + Time.time / 2, y / BackgroundImage_1.height * Scale + Time.time / 2);

                    color_1[System.Convert.ToInt32(y * BackgroundImage_1.width + x)] = Color.Lerp(Color_1, Color_2, sample);

                    x++;
                }
                y++;
            }

            BackgroundImage_1.SetPixels(color_1);
            BackgroundImage_1.Apply();
        }
    }
}
