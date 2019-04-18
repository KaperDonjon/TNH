using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_Procces : MonoBehaviour
{
    public static Core_Procces M;
    public Unit LocalPlayersUnit;
    // Start is called before the first frame update
    void Start()
    {
        Core_Procces.M = this;

        F_SpawnLoot(transform.position, "Wood", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void F_SpawnLoot(Vector3 SpawnPosition, string ItemID, int ItemCount) // функция спавна лута по ID итема и его количеству
    {
        Item wrk = Core_Base.M.F_GetItemCopyByID(ItemID);//создаем итем с помощью функции получения копии итема из базы по ID
        if(wrk != null)//если итем получился (может не получиться в случае неверного ID)
        {
            if (wrk.Stacks)// если итем может складываться с себеподобным, задаем количество, в ином случае количество роли не играет
            {
                wrk.Count = ItemCount;//
            }

            F_SpawnLoot(SpawnPosition, wrk);// вызываем туже функцию создания лута, но уже с готовым итемом
        }
    }

    public void F_SpawnLoot(Vector3 SpawnPosition, Item itm) // функция спавна лута с готовым итемом в опреледенном месте
    {
        GameObject tmp = GameObject.Instantiate( Core_Base.M.LootObjectPrefab); // создаем копию объекта лута из базы

        Loot tmpLoot = tmp.transform.GetComponent<Loot>(); // получаем компонет лута из созданного объекта
        tmpLoot.MeItem = itm; // вкладываем в компонет лута нужный итем

        tmp.transform.position = SpawnPosition;// помещаем объект в выбранную точку
    }
}
