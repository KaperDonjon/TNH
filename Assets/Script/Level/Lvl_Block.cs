using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_Block : MonoBehaviour
{
    public BlockParams Prm;

    public int Layer;

    BoxCollider EditCollider;

  //  public List<SpriteRenderer> Renders;

    public SpriteRenderer BaseRend;
    public Dictionary<string, Renderer> Rends;

    // Start is called before the first frame update
    public void InitForEditor()
    {
        EditCollider = gameObject.AddComponent<BoxCollider>();
        EditCollider.size = Prm.BlockSize;
        EditCollider.center = Vector3.forward * 0.5f * Prm.BlockSize.z;

        Mark TempMark = gameObject.AddComponent<Mark>();
        TempMark.Key = "Block";


        InitRender();
    }

    public void InitRender()
    {
        Rends = new Dictionary<string, Renderer>();

        TileSet MyTile = Core_Base.F_GetTileByID(Prm.TileID);
        if(MyTile== null)
        {
            Debug.Log("NOTILE");
            return;
        }

        LF_RenewSides();

        BaseRend.sprite = MyTile.Center[Prm.C];
        BaseRend.sortingOrder = Layer*10;

        if (Prm.L>=0)
        {
            AddSprite(MyTile.Left[Prm.L], "L", 2 +Layer * 10);
        }
        if (Prm.R >= 0)
        {
            AddSprite(MyTile.Right[Prm.R], "R", 2 + Layer * 10);
        }
        if (Prm.D >= 0)
        {
            AddSprite(MyTile.Down[Prm.D], "D", 1 + Layer * 10);
        }
        if (Prm.U >= 0)
        {
            AddSprite(MyTile.Up[Prm.U],"U", 3 + Layer * 10);
        }
    }

    void LF_RenewSides()
    {

        TileSet wrk = Core_Base.F_GetTileByID(Prm.TileID);
        if (Prm.C > wrk.Center.Count - 1 || !Prm.Lock)
        {
            Prm.C = Random.Range(0, wrk.Center.Count);
        }

        if (Prm.R >= 0)
        {
            if (Prm.R > wrk.Right.Count - 1 || !Prm.Lock)
            {
                Prm.R = Random.Range(0, wrk.Right.Count);
            }
        }
        if (Prm.L >= 0)
        {
            if (Prm.L > wrk.Left.Count - 1 || !Prm.Lock)
            {
                Prm.L = Random.Range(0, wrk.Left.Count);
            }
        }
        if (Prm.D >= 0)
        {
            if (Prm.D > wrk.Down.Count - 1 || !Prm.Lock)
            {
                Prm.D = Random.Range(0, wrk.Down.Count);
            }
        }
        if (Prm.U >= 0)
        {

            if (Prm.U > wrk.Up.Count - 1 || !Prm.Lock)
            {
                Prm.U = Random.Range(0, wrk.Up.Count);
            }
        }




    }

    void AddSprite(Sprite sprt, string Key, int Order)
    {
        SpriteRenderer n1 = GameObject.Instantiate(BaseRend).GetComponent<SpriteRenderer>();
        n1.transform.parent = transform;
        n1.transform.localPosition = Vector3.zero;

        n1.sortingOrder = Order;
        n1.sprite = sprt;

        Rends.Add(Key, n1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class BlockParams
    {
        public string TileID;
        public bool Lock;

        public Vector3 BlockSize;
        public Vector2Int pos;

        public int C;
        public int R;
        public int L;
        public int U;
        public int D;

        public void F_ClearSides()
        {
            R = -1;
            L = -1;
            D = -1;
            U = -1;
            C = 0;
        }
    }
}
