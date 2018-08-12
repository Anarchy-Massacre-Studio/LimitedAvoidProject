using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDirection
{
    public static Vector2 Left { get { return Vector2.left; } }
    public static Vector2 Right { get { return Vector2.right; } }
    public static Vector2 Up { get { return Vector2.up; } }
    public static Vector2 Down { get { return Vector2.down; } }

    public static Vector2 LeftUp { get { return new Vector2(-0.7f, 0.7f); } }
    public static Vector2 LeftDown { get { return new Vector2(-0.7f, -0.7f); } }
    public static Vector2 RightUp { get { return new Vector2(0.7f, 0.7f); } }
    public static Vector2 RightDown { get { return new Vector2(0.7f, -0.7f); } }
}

public class Enemy : MonoBehaviour
{
    Transform player;

    [SerializeField]
    Vector2 attackDirection;
    [SerializeField]
    int speed = 2;

    public void SetAttackDirection(Vector2 vector2)
    {
        attackDirection = vector2;
    }

    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    private void Awake()
    {
        player = Res.Player.transform;
    }

    private void Update()
    {
        if (canKILL()) KILL();
        transform.Translate(AttackDirection.RightUp * speed * Time.deltaTime);
    }

    bool canKILL()
    {
        return Vector2.Distance(transform.position, player.position) <= 0.5f;
    }

    void KILL()
    {
        Debug.Log("KILL");
        Res.Dies.SetPosition(player.position);
        Res.Dies.Take();
    }

}
