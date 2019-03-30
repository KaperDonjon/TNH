using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBld : MonoBehaviour
{
    public Transform LevelRoot;

    public Transform ViewPoint;
    public float ViewSense;

    public Transform Zoomer;
    public Vector2 ZoomBorders;
    public float ZoomSense;

    public Vector3 MousePos;
    public Vector3 MouseGridPos;
    public Vector3 MousePosDelta;
    public Vector3 MouseGridPosDelta;
    Vector3 StrtMousePos;

    public int GridSize;
    public Lvl_BlockGroup MyActvBlockGrp;
    string GrpModType;


    Vector3 ToolPos;
    List<RaycastHit> AllMarkHits;
    RaycastHit ToolHit;

    public GameObject GroupMarker;
    public GameObject GroupSizeButton;

    public string GroupTileID;
    int LastGrpInd;
    public GameObject GroupPrefab;

    public enum E_EditModeType {Non, Group, Geom, Obj}
    E_EditModeType CurEditMode;

    GameObject GrpInterface;
    GameObject GrpListPoint;
    // Start is called before the first frame update
    void Start()
    {
        if (LevelRoot == null)
        {
            LevelRoot = new GameObject("LevelRoot").transform;
            LevelRoot.transform.position = Vector3.zero;
            LevelRoot.tag = "LevelRoot";
        }
        ViewPoint.transform.position = Vector3.zero;

        foreach(Mark go in transform.GetComponentsInChildren<Mark>())
        {
            if(go.Key == "GrpInt")
            {
                GrpInterface = go.gameObject;
                GrpInterface.SetActive(false);
            }
            if(go.Key == "GrpList")
            {
                GrpListPoint = go.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if(GridSize <= 0)
        {
            GridSize = 1;
        }

        Work_Cursor();
        Work_Control(dt);

        Work_GrpControl();
    }

    void Work_GrpControl()
    {
        if(CurEditMode == E_EditModeType.Group)
        {
            if (!GrpInterface.activeSelf)
            {
                GrpInterface.SetActive(true);
                F_BuildListForGroup(LastGrpInd);
            }
            

            if (MyActvBlockGrp != null)
            {
                if (MyActvBlockGrp.Actv)
                {
                    GroupMarker.transform.position = MyActvBlockGrp.MyPos - (Vector3.right + Vector3.up) * MyActvBlockGrp.BlockSiz * 0.5f;
                    GroupMarker.transform.localScale = (MyActvBlockGrp.MySiz + Vector3.one) * MyActvBlockGrp.BlockSiz;



                    if (GrpModType == "ReSiz")
                    {
                        MyActvBlockGrp.F_SetSize(ToolPos + MousePosDelta);

                    }
                    if (GrpModType == "RePos")
                    {
                        MyActvBlockGrp.transform.position = ToolPos + SnapToGrid(MousePosDelta);
                    }
                }




                if (Input.GetMouseButtonUp(0))
                {
                    if (GrpModType == "ReSiz")
                    {
                        MyActvBlockGrp.F_ShowGeom();

                    }
                    GrpModType = "Non";
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckMark("Gico"))
                    {
                        Debug.Log(ToolHit.collider.transform.GetComponent<Mark>().SubKey);

                        MyActvBlockGrp.F_ChangeTileID(ToolHit.collider.transform.GetComponent<Mark>().SubKey);
                        F_BuildListForGroup(ToolHit.collider.transform.GetComponent<Mark>().Num);

                    }
                    else
                    {

                        StrtMousePos = MousePos;
                        if (CheckMark("RePosGroup"))
                        {
                            GrpModType = "RePos";
                            ToolPos = MyActvBlockGrp.transform.position;
                        }
                        else
                        {
                            if (CheckMark("ReSizeGroup"))
                            {
                                GrpModType = "ReSiz";
                                ToolPos = MyActvBlockGrp.MySiz;
                            }
                        }
                    }


                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckMark("Gico"))
                    {
                        Debug.Log(ToolHit.collider.transform.GetComponent<Mark>().SubKey);
                        F_BuildListForGroup(ToolHit.collider.transform.GetComponent<Mark>().Num);
                    }
                }

                GroupMarker.transform.position = Vector3.back * 50f;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (CheckMark("Block"))
                {
                    SetActiveGrp(ToolHit.collider.transform.GetComponentInParent<Lvl_BlockGroup>());
                }
                else
                {
                    if(MyActvBlockGrp!= null)
                    {
                        MyActvBlockGrp.Actv = false;
                        MyActvBlockGrp = null;
                    }

                }
            }


            Vector3 ButPos = Vector3.zero;
            foreach (Mark go in GroupMarker.transform.GetComponentsInChildren<Mark>())
            {
                if (go.Key == "sbut")
                {
                    ButPos = go.transform.position;
                }
            }

            GroupSizeButton.transform.position = ButPos;
        }
        else
        {
            GrpInterface.SetActive(false);
        }


    }

    void SetActiveGrp(Lvl_BlockGroup grp)
    {
        if(MyActvBlockGrp!= null)
        {
            MyActvBlockGrp.Actv = false;
        }

        MyActvBlockGrp = grp;
        MyActvBlockGrp.Actv = true;
    }

    void Work_Control(float dt)
    {
        if (LocCon.M.Jump)
        {
            ViewPoint.position = Vector3.zero;
        }

        float ZoomParam = LocCon.M.MouseScroll*ZoomSense;
        Vector3 ZoomPos = Zoomer.localPosition;
        ZoomPos.z += ZoomParam;
        if (ZoomPos.z > ZoomBorders.x)
        {
            ZoomPos.z = ZoomBorders.x;
        }
        if (ZoomPos.z < ZoomBorders.y)
        {
            ZoomPos.z = ZoomBorders.y;
        }
        Zoomer.localPosition = ZoomPos;

        ViewPoint.position += (Vector3.up * LocCon.M.Dir.y +Vector3.right*LocCon.M.Dir.x)*ViewSense*dt;




        if (Input.GetMouseButtonDown(0))
        {
            StrtMousePos = MousePos;

        }
        if (Input.GetMouseButton(0))
        {
            MousePosDelta = MousePos - StrtMousePos;
        }
    }

    public void F_SetGroupMode()
    {
        CurEditMode = E_EditModeType.Group;

    }
    public void BF_SetGeomMode()
    {
        CurEditMode = E_EditModeType.Geom;
    }
    public void F_DelGroup()
    {
        if (MyActvBlockGrp!=null)
        {
            Destroy(MyActvBlockGrp.gameObject);
        }
    }

    void F_BuildListForGroup(int strtInd)
    {
        string icokey = "Gico";
        Debug.Log("Test");
        Mark[] temp = GrpListPoint.GetComponentsInChildren<Mark>();
        for (int go = temp.Length-1; go>=0; go--)
        {
            Mark wrk = temp[go];
            if(wrk.Key == icokey)
            {
                Destroy(wrk.gameObject);
            }
        }

        int wrkind = strtInd;
        //foreach(TileSet go in Core_Base.M.TileSets)
        //{
        //    if(go.ID == GroupTileID)
        //    {
        //        wrkind = Core_Base.M.TileSets.IndexOf(go);
        //    }
        //}

        float tilesiz = 2f;

        for(int go =0; go<Core_Base.M.TileSets.Count; go++)
        {
            TileSet wTile = Core_Base.M.TileSets[go];

            Vector3 pos = GrpListPoint.transform.position;
            pos += Vector3.right * tilesiz * (go - wrkind);

            GameObject tmp = new GameObject("Ico");
            Mark TmpMrk = Mark.F_MarkObjWithKey(tmp, icokey);
            TmpMrk.SubKey = wTile.ID;
            TmpMrk.Num = go;

            BoxCollider cldr = tmp.AddComponent<BoxCollider>();
            cldr.size = Vector3.one;
            cldr.center = -Vector3.forward * 0.5f;

            SpriteRenderer rndr = tmp.AddComponent<SpriteRenderer>();
            rndr.sprite = wTile.Icon;
            rndr.color = Color.white;

            tmp.transform.position = pos;

            if (wrkind == go)
            {
                tmp.transform.localScale = Vector3.one * 1.2f;
            }
            tmp.transform.parent = GrpListPoint.transform;
        }

        LastGrpInd = strtInd;
    }

    public void F_AddGroup()
    {
        Lvl_BlockGroup n1 = GameObject.Instantiate(GroupPrefab).transform.GetComponent<Lvl_BlockGroup>();
        n1.transform.position = ViewPoint.transform.position;
        n1.transform.parent = LevelRoot;
        n1.F_SetTile(Core_Base.F_GetTileByID(GroupTileID));

        SetActiveGrp(n1);
    }

    void Work_Cursor()
    {
        AllMarkHits = new List<RaycastHit>();
        RaycastHit[] alh = Physics.RaycastAll(Zoomer.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), 50f);
        foreach(RaycastHit go in alh)
        {
            if(go.collider.transform == ViewPoint)
            {
                MousePos = go.point;
                MousePos.z = 0;
            }
            if (go.collider.transform.GetComponent<Mark>())
            {
                AllMarkHits.Add(go);
            }
        }


    }
    bool CheckMark(string key)
    {
        foreach(RaycastHit go in AllMarkHits)
        {
            if(go.collider.transform.GetComponent<Mark>().Key == key)
            {
                Lvl_BlockGroup tst = go.collider.transform.GetComponentInParent<Lvl_BlockGroup>();
                if(MyActvBlockGrp == null || MyActvBlockGrp != tst)
                {
                    ToolHit = go;
                    return true;
                }

            }
        }
        return false;
    }


    Vector3 SnapToGrid(Vector3 pnt)
    {
        Vector3 rez = Vector3.zero;
        rez.x = Mathf.RoundToInt(pnt.x * GridSize) * (1f / GridSize);
        rez.y = Mathf.RoundToInt(pnt.y * GridSize) * (1f / GridSize);
        return rez;
    }
}
