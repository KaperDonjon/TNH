using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Brain : MonoBehaviour
{
    Unit Me;


    public bool PlayerControl;

    public string Team;

    public Unit Target;
    public bool SeeLine;

    public float SeeRad;
    public string H_Tactic;



    float Tim;

    public int CheckCount;
    public float CheckStep;
    // Start is called before the first frame update
    public void Init(Unit main)
    {
        Me = main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Work_Params()
    {

    }

    void Work_SimpleEnemy()
    {
        Vector3 TargetVec = Target.pos - Me.pos;

        float TargetDist = TargetVec.magnitude;

        RaycastHit2D[] alh;
        if (TargetDist< SeeRad)
        {
            SeeLine = true;
            alh = Physics2D.RaycastAll(Me.pos, TargetVec, TargetDist);
            foreach (RaycastHit2D go in alh)
            {
                if (go.collider.transform.GetComponent<Solid2D>())
                {
                    Solid2D wrk = go.collider.transform.GetComponent<Solid2D>();
                    if (!wrk.Platform)
                    {
                        SeeLine = false;
                        break;
                    }

                }
            }
        }

        float sideMod = 1;
        float HalfRad = 0.5f;
        Vector3 point = Me.pos + Vector3.right * sideMod * HalfRad + Vector3.up * Me.prm.JumpHeight;
        alh = Physics2D.CircleCastAll(point, HalfRad, Vector2.down);
        foreach(RaycastHit2D go in alh)
        {

        }
    }

}
