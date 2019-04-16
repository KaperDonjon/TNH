using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Act : MonoBehaviour
{
    Unit Me;
    public Vector3 AtkPoint;

    public float AtkDist;
    public float AttackDelay;
    public float AttackDelayTimer;

    public bool DoAtk;

    public GameObject EffTest;
    // Start is called before the first frame update
    public void Init(Unit main)
    {
        Me = main;
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackDelayTimer > 0)
        {
            DoAtk = false;
            AttackDelayTimer -= Time.deltaTime;
        }
        else
        {
            if (DoAtk)
            {
                DoAttack(AtkPoint);
            }
        }
    }

    public void DoAttack(Vector3 trgpoint)
    {
        Debug.Log("Атака");
        AttackDelayTimer = AttackDelay;


        Vector3 pnt = Me.pos + Vector3.up * Me.UnitHeight*0.5f + (trgpoint - Me.pos + Vector3.up * Me.UnitHeight*0.5f).normalized * (AtkDist );

        GameObject loleff = GameObject.Instantiate(EffTest);
        loleff.transform.position = pnt;
        Destroy(loleff, 0.4f);

        DoAtk = false;
    }
}

public class C_Action
{

}
