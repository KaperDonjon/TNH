using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov : MonoBehaviour
{
    Rigidbody2D RB;

    public float WalkSpeed;
    public float SprintSpeed;
    public float JumpForce;

    public bool InAir;

    bool jump;
    // Start is called before the first frame update
    void Start()
    {
        RB = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LocCon.M.Jump)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        

        Vector2 dir = LocCon.M.Dir;

        Vector2 CurrentVelocity = RB.velocity;


        float spd = WalkSpeed;
        if (LocCon.M.Sprint)
        {
            spd = SprintSpeed;
        }
        Vector2 NewVelocity = dir * spd;

        NewVelocity.y = CurrentVelocity.y;

        RB.velocity = NewVelocity;




        if (jump)
        {
            DoJump();
            jump = false;
        }
    }

    void DoJump()
    {
        if (!InAir)
        {
            RB.AddForce(Vector2.right * JumpForce, ForceMode2D.Impulse);
        }
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.transform!= transform)
        {
            InAir = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform != transform)
        {
            InAir = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform != transform)
        {
            InAir = true;
        }
    }

}
