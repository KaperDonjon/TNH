using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_BlockGroup : MonoBehaviour
{
    public string TileID;
    public Vector3 MyPos;
    public Vector3 MySiz;
    public float BlockSiz;

    public List<Lvl_Block> MyBlocks;

    public bool Actv;

    public GameObject Pef;
    // Start is called before the first frame update
    void Start()
    {
        Work_Geom2();
    }

    // Update is called once per frame
    void Update()
    {

        if (Actv)
        {

            MyPos = transform.position;

        }
    }

  
    public void F_SetSize(Vector3 siz)
    {
        MySiz = siz;
        if (MySiz.x < 0)
        {
            MySiz.x = 0;
        }
        if (MySiz.y < 0)
        {
            MySiz.y = 0;
        }
    }
    public void F_ChangeSiz(Vector3 delt)
    {
        MySiz += delt;
        if (MySiz.x < 0)
        {
            MySiz.x = 0;
        }
        if (MySiz.y < 0)
        {
            MySiz.y = 0;
        }


    }
    public void F_SetTile(TileSet til)
    {
        TileID = til.ID;
        BlockSiz = til.TileSize;
        Work_Geom2();
    }
    public void F_ShowGeom()
    {
        Work_Geom2();
    }

    void Work_Geom2()
    {

        List<Lvl_Block.BlockParams> TempList = new List<Lvl_Block.BlockParams>();
        for(int go = MyBlocks.Count-1; go>=0; go--)
        {
            TempList.Add(MyBlocks[go].Prm);
            Destroy(MyBlocks[go].gameObject);
            MyBlocks.RemoveAt(go);
        }

        int xx = Mathf.FloorToInt(MySiz.x / BlockSiz)+1;
        int yy = Mathf.FloorToInt(MySiz.y / BlockSiz) + 1;
        for(int gox = 0; gox<xx; gox++)
        {
            for(int goy = 0; goy< yy; goy++)
            {
                Vector3 pos = Vector3.zero;
                pos.x = gox * BlockSiz;
                pos.y = goy * BlockSiz;

                Lvl_Block tmp = GameObject.Instantiate(Pef).transform.GetComponent<Lvl_Block>();
                tmp.transform.parent = transform;
                tmp.transform.localPosition = pos;

                bool n1 = true;
                foreach(Lvl_Block.BlockParams go in TempList)
                {
                    if(go.pos.x == gox && go.pos.y == goy)
                    {
                        n1 = false;
                        tmp.Prm = go;

                        TempList.Remove(go);
                        break;
                    }
                }
                if(n1){
                    tmp.Prm = new Lvl_Block.BlockParams();
                    tmp.Prm.TileID = TileID;

                    tmp.Prm.BlockSize = Vector3.one*BlockSiz;
                }

                tmp.Prm.SideHor = Vector2Int.zero;
                tmp.Prm.SideVer = Vector2Int.zero;
                tmp.Prm.pos.x = gox;
                tmp.Prm.pos.y = goy;


                if (gox == 0) { tmp.Prm.SideHor.x++; }
                if (goy == 0) { tmp.Prm.SideVer.x++; }
                if (gox == xx-1) { tmp.Prm.SideHor.y++; }
                if (goy == yy-1) { tmp.Prm.SideVer.y++; }

                MyBlocks.Add(tmp);
                tmp.InitForEditor();
    
            }
        }
    }
}

public enum E_Direction { up, dn, rg, lf}
