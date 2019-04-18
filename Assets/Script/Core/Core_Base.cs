using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_Base : MonoBehaviour
{
    public static Core_Base M;

    public GameObject LootObjectPrefab;
    public List<Item> ItemList; // базовый список итемов

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

    public Item F_GetItemCopyByID(string ID) // функция поиска по ID нужного итема в базовом списке итемов, которая возвращает копию найденного итема или возвращает ничего если ID не верный
    {
        Item rez = new Item(); // заготовка возвращаемой копии итема
        Item wrk = null; // переменная для найденного в базе итема
        foreach(Item go in ItemList) // перебираем все итемы в базовом списке
        {
            if(go.ID == ID)// если искомый ID совпадает с ID итема из базы
            {
                wrk = go; // заносим итем в переменную
                break;// и обрываем цикл
            }
        }
        if (wrk != null)// если после завершения цикла в переменной есть найденный итем
        {
            // копируем его постоянные параметры в заготовку копии
            rez.ID = wrk.ID;
            rez.Ico = wrk.Ico;
            rez.Stacks = wrk.Stacks;

            return rez; // и возвращаем заготовку
        }
        else // если после просмотра всех итемов в базовом списке ни чего не было найдено
        {
            return null; // возвращаем ничто.
        }
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
    public Sprite Icon;

    public List<Sprite> Center;
    public List<Sprite> Right;
    public List<Sprite> Left;
    public List<Sprite> Up;
    public List<Sprite> Down;
}
