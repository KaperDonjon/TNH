using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{

    public Item MeItem; // итем который находится в объекте с лутом

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentInChildren<SpriteRenderer>().sprite = MeItem.Ico;// Ищем во всех дочерних объектах компонент для отображения спрайта и задаем ему картинку итема. Если не будет компонента или картинки или итема - будет ошибка.
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;//иногда надо еще сменить цвет для спрайта

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Disappear()//Функция уничтожения объекта
    {
        Destroy(gameObject);// пока достаточно просто уничтожить этот GameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Unit WorkUnit = col.transform.GetComponentInParent<Unit>(); //если что-то пересеклось с лутом, проверяем есть ли у этого компонент Юнита(т.е. Юнит ли это)
        if(WorkUnit != null)//если это все таки юнит...
        {
            if (WorkUnit != Core_Procces.M.LocalPlayersUnit) {// если юнит пересекший коллайдер не игровой, то прерываю выполнение функции.
                return; 
            }

            bool new1 = true;//делаем переменную которая определит, будет ли поднятое новой позицией в кармане юнита или сложится с тем что уже есть
            if (MeItem.Stacks)//если итем может складываться с себеподобными...
            {
                foreach (Item go in WorkUnit.Pocket)//перебираем все уже имеющиеся итемы в кармане юнита по одному
                {
                    if (go.ID == MeItem.ID)//если находим итем с совпадающим ID...
                    {
                        go.Count += MeItem.Count; //добавляем к количеству в кармане количество поднятого
                        new1 = false;//ставим отметку что новая позиция в кармане не нужна
                        break;// перрываем цикл, т.к. 1. повторяющихся складывающихся итемов быть в кармане не должно 2. все количество поднятого уже добавлено
                    }
                }
            }
            if (new1)// если поднятое не складывает или не было найдено в кармане...
            {
                if (!WorkUnit.Pocket.Contains(MeItem))// проверяем нет ли вкармане уже этого предмета во избежание дублирования
                {
                    WorkUnit.Pocket.Add(MeItem); // добавляем итем из лута в карман как новую позицию
                }
               
            }

            //тут бы добавить функцию для показа эффекта поднятия
            Disappear();// вызываем функцию уничтожения объекта лута
        }
    }
}

[System.Serializable]
public class Item
{
    public string ArcType; // архетип (материал, оружие, расход...)
    public string ID; // уникальное название
    public int Count;// количество
    public bool Stacks; // отвечает за то можетли Итем складываться с другими такимиже

    public Sprite Ico; //Спрайт для визуального отображения итема
}
