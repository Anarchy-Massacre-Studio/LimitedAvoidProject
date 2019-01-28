using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGen
{
	public Color Color;

	public Vector3 HSV;

	public Color original;
	public Color light_1;
	public Color light_2;
	public Color dark_1;
	public Color dark_2;
	public Color dark_3;

	public Color comple_original;
	public Color comple_light_1;
	public Color comple_light_2;
	public Color comple_dark_1;
	public Color comple_dark_2;
	
	// Update is called once per frame
	public ColorGen(float m_h)
	{
		Color = Color.HSVToRGB(m_h, 1f, 1f);
		HSV = new Vector3(m_h,1f,1f);

		original = Color;
		light_1 = Color.HSVToRGB(HSV.x, HSV.y - 0.25f, HSV.z);
		light_2 = Color.HSVToRGB(HSV.x, HSV.y - 0.5f, HSV.z);
		dark_1 = Color.HSVToRGB(HSV.x, HSV.y, HSV.z - 0.25f);
		dark_2 = Color.HSVToRGB(HSV.x, HSV.y, HSV.z - 0.5f);
		dark_3 = Color.HSVToRGB(HSV.x, HSV.y, HSV.z - 0.7f);

		comple_original = Color.HSVToRGB(com_h(HSV.x), HSV.y,HSV.z);
		comple_light_1 = Color.HSVToRGB(com_h(HSV.x), HSV.y - 0.25f,HSV.z);
		comple_light_2 = Color.HSVToRGB(com_h(HSV.x), HSV.y - 0.5f,HSV.z);
		comple_dark_1 = Color.HSVToRGB(com_h(HSV.x), HSV.y,HSV.z - 0.25f);
		comple_dark_2 = Color.HSVToRGB(com_h(HSV.x), HSV.y,HSV.z - 0.5f);
	}

	float com_h(float h)
	{
		h += 0.5f;
		if(h > 1f) h -= 1f;
		return h;
	}
}
