using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public Vector3 Pos;
    public List<WayPoint> Nexts;
    public List<ReachType> Reachs;

    public bool Solid;

    public List<WayPoint> Prevs;

    public int tmp;
    // Start is called before the first frame update
    void Start()
    {
        foreach(WayPoint go in Nexts)
        {
            if(go.Prevs == null)
            {
                go.Prevs = new List<WayPoint>();
            }

            go.Prevs.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Pos = transform.position;
    }

    public bool CheckReached(Unit un)
    {
        if((un.pos - Pos).sqrMagnitude < 0.25f)
        {
            return true;
        }
        return false;
    }

    public enum ReachType {Move, JumpUp,  JumpForward }
}
