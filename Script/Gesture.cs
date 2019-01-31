using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SyzygyStudio
{
    /// <summary>
    /// 获取手势。
    /// </summary>
    public class Gesture : MonoBehaviour
    {
        public Text Debug_Text;

        public enum Direction
        {
            Left, Right, Up, Down, LeftUp, LeftDown, RightUp, RightDown
        }

        public static Direction GetDirection()
        {
            return direction;
        }

        public static bool IsEnded()
        {
            return isEnded;
        }

        private static Direction direction;
        private static bool isEnded = false;

        Vector2 touchPos;

        Touch touch_0;

        private void Update()
        {
            if (Input.touchCount == 1)
            {
                touch_0 = Input.touches[0];
                if (touch_0.phase == TouchPhase.Began)
                {
                    isEnded = false; ;
                    touchPos = touch_0.position;
                }
                else if (touch_0.phase == TouchPhase.Moved)
                {
                    var offset = (touch_0.position - touchPos).normalized;
                    if (Mathf.Abs(offset.x) < 0.3f && offset.y > 0.95f) direction = Direction.Up;
                    else if (Mathf.Abs(offset.x) < 0.3f && offset.y < -0.95f) direction = Direction.Down;
                    else if (Mathf.Abs(offset.y) < 0.3f && offset.x < -0.95f) direction = Direction.Left;
                    else if (Mathf.Abs(offset.y) < 0.3f && offset.x > 0.95f) direction = Direction.Right;
                    else if (offset.x >= 0.3f && offset.x <= 0.95f && offset.y > 0f) direction = Direction.RightUp;
                    else if (offset.x >= 0.3f && offset.x <= 0.95f && offset.y < 0f) direction = Direction.RightDown;
                    else if (offset.x >= -0.95f && offset.x <= -0.3f && offset.y > 0f) direction = Direction.LeftUp;
                    else if (offset.x >= -0.95f && offset.x <= -0.3f && offset.y < 0f) direction = Direction.LeftDown;
                }
                else if (touch_0.phase == TouchPhase.Ended)
                {
                    isEnded = true;
                    Debug_Text.text = "";
                }
            }
            if (Input.touchCount > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    var touch = Input.touches[i];
                }
            }
        }
    }
}
