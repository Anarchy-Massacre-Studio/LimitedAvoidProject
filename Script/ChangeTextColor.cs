using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LA.UI;

[ExecuteInEditMode]
public class ChangeTextColor : MonoBehaviour 
{
	public Transform Can;

	public Color Color1;
	public Color Color2;
	List<Text> tList = new List<Text>();

	private void Awake()
	{
		Debug.Log("gg");
		TextTools.FindCom<Text>(Can, ref tList);
		Debug.Log(tList.Count);
		foreach (var item in tList)
		{
			Debug.Log(item.text);
		}
	}

	private void Update() 
	{
		foreach (var item in tList)
		{
			item.color = new Color(Color1.r,Color1.g,Color1.b,item.color.a);
		}
	}
}
