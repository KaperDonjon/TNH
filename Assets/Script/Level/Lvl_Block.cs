using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_Block : MonoBehaviour
{
    public BlockParams Prm;

    BoxCollider EditCollider;

    public List<SpriteRenderer> Renders;

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
        TileSet MyTile = Core_Base.F_GetTileByID(Prm.TileID);
        if(MyTile== null)
        {
            Debug.Log("NOTILE");
            return;
        }
        Renders[0].sprite = MyTile.Center[Random.Range(0, MyTile.Center.Count)];

        if (Prm.SideHor.x != 0)
        {
            AddSprite(MyTile.Left[Random.Range(0, MyTile.Left.Count)], 2);
        }
        if (Prm.SideHor.y != 0)
        {
            AddSprite(MyTile.Right[Random.Range(0, MyTile.Right.Count)], 2);
        }
        if (Prm.SideVer.x != 0)
        {
            AddSprite(MyTile.Down[Random.Range(0, MyTile.Down.Count)], 1);
        }
        if (Prm.SideVer.y != 0)
        {
            AddSprite(MyTile.Up[Random.Range(0, MyTile.Up.Count)], 3);
        }

    }
    void AddSprite(Sprite sprt, int Order)
    {
        SpriteRenderer n1 = GameObject.Instantiate(Renders[0].gameObject).GetComponent<SpriteRenderer>();
        n1.transform.parent = transform;
        n1.transform.localPosition = Vector3.zero;

        n1.sortingOrder = Order;
        n1.sprite = sprt;

        Renders.Add(n1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class BlockParams
    {
        public string TileID;

        public Vector3 BlockSize;
        public Vector2Int pos;
        public Vector2Int SideHor;
        public Vector2Int SideVer;
    }
}
