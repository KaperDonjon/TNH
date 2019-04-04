using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public U_Mov2D mov;
    public U_Brain brn;
    public U_Act act;
    public U_Param prm;

    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        mov = transform.GetComponent<U_Mov2D>();
        mov.Init(this);

        act = transform.GetComponent<U_Act>();
        act.Init(this);
        prm = transform.GetComponent<U_Param>();
        prm.Init(this);

        brn = transform.GetComponent<U_Brain>();
        brn.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
    }
}
