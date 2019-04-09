using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Param : MonoBehaviour
{
    Unit Me;
    public float MaxJumpHeight;
    public float MaxJumpDist;

    public float AtkDist;
    public float AtkDelay;
    // Start is called before the first frame update
    public void Init(Unit main)
    {
        Me = main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
