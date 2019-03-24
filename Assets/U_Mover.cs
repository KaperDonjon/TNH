using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Mover : MonoBehaviour
{
    Rigidbody RB;

    public RaycastHit SurfaceCheckHit;

    public float H_SidedSurfCheckRad;
    public RaycastHit H_Rhit;
    public RaycastHit H_Lhit;
    public RaycastHit H_Chit;

    RaycastHit HH_Hit;
    float HH_Angl;

    public Vector3 Angls;

    public bool H_OnGround;
    //public L_Material MyGround;
    //public List<L_Material> H_GroundMaterials;


    public Vector3 HorSpeed;

    public float TopGroundSpeed;
    public float GroundAcc;
    public float MovDir;

    Transform SecMotor;

    public float DragParam;

    public float Stupen;

   // List<Collider> MyColliders;
    // Start is called before the first frame update
    void Start()
    {
        RB = transform.GetComponent<Rigidbody>();
        //H_GroundMaterials = new List<L_Material>();


        SecMotor = new GameObject("Motor").transform;
        SecMotor.parent = transform;
        SecMotor.localPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {

        MovDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            MovDir--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovDir++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            F_AddForc(Vector3.up, 5f);
        }
    }
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Work_Move3(dt);
    }
    void Work_Move3(float dt)
    {
        if (RB.velocity.y > 0.1f)
        {
            gameObject.layer = 13;
        }
        else
        {
            gameObject.layer = 0;
        }

        LF_SurfaceCheck();
        Vector3 ContorolForcDir = Vector3.zero;

        Vector3 CurVel = RB.velocity;
        float CurrentSpeed = CurVel.magnitude;

        if (H_OnGround)
        {
            RB.drag = DragParam;
        }
        else
        {
            RB.drag = 0;
        }

        bool rgh = true;
        if (MovDir < 0)
        {
            rgh = false;
        }

        RaycastHit Nhit = H_Lhit;
        if (rgh)
        {
            Nhit = H_Rhit;
        }

        bool invmo = false;
        if (MovDir >= 0)
        {
            if (CurVel.x < 0)
            {
                invmo = true;
            }
        }
        else
        {
            if (CurVel.x > 0)
            {
                invmo = true;
            }
        }

        ContorolForcDir = (Nhit.point - H_Chit.point).normalized;
        if( Mathf.Abs( H_Chit.point.y - Nhit.point.y) > Stupen)
        {
            if (rgh)
            {
                ContorolForcDir = Vector3.right;
            }
            else
            {
                ContorolForcDir = Vector3.left;
            }
        }

        SecMotor.eulerAngles = Vector3.forward * Angls.y;
        if (H_Chit.point.y == Nhit.point.y || ContorolForcDir == Vector3.zero)
        {
            ContorolForcDir = SecMotor.right;
            if (!rgh)
            {
                ContorolForcDir = -ContorolForcDir;
            }
        }

        ContorolForcDir.y += 0.1f;
        ContorolForcDir = ContorolForcDir.normalized;
        Vector3 Forc = ContorolForcDir * GroundAcc;

        Debug.DrawLine(transform.position, transform.position + ContorolForcDir, Color.green, 0.1f);

        if (H_OnGround && MovDir != 0 && (CurrentSpeed < TopGroundSpeed || invmo))
        {
            RB.AddForce(Forc, ForceMode.Acceleration);
        }
    }
    public void F_AddForc(Vector3 dir, float Velo)
    {
        RB.AddForce(dir * Velo, ForceMode.VelocityChange);
    }

    void LF_SurfaceCheck()
    {
        float VerOfs = H_SidedSurfCheckRad + Stupen;

        Vector3 RaySource = transform.position + Vector3.up * VerOfs;
        DoCast(RaySource, H_SidedSurfCheckRad);
        H_Chit = HH_Hit;
        Angls.y = HH_Angl;

        float mym = 0.5f;
        RaySource = transform.position + Vector3.up * VerOfs + Vector3.right * H_SidedSurfCheckRad*mym;
        DoCast(RaySource, H_SidedSurfCheckRad);
        H_Rhit = HH_Hit;
        Angls.z = HH_Angl;

        RaySource = transform.position + Vector3.up * VerOfs - Vector3.right * H_SidedSurfCheckRad * mym;
        DoCast(RaySource, H_SidedSurfCheckRad);
        H_Lhit = HH_Hit;
        Angls.x = HH_Angl;
    }
    void DoCast(Vector3 pnt, float rad)
    {
        RaycastHit[] alh = Physics.SphereCastAll(pnt,rad, Vector3.down, 30f);
        RaycastHit closst = new RaycastHit();
        closst.distance = 100f;
        foreach (RaycastHit hit in alh)
        {
            if (hit.collider.transform.GetComponent<L_Material>())
            {
                if (closst.distance > hit.distance)
                {
                    closst = hit;
                }
            }
        }

        float SurfAngl = Vector3.Angle(Vector3.up, closst.normal);
        if (closst.point.x < pnt.x)
        {
            SurfAngl = -SurfAngl;
        }

        HH_Angl = SurfAngl;
        HH_Hit = closst;

        Debug.DrawLine(closst.point, closst.point + closst.normal, Color.red, 0.1f);

    }

    private void OnCollisionEnter(Collision col)
    {
        //if (col.GetContact(0).thisCollider.transform.tag == "Leg")
        //{
        //    if (col.collider.transform.GetComponent<L_Material>())
        //    {
        //        L_Material wrk = col.collider.transform.GetComponent<L_Material>();
        //        if (!H_GroundMaterials.Contains(wrk))
        //        {
        //            H_GroundMaterials.Add(wrk);
        //        }
        //        MyGround = wrk;
        //        H_OnGround = true;
        //    }

        //}
        
    }

    private void OnCollisionStay(Collision col)
    {
        //if (col.GetContact(0).thisCollider.transform.tag == "Leg")
        //{
        //    if (col.collider.transform.GetComponent<L_Material>())
        //    {
        //        L_Material wrk = col.collider.transform.GetComponent<L_Material>();

        //        H_OnGround = true;
        //    }

        //}

        foreach(ContactPoint go in col.contacts)
        {
            if(go.point.y - transform.position.y < Stupen)
            {
                H_OnGround = true;
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        //if (col.collider.transform.GetComponent<L_Material>())
        //{
        //       L_Material wrk = col.collider.transform.GetComponent<L_Material>();
        //       if (H_GroundMaterials.Contains(wrk))
        //       {
        //         H_GroundMaterials.Remove(wrk);
        //       }
        //   if (H_GroundMaterials.Count <= 0)
        //   {
        //       H_OnGround = false;
        //   }
        //}

        if (col.contacts.Length <= 0)
        {
            H_OnGround = false;
        }
    }


}
