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

    public float CheckRad;

    public Action MyAction;
    public float MyActionTimer;
    float WantedMoveDir;

    // Start is called before the first frame update
    public void Init(Unit main)
    {
        Me = main;

        MyActionTimer = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (!PlayerControl)
        {
            Work_SimpleEnemy(dt);
        }
        else
        {
            Me.mov.MoveDir =LocCon.M.HorDir;
            Me.mov.F_DoJump(LocCon.M.Jump);
            if (LocCon.M.Down)
            {
                Me.mov.F_SetSkipPlatformTimer(0.3f);
            }
        }

    }
    void Work_Params()
    {

    }

    void Work_SimpleEnemy(float dt)
    {
        Target = Core_Procces.M.LocalPlayersUnit;


        Work_Vision();



        if (MyActionTimer > 0)
        {
            MyActionTimer -= dt;
        }
        else
        {
            Action n1Act = SelectAction();
            SetAction(n1Act );
        }
        MyAction.DoIt(Me);

        WorkMove();
        




    }

    void Work_Vision()
    {
        RaycastHit2D[] alh;

        if (Target != null)
        {
            SeeLine = true;
            Vector3 TargetVec = Target.pos - Me.pos;

            float TargetDist = TargetVec.magnitude;


            if (TargetDist < SeeRad)
            {

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
        }
    }

    Action SelectAction()
    {
        Action NewOneAction = new Action();
        NewOneAction.ID = "Wait";
        NewOneAction.BaseTimer = 1f;
        NewOneAction.Done = false;

        if (SeeLine)
        {
            Vector3 vec = Target.pos - Me.pos;
            float dist = vec.magnitude;

            
            if(Mathf.Abs( vec.y) > Me.mov.StepOffset)
            {
                //
            }
            else
            {
                if (dist > Me.prm.AtkDist)
                {
                    if (vec.x < 0)
                    {
                        WantedMoveDir = -1;
                    }
                    else
                    {
                        WantedMoveDir = 1;
                    }

                    NewOneAction.ID = "Move";
                    NewOneAction.BaseTimer = 0.1f;

                }
                else
                {
                    Debug.Log(vec);
                    NewOneAction.ID = "Attack";
                    NewOneAction.BaseTimer = Me.prm.AtkDelay;
                }
                
            }
        }



        return NewOneAction;
    }


    void SetAction(Action act)
    {
        MyAction = act;
        MyActionTimer = MyAction.BaseTimer;
    }

    [System.Serializable]
    public class Action
    {
        public string ID;
        public float BaseTimer;
        public int Priority;

        public bool Done;

        public void DoIt(Unit Worker)
        {
            if (!Done)
            {
                if (ID == "Wait")
                {

                }
                if (ID == "Move")
                {

                }
                else
                {
                    Worker.brn.WantedMoveDir = 0;
                }
                if (ID == "Attack")
                {
                    Debug.Log("Проведена атака");
                    Done = true;
                }
            }

        }
    }

    void WorkMove()
    {
        RaycastHit2D[] alh;
        if (Me.mov.OnGrnd)
        {
            Vector3 point = Me.pos + Vector3.right * WantedMoveDir * CheckRad*2f + Vector3.up * CheckRad * 2f;
            alh = Physics2D.CircleCastAll(point, CheckRad, Vector2.down);

            RaycastHit2D Closst = new RaycastHit2D();
            Closst.distance = 33f;
            foreach (RaycastHit2D go in alh)
            {
                if (go.collider.transform.GetComponent<Solid2D>() && go.collider.transform.GetComponentInParent<Unit>() != Me)
                {
                    Solid2D wrk = go.collider.transform.GetComponent<Solid2D>();
                    if (Closst.distance > go.distance)
                    {
                        Closst = go;
                    }
                }
            }

            Debug.Log(Closst.distance + "   "+ (CheckRad + Me.mov.StepOffset * 2f));
            if (Closst.distance < CheckRad + Me.mov.StepOffset * 2f)
            {
                Me.mov.MoveDir = WantedMoveDir;
            }
            else
            {
                Debug.Log("Stop");
                Me.mov.MoveDir = 0;
            }

        }
    }
}
