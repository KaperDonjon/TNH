using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Mover : MonoBehaviour
{
    Rigidbody RB;

    public float H_SurfaceCheckRad;
    public float H_SurfaceCheckVerOfset;
    public RaycastHit SurfaceCheckHit;

    public float H_SidedSurfCheckRad;
    public RaycastHit H_Rhit;
    public RaycastHit H_Lhit;
    public RaycastHit H_Chit;

    RaycastHit HH_Hit;
    float HH_Angl;

    public Vector3 Angls;

    public bool H_OnGround;
    public L_Material MyGround;
    public List<L_Material> H_GroundMaterials;


    public Vector3 HorSpeed;

    public float TopHorSpeed;
    public float HorAcc;
    public float MovDir;

    public float DELPrimAngl;
    public float DELTestForc;
    public Vector3 DeloVe;
    public GameObject Test;

    public float DragParam;

    // Start is called before the first frame update
    void Start()
    {
        RB = transform.GetComponent<Rigidbody>();
        H_GroundMaterials = new List<L_Material>();


        Test = new GameObject("Siursd");
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
    }
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Work_Move2(dt);
    }

    void Work_Move2(float dt)
    {
        LF_SurfaceCheck();

        Test.transform.position = transform.position;

        Vector3 CurVel = RB.velocity;

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

        float WrkAng = Angls.y;

        RaycastHit Nhit = H_Lhit;
        float Nang = Angls.x;
        float Pang = Angls.z;
        RaycastHit Phit = H_Rhit;
        if (rgh)
        {
            Nhit = H_Rhit;
            Nang = Angls.z;
            Phit = H_Lhit;
            Pang = Angls.x;
        }

        bool Invan = false;
        if (Mathf.Abs(Nang) / Nang != Mathf.Abs(Angls.y) / Angls.y && Angls.y!=0)
        {
            if((H_Chit.point.x < Nhit.point.x && Nang<=0) || (H_Chit.point.x > Nhit.point.x && Nang >= 0))
            {
                Debug.Log("ololo");
                Invan = true;
            }
           
        }

        WrkAng = Nang;
        if(Mathf.Abs(Nang) < Mathf.Abs(Angls.y))
        {
           // WrkAng = Angls.y;
        }

        if (Invan)
        {
            WrkAng = Angls.y;
        }

        //if(Nhit.point.y <= H_Chit.point.y) {
        //    if (Mathf.Abs(Nang) < Mathf.Abs(Angls.y))
        //    {
               
        //    }
        //    WrkAng = Angls.y;
        //    ///   WrkAng = Nang;
        //}

        Test.transform.eulerAngles = Vector3.forward * WrkAng;

        Vector3 use = Test.transform.right;
        if (!rgh)
        {
            use = -Test.transform.right;
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


        Vector3 Forc = use * HorAcc;

        float velo = RB.velocity.magnitude;
        if (H_OnGround && MovDir != 0 && (velo < TopHorSpeed || invmo))
        {

            RB.AddForce(Forc, ForceMode.Acceleration);
        }

        DeloVe = Forc;
    }

    void LF_SurfaceCheck()
    {
        Vector3 RaySource = transform.position + Vector3.up * H_SurfaceCheckVerOfset;
        DoCast(RaySource, H_SidedSurfCheckRad);
        H_Chit = HH_Hit;
        Angls.y = HH_Angl;

        float mymo = 1;
        RaySource = transform.position + Vector3.up * H_SurfaceCheckVerOfset + Vector3.right * H_SidedSurfCheckRad*mymo;
        DoCast(RaySource, H_SidedSurfCheckRad);
        H_Rhit = HH_Hit;
        Angls.z = HH_Angl;

        RaySource = transform.position + Vector3.up * H_SurfaceCheckVerOfset - Vector3.right * H_SidedSurfCheckRad*mymo;
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
        if (col.GetContact(0).thisCollider.transform.tag == "Leg")
        {
            if (col.collider.transform.GetComponent<L_Material>())
            {
                L_Material wrk = col.collider.transform.GetComponent<L_Material>();
                if (!H_GroundMaterials.Contains(wrk))
                {
                    H_GroundMaterials.Add(wrk);
                }
                MyGround = wrk;
                H_OnGround = true;
            }

        }
        
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.GetContact(0).thisCollider.transform.tag == "Leg")
        {
            if (col.collider.transform.GetComponent<L_Material>())
            {
                L_Material wrk = col.collider.transform.GetComponent<L_Material>();

                H_OnGround = true;
            }

        }
    }

    private void OnCollisionExit(Collision col)
    {
         if (col.collider.transform.GetComponent<L_Material>())
         {
                L_Material wrk = col.collider.transform.GetComponent<L_Material>();
                if (H_GroundMaterials.Contains(wrk))
                {
                  H_GroundMaterials.Remove(wrk);
                }
            if (H_GroundMaterials.Count <= 0)
            {
                H_OnGround = false;
            }
         }

    }
}
