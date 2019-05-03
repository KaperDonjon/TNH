using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public U_Mov mov;
    public U_Brain brn;
    public U_Act act;
    public U_Param prm;

    public Vector3 pos;

    public List<Item> Pocket;

    public List<InventrySlot> Slots;

    public float UnitHeight;
    // Start is called before the first frame update
    public void Start()
    {
        Init();
    }
    public void Init()
    {
        mov = transform.GetComponent<U_Mov>();
        mov.Init(this);

        act = transform.GetComponent<U_Act>();
        act.Init(this);
        prm = transform.GetComponent<U_Param>();
        prm.Init(this);

        brn = transform.GetComponent<U_Brain>();
        brn.Init(this);

        Pocket = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
    }
    [System.Serializable]
    public class InventrySlot // класс для слота под инвентарь
    {
        public string ArcType; // архетип слота для подбора итема

        Item Contaiment; // контейнер для итема
   

        public void SetItem(Item itm) // назначение итема вконтейнер
        {
            Contaiment = itm;
        }

        public Item GetItem() // получение итема из контейнера
        {
            return Contaiment;
        }
    }

    public Item GetItemFromSlotByArcType(string ArcTypeKey) // получение итема из всех слотов по архетипу
    {
        foreach(InventrySlot go in Slots)
        {
            if(go.ArcType == ArcTypeKey)
            {
                return go.GetItem(); // после слова return выполнение функции прекращается
            }
        }

        return null;
    }

    public void SetItemToSlotByArcType( string ArcTypeKey, Item SetItem) // функция помещения итема в любой слот по ахретипу
    {
        foreach (InventrySlot go in Slots)
        {
            if (go.ArcType == ArcTypeKey)
            {
                if (go.GetItem() != null)// в случае если подходящий слот уже занят, итем из него перемещается в карман
                {
                    Pocket.Add(go.GetItem());
                    
                }
                go.SetItem( SetItem);
            }
        }
    }

    public void RemoveItemFromSlots(Item itm) // функция удаления итема из всех слотов где он есть
    {
        foreach (InventrySlot go in Slots)
        {
            if (go.GetItem() == itm)
            {
                go.SetItem(null);
            }
        }
    }
}
