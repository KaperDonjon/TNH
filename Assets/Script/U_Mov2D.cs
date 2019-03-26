using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Mov2D : MonoBehaviour
{
    // доработать движение вдоль поверхности
    // доработать спрыг с платформы
    // улучшить взбирание на ступеньки
    // убрать и спрятать переменные
    // отрегулировать прыжок

    Rigidbody2D RB;

    public float TestRad;
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
    public List<Solid2D> Platforms;
    public List<Solid2D> ActvPls;

    public List<Solid2D> IgnoredPlatforms;
    Vector2 MyPos;

    public bool NoPlatform;
    float NoPlatformTimer;

    bool JumpStart;
    public bool Jump;
    public float JumpForc;
    public float JumpCharge;
    float CurJumpCharge;


    // Start is called before the first frame update
    void Start()
    {
        RB = transform.GetComponent<Rigidbody2D>();



        SecMotor = new GameObject("Motor").transform;
        SecMotor.parent = transform;
        SecMotor.localPosition = Vector3.zero;

        Grnds = new List<Solid2D>();
       Platforms = new List<Solid2D>();
        ActvPls = new List<Solid2D>();

        IgnoredPlatforms = new List<Solid2D>();
    }

    // Update is called once per frame
    void Update()
    {

        MoveDir = LocCon.M.HorDir;

        Jump = LocCon.M.Jump;

        if (LocCon.M.Down)
        {
            NoPlatformTimer = 0.3f;
        }
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        if (NoPlatformTimer > 0)
        {
            NoPlatform = true;
            NoPlatformTimer -= dt;
        }
        else
        {
            NoPlatform = false;
        }
        if (NoPlatform)
        {
            gameObject.layer = 13;
        }
        else
        {
            gameObject.layer = 0;
        }

        LF_WorkJump(dt);

        LF_WorkMov(dt);


    }
    void LF_WorkJump(float dt)
    {
        if (Jump)
        {
            if (JumpStart)
            {
                JumpStart = false;
                RB.velocity += JumpForc*Vector2.up;
            }
            if (CurJumpCharge > 0)
            {
                RB.AddForce(Vector3.up * 10, ForceMode2D.Force);
                CurJumpCharge -= dt;
            }

            OnGrnd = false;
        }
        if (OnGrnd)
        {
            JumpStart = true;
            CurJumpCharge = JumpCharge;
        }
    }

    void LF_WorkMov(float dt)
    {
        LF_CheckPlatforms();

        MyPos = transform.position;
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


        //LocMovDir.y += 0.1f;
        //LocMovDir = LocMovDir.normalized;

        if (MoveDir!=0)
        {

            if(LocMovDir.y<0 && Vector2.Angle( CurVel, LocMovDir) > 5f && OnGrnd)
            {
                RB.velocity = LocMovDir * CurSpd;
            }

            float AccParam = GroundMoveAcc;
            if (!OnGrnd)
            {
                AccParam = 0.1f;
                LocMovDir = Vector2.right* SideMoveMod;
                CurSpd = Mathf.Abs( CurVel.x);
            }

            if(CurSpd<GroundMoveTopSpeed || InvMove)
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


                    if (!wrk.Platform || (wrk.Platform && ActvPls.Contains(wrk)))
                    {
                        TempHit = goh;
                    }
                }
            }
        }

        TempAngl = 90 - Vector2.Angle(TempHit.normal, Vector2.right);
    }

    void LF_CheckPlatforms()
    {

        ActvPls.Clear();

        if (!NoPlatform)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(MyPos + Vector2.up * TestRad * 2f, TestRad, Vector2.down, TestRad * 3);
            foreach (RaycastHit2D goh in hit)
            {
                if (goh.collider.transform.GetComponent<Solid2D>())
                {
                    Solid2D wrk = goh.collider.transform.GetComponent<Solid2D>();
                    if (Platforms.Contains(wrk))
                    {
                        if (goh.distance >= TestRad)
                        {
                            ActvPls.Add(wrk);
                        }
                    }
                }
            }
        }



    }

    private void OnTriggerStay2D(Collider2D col)
    {
        GrndCnt = 0;
        if (col.transform.GetComponent<Solid2D>()) {

            Solid2D wrk = col.transform.GetComponent<Solid2D>();


            if (wrk.Platform)
            {
                if (!NoPlatform)
                {
                    if (!Platforms.Contains(wrk))
                    {
                        Platforms.Add(wrk);
                    }

                    if (ActvPls.Contains(wrk))
                    {
                        if (!Grnds.Contains(wrk))
                        {
                            GrndCnt++;
                            Grnds.Add(wrk);
                        }
                        else
                        {
                            GrndCnt++;
                        }

                    }
                }

            }
            else
            {
                if (!Grnds.Contains(wrk))
                {
                    Grnds.Add(wrk);
                    GrndCnt++;
                }
                else
                {
                    GrndCnt++;
                }
                
            }

            if (GrndCnt > 0)
            {
                OnGrnd = true;
            }

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

            if (wrk.Platform && Platforms.Contains(wrk))
            {
                Platforms.Remove(wrk);
            }
        }

    }
}
