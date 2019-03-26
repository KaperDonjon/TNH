using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_Base : MonoBehaviour
{
    public static Core_Base M;

    public List<TileSet> TileSets;
    // Start is called before the first frame update
    void Start()
    {
        M = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static TileSet F_GetTileByID(string id)
    {
        foreach(TileSet go in Core_Base.M.TileSets)
        {
            if(go.ID == id)
            {
                return go;
            }
        }

        return null;
    }
}
[System.Serializable]
public class TileSet
{
    public string ID;
    public float TileSize;

    public List<Sprite> Center;
    public List<Sprite> Right;
    public List<Sprite> Left;
    public List<Sprite> Up;
    public List<Sprite> Down;
}
