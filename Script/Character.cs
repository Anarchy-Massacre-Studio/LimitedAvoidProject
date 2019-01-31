using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SyzygyStudio;

public class Character : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void move(Gesture.Direction direction)
    {

    }

    bool check_is_move(Gesture.Direction direction)
    {
        return true;
    }
}

