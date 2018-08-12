using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获取手势。
/// </summary>
public class Gesture : MonoBehaviour
{
    public enum Direction
    {
        Left, Right, Up, Down, LeftUp, LeftDown, RightUp, RightDown
    }

    public static Direction GetDirection()
    {
        return Direction.Down;
    }

    Vector3 updatePos;
    Vector3 lateUpdaePos;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            updatePos = Input.GetTouch(0).position;
        }
    }

    private void LateUpdate()
    {
        if (Input.touchCount > 0)
        {
            lateUpdaePos = Input.GetTouch(1).position;
            Debug.Log(updatePos + "+" + lateUpdaePos);
        }
    }
}
