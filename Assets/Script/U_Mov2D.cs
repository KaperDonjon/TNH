using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Mov2D : MonoBehaviour
{
    Rigidbody2D RB;

    public float CheckRad;
    public float StepOffset;

    public float GroundMoveAcc;
    public float GroundMoveTopSpeed;

    public float MoveDir;

    RaycastHit2D TempHit;
    float TempAngl;

    RaycastHit2D Chit;
    float Cang;
    RaycastHit2D Nhit;

    Transform SecMotor;

    public bool OnGrnd;
    public float DragParam;

    int GrndCnt;

    List<Solid2D> Grnds;

    // Start is called before the first frame update
    void Start()
    {
        RB = transform.GetComponent<Rigidbody2D>();

        Grnds = new List<Solid2D>();

        SecMotor = new GameObject("Motor").transform;
        SecMotor.parent = transform;
        SecMotor.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        MoveDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            MoveDir--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveDir++;
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        LF_WorkMov(dt);
    }


    void LF_WorkMov(float dt)
    {
        Vector2 MyPos = transform.position;
        float SideMoveMod = Mathf.Abs(MoveDir) / MoveDir;

        Vector2 CurVel = RB.velocity;
        float CurSpd = CurVel.magnitude;

        bool InvMove = false;
        if(SideMoveMod!= Mathf.Abs(CurVel.x) / CurVel.x)
        {
            InvMove = true;
        }

        if (OnGrnd)
        {
            RB.drag = DragParam;
        }
        else
        {
            RB.drag = 0;
        }

        //-----------------------------------------------

        LF_CheckSurf(MyPos + Vector2.up * (CheckRad + StepOffset), CheckRad);
        Chit = TempHit;
        Cang = TempAngl;

        LF_CheckSurf(MyPos +Vector2.up * (CheckRad + StepOffset) + Vector2.right*(CheckRad*0.5f)*SideMoveMod, CheckRad);
        Nhit = TempHit;

        Vector2 LocMovDir = (Nhit.point - Chit.point).normalized;
        if (Mathf.Abs(Chit.point.y - Nhit.point.y) > StepOffset)
        {
            if (SideMoveMod < 0)
            {
                LocMovDir = Vector2.left;
            }
            else
            {
                LocMovDir = Vector2.right;
            }
        }

        SecMotor.eulerAngles = Vector3.forward * Cang;
        if (Chit.point.y == Nhit.point.y || LocMovDir == Vector2.zero)
        {
            LocMovDir = SecMotor.right;
            if (SideMoveMod<0)
            {
                LocMovDir = -LocMovDir;
            }
        }


        LocMovDir.y += 0.1f;
        LocMovDir = LocMovDir.normalized;

        if (MoveDir!=0)
        {
            float AccParam = GroundMoveAcc;

            if(OnGrnd &&(CurSpd<GroundMoveTopSpeed || InvMove))
            {
                RB.AddForce(LocMovDir * AccParam, ForceMode2D.Impulse);
            }
        }

        if(MoveDir!= 0)
        {
            Vector3 Lololo = LocMovDir;
            Debug.DrawLine(transform.position, transform.position + Lololo, Color.green, 0.1f);
        }

    }
    void LF_CheckSurf(Vector2 strt, float rad)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(strt, rad, Vector2.down,200f);
        TempHit.distance = 222f;
        foreach(RaycastHit2D goh in hit)
        {
            if (goh.collider.transform.GetComponent<Solid2D>())
            {
                Solid2D wrk = goh.collider.transform.GetComponent<Solid2D>();

                if (TempHit.distance > goh.distance)
                {
                    if (!wrk.Platform || (wrk.Platform && Grnds.Contains(wrk)))
                    {
                        TempHit = goh;
                    }
                }
            }
        }

        TempAngl = 90 - Vector2.Angle(TempHit.normal, Vector2.right);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.GetComponent<Solid2D>()) {

            Solid2D wrk = col.transform.GetComponent<Solid2D>();

            if (!Grnds.Contains(wrk))
            {
                Grnds.Add(wrk);
            }

            GrndCnt++;
            OnGrnd = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.GetComponent<Solid2D>())
        {
            Solid2D wrk = col.transform.GetComponent<Solid2D>();

            if (Grnds.Contains(wrk))
            {
                Grnds.Remove(wrk);
            }

            GrndCnt--;
            if (GrndCnt <= 0)
            {
                OnGrnd = false;
            }
        }

    }
}
