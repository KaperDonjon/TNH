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


    float DelayTimer;
    public bool V_FreeSpace;
    public float V_HoleSize;
    public bool V_Hole;
    public List<Vector3> V_JumpPoints;

    int toolnum;

    public WayPoint MyWayPoint;
    public WayPoint NextWayPoint;
    public List<WayPoint> Way;

    public List<WayPoint> AllWP;
    // Start is called before the first frame update
    public void Init(Unit main)
    {
        Me = main;

        MyActionTimer = 0.1f;

        AllWP = new List<WayPoint>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("WP"))
        {
            AllWP.Add(go.transform.GetComponent<WayPoint>());
        }

        MyWayPoint = ClossestWayPoint(transform.position);
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
                // Work_Move2();
                 Work_Move3();
               // Work_Move4();
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
             //   Debug.Log("Дырка");
                float HoleSize = LF_CheckHoleSize(Me, WantedMoveDir);
                Debug.Log(HoleSize);
                if(HoleSize < Me.prm.MaxJumpDist)
                {
                  ////  Debug.Log("Прыгаю");
                    Me.mov.F_DoJump((Vector3.up + Vector3.right * WantedMoveDir).normalized, 0.2f);
                }
                else
                {
                   // Debug.Log("Не допрыгнуть");
                    WantedMoveDir = 0;
                }
            }

            if(!LF_CheckMoveSpace(Me, WantedMoveDir))
            {
              //  Debug.Log("Стенка");
                List<Vector3> JumpTrgs = LF_FindReachebleHeights(TempToolPoint, Me);
                WantedMoveDir = 0;
                if (JumpTrgs.Count > 0)
                {
                    Vector3 JumpPoint = Me.pos;
                    foreach(Vector3 go in JumpTrgs)
                    {
                        Debug.Log("saddas");
                        if (Mathf.Abs(JumpPoint.y - TargetPoint.y) < Mathf.Abs(TargetPoint.y - go.y))
                        {
                            JumpPoint = go;
                        }
                    }
                 //   Debug.Log("Запрыгиваю");
                    Me.mov.F_DoJump(Vector3.up, 1f);
                }
                else
                {
                //    Debug.Log ("Не запрыгнуть");
                }
            }
        }
        else
        {
          //  Debug.Log("Пришел");
            WantedMoveDir = 0;
        }
      //  Debug.Log(WantedMoveDir);
        Me.mov.In_HorDir = WantedMoveDir;
    }

    void Work_Move3()
    {
        Vector3 TargetPoint = TestObject.transform.position;

        WantedMoveDir = 0;
        if (DelayTimer > 0)
        {
            DelayTimer -= Time.deltaTime;
        }
        else
        {
            WantedMoveDir = 1;
            float HorDist = Mathf.Abs(TargetPoint.x - Me.pos.x);
            if (TargetPoint.x < Me.pos.x)
            {
                WantedMoveDir = -1;
            }

            V_Hole = LF_CheckStepForHole(Me, WantedMoveDir);
            if (V_Hole)
            {
                V_HoleSize = LF_CheckHoleSize(Me, WantedMoveDir);
            }
            V_FreeSpace = !LF_CheckMoveSpace(Me, WantedMoveDir);
            if (!V_FreeSpace)
            {
                V_JumpPoints = LF_FindReachebleHeights(TempToolPoint, Me);
            }

            if (HorDist < Me.mov.BaseRad+Me.act.AtkDist)
            {
                WantedMoveDir = 0;
                if(Mathf.Abs( Me.pos.y + Me.UnitHeight*0.5f - TargetPoint.y)<Me.UnitHeight*0.5f)
                {
                    Me.act.DoAtk = true;
                    Me.act.AtkPoint = TargetPoint;
                }
            }
            else
            {
                if (V_Hole)
                {
                    if (V_HoleSize < Me.prm.MaxJumpDist)
                    {
                        Me.mov.F_DoJump((Vector3.up + Vector3.right * WantedMoveDir).normalized, 0.3f);
                    }
                    else
                    {
                        WantedMoveDir = 0;
                    }
                }
                else
                {
                    if (V_FreeSpace)
                    {
                        if (V_JumpPoints.Count > 0)
                        {
                            Vector3 JumpPoint = V_JumpPoints[0];
                            foreach (Vector3 go in V_JumpPoints)
                            {
                                if (Mathf.Abs(JumpPoint.y - TargetPoint.y) > Mathf.Abs(TargetPoint.y - go.y))
                                {
                                    JumpPoint = go;
                                }
                            }


                            if (!Me.mov.OnGround)
                            {
                                Debug.Log(JumpPoint.y);
                                if (Me.pos.y - Me.mov.BaseRad*2f < JumpPoint.y)
                                {
                                    
                                    WantedMoveDir = 0;
                                }
                                else
                                {
                                    Me.mov.F_StopJump();
                                }
                            }
                            else
                            {
                                Me.mov.F_DoJump(Vector3.up, 1f);
                                WantedMoveDir = 0;
                            }
                        }
                        else
                        {
                           
                            WantedMoveDir = 0;
                        }


                    }
                }
            }


        }




        Me.mov.In_HorDir = WantedMoveDir;
    }

    void Work_Move4()
    {
        Vector3 TargetPoint = TestObject.transform.position;

        int WantedMovDir = 0;

        if (MyWayPoint != null)
        {
            FindWayToPos(TargetPoint);
        }

        if(MyWayPoint!=null  && NextWayPoint != null)
        {

            if (MyWayPoint.Nexts.Contains(NextWayPoint))
            {

                if(NextWayPoint.Pos.x > Me.pos.x)
                {
                    WantedMovDir = 1;
                }
                else
                {
                    WantedMovDir = -1;
                }


                WayPoint.ReachType Reach = MyWayPoint.Reachs[MyWayPoint.Nexts.IndexOf(NextWayPoint)];

                if (MyWayPoint.CheckReached(Me))
                {
                    if (Reach == WayPoint.ReachType.Move)
                    {

                    }
                    if (Reach == WayPoint.ReachType.JumpUp)
                    {
                      //  WantedMovDir = 0;
                        Me.mov.F_DoJump(Vector3.up, 1f);
                    }
                    if (Reach == WayPoint.ReachType.JumpForward)
                    {
                        Me.mov.F_DoJump((Vector3.up + Vector3.right * WantedMoveDir).normalized, 0.8f);
                    }
                }


            }

            if (NextWayPoint.CheckReached(Me))
            {
                MyWayPoint = NextWayPoint;
                if (Way.Count > 0)
                {
                    NextWayPoint = Way[0];

                    Way.RemoveAt(0);
                }

            }
        }


        Me.mov.In_HorDir = WantedMovDir;

    }

    WayPoint ClossestWayPoint(Vector3 pos)
    {
        WayPoint Clost = null;
        foreach (WayPoint go in AllWP)
        {
            if (Clost == null)
            {
                Clost = go;
            }
            else
            {
                if (go.Solid)
                {
                    if (Mathf.Abs(Clost.Pos.x - pos.x) > Mathf.Abs(go.Pos.x - pos.x))
                    {
                        Clost = go;
                    }
                }

            }
        }

        return Clost;
    }
    public void FindWayToPos(Vector3 pos)
    {
        WayPoint Clost = ClossestWayPoint(pos);



        TestWay(MyWayPoint, 10);
        List<WayPoint> TempList = new List<WayPoint>();
        TempList.Add(Clost);
        AddPrevToList(TempList, Clost);
        Way = TempList;

        Way.RemoveAt(0);
        if (Way.Count > 0)
        {
            NextWayPoint = Way[0];
            Way.RemoveAt(0);
        }

    }

    void AddPrevToList(List<WayPoint> lt, WayPoint cur)
    {
        foreach(WayPoint go in cur.Prevs)
        {
            if (go.tmp < cur.tmp)
            {
                lt.Insert(0,go);
                AddPrevToList(lt, go);
                return;
            }
        }
    }

    void TestWay(WayPoint strt, int steps)
    {
        foreach(WayPoint go in AllWP)
        {
            go.tmp = -100;
        }
        strt.tmp = 0;
        CalcNextStep(strt, steps);
    }

    void CalcNextStep(WayPoint frst, int cap)
    {
        foreach(WayPoint go in frst.Nexts)
        {
            if(go.tmp<0 || go.tmp >frst.tmp+1)
            {
                go.tmp = frst.tmp + 1;
            }
        }
        if (frst.tmp+1 < cap)
        {
            foreach (WayPoint go in frst.Nexts)
            {
                if(go.tmp == frst.tmp + 1)
                {
                    CalcNextStep(go, cap);
                }
            }
        }
    }

    List<Vector3> LF_FindReachebleHeights(Vector3 CheckPoint, Unit Jumper)
    {
        
        RaycastHit2D[] AllHits = Physics2D.CircleCastAll(Me.pos+Vector3.up*Me.mov.BaseRad, Me.mov.BaseRad*0.8f, Vector3.up, Me.prm.MaxJumpHeight + Me.UnitHeight);
        RaycastHit2D Clossest = new RaycastHit2D();
        Clossest.distance = (Me.prm.MaxJumpHeight + Me.UnitHeight) * 2f;
        Clossest.point = Me.pos + Vector3.up * 100f;
        foreach(RaycastHit2D go in AllHits)
        {
            Solid2D WorkSolid = go.transform.GetComponent<Solid2D>();
            if (WorkSolid != null && WorkSolid.GetComponentInParent<Unit>() != Jumper)
            {
                if (go.distance < Clossest.distance)
                {
                    Clossest = go;
                }
            }

        }

        List<Vector3> rezult = new List<Vector3>();
        float CheckRad = Jumper.UnitHeight * 0.5f;
        float MaxHeight = Jumper.prm.MaxJumpHeight;



        AllHits = Physics2D.CircleCastAll(CheckPoint + Vector3.up * (MaxHeight + Jumper.UnitHeight), CheckRad, Vector3.down, MaxHeight * 1.5f);
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
            if(GoPoint.y + Me.UnitHeight*1.1f > Clossest.point.y)
            {
                PassPoint = false;
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
