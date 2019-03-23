using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocCon : MonoBehaviour
{
    public static LocCon M;
    public Vector2 Dir;

    public bool Sprint;
    public bool Jump;
    // Start is called before the first frame update
    void Start()
    {
        LocCon.M = this;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            Dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Dir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Dir += Vector2.down;
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
    }
}
