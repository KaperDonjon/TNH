using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocCon : MonoBehaviour
{
    public static LocCon M;
    public Vector2 Dir;
    public float HorDir;
    public bool Up;
    public bool Down;
    public bool Sprint;
    public bool Jump;

    public float MouseScroll;
    // Start is called before the first frame update
    void Start()
    {
        LocCon.M = this;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = Vector2.zero;
        HorDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            HorDir--;
            Dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            HorDir++;
            Dir += Vector2.right;
        }

        Up = false;
        if (Input.GetKey(KeyCode.W))
        {
            Up = true;
            Dir += Vector2.up;
        }

        Down = false;
        if (Input.GetKey(KeyCode.S))
        {
            Dir += Vector2.down;
            Down = true;
        }

        Dir = Dir.normalized;

        Sprint = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Sprint = true;
        }
        Jump = false;
        if (Input.GetKey(KeyCode.Space))
        {
            Jump = true;
        }

        MouseScroll = 0;
        MouseScroll = Input.mouseScrollDelta.y;
    }
}
