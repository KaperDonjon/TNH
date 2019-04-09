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

    Vector3 TempToolPoint;

    public bool TestBot;
    public GameObject TestObject;
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
            //Work_SimpleEnemy(dt);
            if (TestBot)
            {
                Work_Move2();
            }
        }
        else
        {
            Me.mov.In_HorDir =LocCon.M.HorDir;
            if (LocCon.M.Jump)
            {
                Me.mov.F_DoJump(Vector3.up, 1);
            }
            else
            {
                Me.mov.F_StopJump();
            }

            if (LocCon.M.Down)
            {
                Me.mov.F_JumpOffFromPlatform();
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
        if (Me.mov.OnGround)
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

            if (Closst.distance < CheckRad + Me.mov.StepOffset * 2f)
            {
                Me.mov.In_HorDir = WantedMoveDir;
            }
            else
            {
                Debug.Log("Stop");
                Me.mov.In_HorDir = 0;
            }

        }
    }

    void Work_Move2()
    {
        Vector3 TargetPoint = TestObject.transform.position;

        WantedMoveDir = 1;
        float HorDist = Mathf.Abs(TargetPoint.x - Me.pos.x); 
        if (TargetPoint.x < Me.pos.x)
        {
            WantedMoveDir = -1;
        }


        if(HorDist > Me.mov.BaseRad * 2f)
        {

            if (LF_CheckStepForHole(Me, WantedMoveDir))
            {
                Debug.Log("Дырка");
                float HoleSize = LF_CheckHoleSize(Me, WantedMoveDir);
                Debug.Log(HoleSize);
                if(HoleSize < Me.prm.MaxJumpDist)
                {
                    Debug.Log("Прыгаю");
                    Me.mov.F_DoJump((Vector3.up + Vector3.right * WantedMoveDir).normalized, 0.2f);
                }
                else
                {
                    Debug.Log("Не допрыгнуть");
                    WantedMoveDir = 0;
                }
            }

            if(!LF_CheckMoveSpace(Me, WantedMoveDir))
            {
                Debug.Log("Стенка");
                List<Vector3> JumpTrgs = LF_FindReachebleHeights(TempToolPoint, Me);
                WantedMoveDir = 0;
                if (JumpTrgs.Count > 0)
                {
                    Debug.Log("Запрыгиваю");
                    Me.mov.F_DoJump(Vector3.up, 1f);
                }
                else
                {
                    Debug.Log ("Не запрыгнуть");
                }
            }
        }
        else
        {
            Debug.Log("Пришел");
            WantedMoveDir = 0;
        }
        Debug.Log(WantedMoveDir);
        Me.mov.In_HorDir = WantedMoveDir;
    }

    List<Vector3> LF_FindReachebleHeights(Vector3 CheckPoint, Unit Jumper)
    {
        List<Vector3> rezult = new List<Vector3>();

        float MaxHeight = Jumper.prm.MaxJumpHeight;
        float CheckRad = Jumper.UnitHeight * 0.5f;


        RaycastHit2D[] AllHits = Physics2D.CircleCastAll(CheckPoint + Vector3.up * (MaxHeight + Jumper.UnitHeight), CheckRad, Vector3.down, MaxHeight * 1.5f);
        List<Vector3> TempPointList = new List<Vector3>();
        foreach(RaycastHit2D GoHit in AllHits)
        {
            Solid2D WorkSolid = GoHit.transform.GetComponent<Solid2D>();
            if(WorkSolid!= null && WorkSolid.GetComponentInParent<Unit>() != Jumper)
            {

                Vector3 GoPoint = GoHit.point;
                TempPointList.Add(GoPoint);
            }
        }

        foreach(Vector3 GoPoint in TempPointList)
        {
            bool PassPoint = true;

            Collider2D[] AllCols = Physics2D.OverlapCircleAll(GoPoint + Vector3.up * CheckRad * 1.05f, CheckRad);
            foreach(Collider2D GoCol in AllCols)
            {
                
                Solid2D WorkSolid = GoCol.transform.GetComponent<Solid2D>();
                if (WorkSolid != null)
                {
                    PassPoint = false;
                }
            }

            if (PassPoint)
            {
                rezult.Add(GoPoint);
            }
        }

        return rezult;
    }

    bool LF_CheckStepForHole(Unit Stepper, float MovDir)
    {
        bool rezult = false;
        float CheckStepDist = Stepper.mov.BaseRad * 4f;

        RaycastHit2D[] AllHits = Physics2D.CircleCastAll(Stepper.pos+Vector3.up*Stepper.mov.BaseRad+Vector3.right*MovDir*CheckStepDist, Stepper.mov.BaseRad, Vector3.down, Stepper.prm.MaxJumpHeight*1.2f);
        RaycastHit2D ClossestHit = new RaycastHit2D();
        ClossestHit.distance = Stepper.prm.MaxJumpHeight * 2f;

        foreach (RaycastHit2D GoHit in AllHits)
        {
            Solid2D WorkSolid = GoHit.transform.GetComponent<Solid2D>();
            if (WorkSolid != null && WorkSolid.GetComponentInParent<Unit>() != Stepper)
            {
                if(GoHit.distance< ClossestHit.distance)
                {
                    ClossestHit = GoHit;
                }
            }
        }

        if(ClossestHit.distance>0.9f * Stepper.prm.MaxJumpHeight)
        {
            rezult = true;
        }

        return rezult;
    }

    float LF_CheckHoleSize(Unit Jumper, float HorDir)
    {
        float CheckRad = 0.1f;
        RaycastHit2D[] AllHits = Physics2D.CircleCastAll(Jumper.pos, CheckRad, Vector3.right * HorDir, Jumper.prm.MaxJumpDist * 1.5f);
        RaycastHit2D ClossestHit = new RaycastHit2D();
        ClossestHit.distance = Jumper.prm.MaxJumpDist * 2f;
        ClossestHit.point = Jumper.pos + Vector3.right * HorDir * 99f;

        foreach (RaycastHit2D GoHit in AllHits)
        {
            Solid2D WorkSolid = GoHit.transform.GetComponent<Solid2D>();
            if (WorkSolid != null && WorkSolid.GetComponentInParent<Unit>() != Jumper)
            {
                if(Mathf.Abs( GoHit.point.x - Jumper.pos.x)> Jumper.mov.BaseRad)
                {
                    if (GoHit.distance < ClossestHit.distance)
                    {
                        ClossestHit = GoHit;
                    }
                }

            }
        }

        float rezult = Mathf.Abs(ClossestHit.point.x - Jumper.pos.x);

        return rezult;
    }

    bool LF_CheckMoveSpace(Unit Mover, float HorDir)
    {
        float CheckRad = Mover.mov.BaseRad;
        float CheckDist = 5f;
        RaycastHit2D[] AllHits = Physics2D.CircleCastAll(Mover.pos+Vector3.up*CheckRad*1.05f, CheckRad, Vector3.right * HorDir, CheckDist);
        RaycastHit2D ClossestHit = new RaycastHit2D();
        ClossestHit.distance = CheckDist * 2f;

        foreach (RaycastHit2D GoHit in AllHits)
        {
            Solid2D WorkSolid = GoHit.transform.GetComponent<Solid2D>();
            if (WorkSolid != null && WorkSolid.GetComponentInParent<Unit>() != Mover)
            {
                if (GoHit.distance < ClossestHit.distance)
                {
                    ClossestHit = GoHit;
                }
            }
        }

        TempToolPoint = ClossestHit.point;
        bool rezult = false;
        if(Mathf.Abs( Mover.pos.x - ClossestHit.point.x) > Mover.mov.BaseRad * 2f || ClossestHit.normal.y >0.1f)
        {
            rezult = true;
        }

        return rezult;
    }
}
