using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Mov : MonoBehaviour
{
    Unit Me;

    Rigidbody2D RB;
    Transform Motor;
    Transform Surfacer;
    public int PlatformLayer;
    public int NoPlatformLayer;

    public float BaseRad;
    public float StepOffset;
    public float DragParam;

    public float In_HorDir;
    public Vector3 JumpDir;

    public int H_JumpPhace;
    public float H_JumpStartForce;
    public float MaxJumpCharge;
    float H_JumpCharge;

    public List<Solid2D> H_MySolids;
    public List<Solid2D> H_CloseSolids;
    public Vector3 R_MyPos;
    public float R_TopSpeed;
    public float R_Acc;
    public float R_AirAcc;
    public Vector3 H_Angls;

    Vector3 cdir;
    Vector3 rdir;
    Vector3 ldir;

    public bool OnGround;
    int Grnds;

    public float H_PlatformIgnorTimer;
    public bool PlatformIgnor;

    public float Charge;
    //ограничить высоту ступенек

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(Unit main)
    {
        Me = main;

        RB = transform.GetComponent<Rigidbody2D>();
        H_MySolids = new List<Solid2D>();
        H_CloseSolids = new List<Solid2D>();

        Motor = new GameObject("Motor").transform;
        Motor.parent = transform;
        Motor.localPosition = Vector3.zero;

        Surfacer = new GameObject("Surfacer").transform;
        Surfacer.parent = transform;
        Surfacer.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //R_MyPos = transform.position;
        //In_HorDir = 0;
        //if (Input.GetKey(KeyCode.D))
        //{
        //    In_HorDir++;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    In_HorDir--;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    F_JumpOffFromPlatform();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (OnGround && H_JumpPhace == 0)
        //    {
        //        H_JumpPhace = 1;
        //    }
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    if (H_JumpPhace == 2)
        //    {
        //        H_JumpCharge = 0;
        //    }
        //}
    }

    private void FixedUpdate()
    {
        R_MyPos = transform.position+ Vector3.up*BaseRad;

        float dt = Time.fixedDeltaTime;
        Vector3 Vel = RB.velocity;
        float Speed = Vel.magnitude;

        if (H_PlatformIgnorTimer > 0)
        {
            H_PlatformIgnorTimer -= dt;
            LF_SetIgnoreAllPlatforms(true);
        }
        else
        {
            LF_SetIgnoreAllPlatforms(false);
        }

        if (!OnGround)
        {
            RB.gravityScale = 1;
            RB.drag = 0;
        }
        else
        {
            RB.gravityScale = 0;
            RB.drag = DragParam;

            float CustomGrav = 1;
            RB.AddForce(-Surfacer.up * CustomGrav, ForceMode2D.Force);
        }
        LF_SurfaceWork();
        LF_Work_Jump(dt);


        float UseAngl = H_Angls.z;
        Vector3 UseDir = rdir;
        if (In_HorDir < 0)
        {
            UseAngl = H_Angls.x;
            UseDir = ldir;

            cdir = -cdir;
        }

        Motor.up = Surfacer.up;

        if (Vector3.Angle(Vector3.up, cdir) < Vector3.Angle(Vector3.up, UseDir))
        {
            UseAngl = H_Angls.y;
        }



        Motor.eulerAngles += Vector3.forward * UseAngl;

        if (In_HorDir != 0)
        {
            float Velx = Surfacer.InverseTransformDirection(Vel).x;

            if (OnGround)
            {

                float Acc = R_Acc;
                float Delt = R_TopSpeed - Speed;
               // Debug.Log(Delt);


                if (Delt < 0)
                {
                    Acc = 0;
                }
                else
                {
                    if (Delt > 0 && Delt < Acc * dt && In_HorDir == Mathf.Abs(Velx) / Velx)
                    {
                        Acc = Delt / dt;

                    }
                }


                RB.AddForce(Motor.right * In_HorDir * Acc, ForceMode2D.Force);
            }
            else
            {
                if (Mathf.Abs(Velx) > R_TopSpeed)
                {
                    if (In_HorDir != Mathf.Abs(Velx) / Velx)
                    {
                        RB.AddForce(Vector3.right * In_HorDir * R_AirAcc, ForceMode2D.Force);
                    }
                }
                else
                {
                    RB.AddForce(Vector3.right * In_HorDir * R_AirAcc, ForceMode2D.Force);
                }


            }

        }

        Debug.DrawLine(R_MyPos, R_MyPos + Vel, Color.blue, 0.1f);
    }

    void LF_SurfaceWork()
    {
        float CastStepOffset = BaseRad * 0.5f;
        float CastDist = 5f;
        Vector3 cpoint = R_MyPos + Surfacer.up * (StepOffset);//убрал отсюда базовый радиус

        H_CloseSolids.Clear();

        RaycastHit2D chit = LF_DoRay(cpoint, -Surfacer.up, BaseRad, CastDist, true);

        Vector3 rpoint = cpoint + Surfacer.right * CastStepOffset;
        RaycastHit2D rhit = LF_DoRay(rpoint, -Surfacer.up, BaseRad, CastDist, false);

        Vector3 lpoint = cpoint - Surfacer.right * CastStepOffset;
        RaycastHit2D lhit = LF_DoRay(lpoint, -Surfacer.up, BaseRad, CastDist, false);


        for(int lo =H_MySolids.Count-1; lo>=0; lo--)
        {
            Solid2D go = H_MySolids[lo];
            if (!H_CloseSolids.Contains(go))
            {
                H_MySolids.RemoveAt(lo);
            }
        }
        if (H_MySolids.Count <= 0)
        {
            OnGround = false;
        }
        //cpoint += -Surfacer.up * chit.distance; нужная точка


        rdir = rpoint - (Surfacer.up * rhit.distance) - (cpoint - Surfacer.up * (chit.distance));
        rdir = rdir.normalized;
        float rangl = Vector3.Angle(rdir, Surfacer.right);
        if (Surfacer.InverseTransformDirection(rdir).y < 0)
        {
            rangl = -rangl;
        }

        Debug.DrawLine(rpoint - (Surfacer.up * rhit.distance), cpoint - Surfacer.up * (chit.distance), Color.red, 0.1f);

        ldir = lpoint - (Surfacer.up * lhit.distance) - (cpoint - Surfacer.up * (chit.distance));
        ldir = ldir.normalized;
        float langl = Vector3.Angle(ldir, -Surfacer.right);
        if (Surfacer.InverseTransformDirection(ldir).y > 0)
        {
            langl = -langl;
        }

        float cangl = Vector3.Angle(chit.normal, Surfacer.up);
        if (Surfacer.InverseTransformDirection(chit.normal).x > 0)
        {
            cangl = -cangl;
        }
        cdir.y = -chit.normal.x;
        cdir.x = chit.normal.y * (Mathf.Abs(chit.normal.x) / chit.normal.x);

        H_Angls.x = langl;
        H_Angls.y = cangl;
        H_Angls.z = rangl;
    }


    RaycastHit2D LF_DoRay(Vector3 point, Vector3 dir, float rad, float Dist, bool spec)
    {
        RaycastHit2D RezHit = new RaycastHit2D();
        RezHit.distance = 2 * Dist;
        RezHit.point = point + dir * (rad + Dist);

        RaycastHit2D[] alh = Physics2D.CircleCastAll(point, rad, dir, Dist);
        foreach (RaycastHit2D hit in alh)
        {
            Solid2D wrk = hit.collider.transform.GetComponent<Solid2D>();
            if (wrk != transform.GetComponentInChildren<Solid2D>())
            {

                bool Ignor = true;
                if (wrk != null)
                {
                    if (wrk.Platform)
                    {
                        Ignor = LF_IgnorPlatform(wrk);
                    }
                    else
                    {
                        Ignor = false;
                    }

                    if (hit.point.y - R_MyPos.y > StepOffset)
                    {
                        Ignor = true;
                    }
                    if (spec)
                    {
                      //  Debug.Log(hit.distance + hit.collider.gameObject.name);
                        if (hit.distance <= StepOffset * 1.1f)
                        {
                            H_CloseSolids.Add(wrk);
                        }
                    }

                }

                if (!Ignor)
                {
                    if (RezHit.distance > hit.distance)
                    {
                        RezHit = hit;
                    }
                }
            }

        }
        return RezHit;


    }

    bool LF_IgnorPlatform(Solid2D platform)
    {
        
        Vector3 LoPo = platform.transform.InverseTransformPoint(R_MyPos);
        if (LoPo.y < 0)
        {
            return true;
        }
        if (LoPo.y < BaseRad)
        {
            if (Mathf.Abs(LoPo.x) - (0.5f) < 0)
            {

             //   Debug.Log("GOV"+LoPo);
                return true;
            }

            Vector3 pnt = platform.transform.GetComponent<EdgeCollider2D>().points[0];
            if (LoPo.x > 0)
            {
                pnt = platform.transform.GetComponent<EdgeCollider2D>().points[1];
            }

            float Dist = (LoPo - pnt).sqrMagnitude;

            if (Mathf.Abs(LoPo.x) - (0.5f + BaseRad * 1.01f) >= 0 && Dist < (BaseRad * BaseRad) * 1.01f)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        return false;
    }
    public void F_JumpOffFromPlatform()
    {
        H_PlatformIgnorTimer = 0.2f;
    }

    void LF_SetIgnoreAllPlatforms(bool Ignor)
    {
        if (Ignor)
        {
            if (gameObject.layer != NoPlatformLayer)
            {
                gameObject.layer = NoPlatformLayer;
            }
        }
        else
        {
            if (gameObject.layer != PlatformLayer)
            {
                gameObject.layer = PlatformLayer;
            }
        }

    }

    public void F_DoJump(Vector3 dir, float ChargeMod)
    {
        if (OnGround)
        {
            JumpDir = dir;
            H_JumpPhace = 1;
            H_JumpCharge = MaxJumpCharge * ChargeMod;
        }

    }
    public void F_StopJump()
    {
        if (H_JumpPhace != 0)
        {
            H_JumpCharge = 0;
        }
    }

    void LF_Work_Jump(float dt)
    {
        if (H_JumpPhace == 1)
        {
            H_JumpPhace = 2;
            RB.velocity = Vector3.right * RB.velocity.x;
            RB.AddForce(JumpDir * H_JumpStartForce, ForceMode2D.Impulse);
        }
        if (H_JumpPhace == 2)
        {
            if (H_JumpCharge > 0)
            {
                H_JumpCharge -= dt;

                float mod = 0.8f + 0.2f * (1 - H_JumpCharge / MaxJumpCharge);
                mod = 1;
                RB.AddForce(-Physics2D.gravity * mod, ForceMode2D.Force);
            }
            else
            {
                H_JumpPhace = 0;
            }
        }

        if (OnGround && H_JumpPhace == 0)
        {
            H_JumpCharge = MaxJumpCharge;
        }
    }

    //private void OnCollisionEnter2D(Collision2D cos)
    //{
    //    // H_MySolids.Clear();
    //    // OnGround = false;
    //    Debug.Log("FUFUFUUFU");
    //    foreach (ContactPoint2D go in cos.contacts)
    //    {
    //        Solid2D wrk = go.collider.transform.GetComponent<Solid2D>();
    //        // if(wrk != null && go.point.y - R_MyPos.y <= StepOffset+ BaseRad)
    //        if (wrk != null && H_CloseSolids.Contains(wrk))
    //        {
    //            bool AddWrk = true;
            
    //            if(wrk.Platform)
    //            {
                    
    //                if (LF_IgnorPlatform(wrk))
    //                {
    //                    AddWrk = false;
    //                }
    //            }


    //            if (AddWrk)
    //            {
    //                if (!H_MySolids.Contains(wrk))
    //                {
    //                    H_MySolids.Add(wrk);
    //                    OnGround = true;
    //                }
    //            }
    //        }
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D cos)
    //{
    //    H_MySolids.Clear();
    //    OnGround = false;
    //    foreach (ContactPoint2D go in cos.contacts)
    //    {
    //        Solid2D wrk = go.collider.transform.GetComponent<Solid2D>();
    //        if (wrk != null && go.point.y - R_MyPos.y <= StepOffset + BaseRad)
    //        {
    //            bool AddWrk = true;

    //            if (wrk.Platform)
    //            {
    //                if (LF_IgnorPlatform(wrk))
    //                {
    //                    AddWrk = false;
    //                }
    //            }


    //            if (AddWrk)
    //            {
    //                if (!H_MySolids.Contains(wrk))
    //                {
    //                    H_MySolids.Add(wrk);
    //                    OnGround = true;
    //                }
    //            }
    //        }
    //    }
    //}
    private void OnCollisionStay2D(Collision2D cos)
    {
        //H_MySolids.Clear();
        //OnGround = false;
        foreach (ContactPoint2D go in cos.contacts)
        {
            Solid2D wrk = go.collider.transform.GetComponent<Solid2D>();
            if (wrk != null && H_CloseSolids.Contains(wrk))
            {
                bool AddWrk = true;

                if (wrk.Platform)
                {
                    if (LF_IgnorPlatform(wrk))
                    {
                        AddWrk = false;
                    }
                }


                if (AddWrk)
                {
                    if (!H_MySolids.Contains(wrk))
                    {
                        H_MySolids.Add(wrk);
                        OnGround = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Solid2D wrk = col.transform.GetComponent<Solid2D>();
        //if (wrk != null && H_CloseSolids.Contains(wrk))
        //{

        //    if (!wrk.Platform || (wrk.Platform && !LF_IgnorPlatform(wrk)))
        //    {
        //        if (!H_MySolids.Contains(wrk))
        //        {
        //            H_MySolids.Add(wrk);
        //            OnGround = true;
        //        }

        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //Solid2D wrk = col.transform.GetComponent<Solid2D>();
        //if (wrk != null && H_CloseSolids.Contains(wrk))
        //{
        //    if (wrk.Platform)
        //    {
        //        if (LF_IgnorPlatform(wrk))
        //        {
        //            if (H_MySolids.Contains(wrk))
        //            {
        //                H_MySolids.Remove(wrk);
        //                if (H_MySolids.Count <= 0)
        //                {
        //                    OnGround = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!H_MySolids.Contains(wrk))
        //            {
        //                H_MySolids.Add(wrk);
        //                OnGround = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (!H_MySolids.Contains(wrk))
        //        {
        //            H_MySolids.Add(wrk);
        //            OnGround = true;
        //        }
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Solid2D wrk = col.transform.GetComponent<Solid2D>();
        if (wrk != null)
        {
            if (H_MySolids.Contains(wrk))
            {
                H_MySolids.Remove(wrk);
                if (H_MySolids.Count <= 0)
                {
                    OnGround = false;
                }
            }
        }
    }

}
