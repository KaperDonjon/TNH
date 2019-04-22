using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{

    public bool Active;
    public GameObject WindowObj;

    Transform TempRoot;

    public Camera InterfaceCamera;
    public Vector3 InterfaceCursorPos;

    public Icon ActiveIcon;

    public Icon SlotUnderCursor;
    public Icon IconUnderCursor;

    public Icon PrevIconSlot;

    // Start is called before the first frame update
    void Start()
    {
        CloseWindow();
    }

    void CameraRayWork() // обработка объектов под курсором мыши
    {
        IconUnderCursor = null; // обнуляем объекты находящиеся под курсором мыши
        SlotUnderCursor = null;

        RaycastHit[] AllHits = Physics.RaycastAll(InterfaceCamera.ScreenPointToRay(Input.mousePosition), 50); // получаем массив всех столкновени1 с коллайдерами луча выпущенного из камеры интерфейса из точки расположения курсора мыши
        foreach (RaycastHit go in AllHits)// перебираем все столкновения в цикле
        {
            Mark wrk = go.collider.transform.GetComponent<Mark>();// получаю компонент Марк в коллайдере обрабатываемого столкновения
            if (wrk!= null)// если он есть...
            {
                if(wrk.Key == "InterfaceSurface")//если ключь в Марке соответствует плоскости интерфейса получаю току в которой находится курсор из столкновения
                {
                    InterfaceCursorPos = go.point;
                }

                if(wrk.Key == "Icon")// если ключ в марке соотвествует иконке, определив слот это или иконка итема присваиваю результат соответствующей переменной
                {
                    Icon WorkIcon = wrk.transform.GetComponent<Icon>();
                    if (WorkIcon.Slot)
                    {
                        SlotUnderCursor = WorkIcon;
                    }
                    else
                    {
                        IconUnderCursor = WorkIcon;
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update() 
    {
        if (Active)
        {
            CameraRayWork(); // обработка объектов под курсором мыши

            if (Input.GetMouseButtonDown(0)) // если нажата кнопка мыши
            {
                if (IconUnderCursor != null)
                {
                    if (IconUnderCursor.Key == "Close") // закрытие окна на кнопке закрытия
                    {
                        CloseWindow();
                        return;
                    }
                    else// если же под курсором была иконка, делаем ее активной, а последним ее слотом делается слот который был в этот момент под курсором
                    {

                        ActiveIcon = IconUnderCursor; 
                        PrevIconSlot = SlotUnderCursor;
                    }


                }
            }

            if (Input.GetMouseButton(0)) // если жмется кнопка мыши
            {
                if (ActiveIcon != null) // если есть активная иконка, двигаем ее в позицию курсора
                {
                    Vector3 newPosition = InterfaceCursorPos;
                    newPosition.z = ActiveIcon.transform.position.z;
                    ActiveIcon.transform.position = newPosition;
                }
            }


            if(ActiveIcon!= null) // если есть активная иконка
            {
                if (Input.GetMouseButtonUp(0)) // и если отпущена кнопка мыши
                {
                    bool IconGoBack = true; // параметр успеха.провала
                    if (SlotUnderCursor != null) // делаем проверку можетли иконка с итемом встать на место слота под курсором
                    {
                        if(!SlotUnderCursor.Occuped && (SlotUnderCursor.MyItem.ArcType == ActiveIcon.MyItem.ArcType || SlotUnderCursor.MyItem.ArcType == "Any"))
                        {
                            IconGoBack = false;
                        }
                    }

                    if (IconGoBack)// если нет под курсором подходящего слота, перемещаем отпущенную иконку в ее предыдущий слот
                    { 
                        Vector3 newpos = PrevIconSlot.transform.position;
                        newpos.z = ActiveIcon.transform.position.z;
                        ActiveIcon.transform.position = newpos;
                        ActiveIcon = null;
                    }
                    else// иначе перемещаем ее на место нового слота
                    {

                        Vector3 newpos = SlotUnderCursor.transform.position;
                        newpos.z = ActiveIcon.transform.position.z;
                        ActiveIcon.transform.position = newpos;
                        

                        PrevIconSlot.Occuped = false; // ставим отметку в слотах новом и предыдущем об занятии и освобожнении
                        SlotUnderCursor.Occuped = true;

                        if(SlotUnderCursor.Key != PrevIconSlot.Key)// если тип слота отличается от типа предыдущего слота
                        {
                            if(SlotUnderCursor.Key == "Pocket")// если новый слот - карман
                            {
                                Core_Procces.M.LocalPlayersUnit.RemoveItemFromSlots(ActiveIcon.MyItem); // удаляем итем из слота снаряжения
                                Core_Procces.M.LocalPlayersUnit.Pocket.Add(IconUnderCursor.MyItem); // помещаем итем из иконки в карман юниту
                            }
                            if(SlotUnderCursor.Key == "Equip")// если новый слот - снаряга
                            {
                                Core_Procces.M.LocalPlayersUnit.Pocket.Remove(ActiveIcon.MyItem);// убираем итем из кармана
                                Core_Procces.M.LocalPlayersUnit.SetItemToSlotByArcType(ActiveIcon.MyItem.ArcType, ActiveIcon.MyItem);// кладем итем в выбранный слот снаряжения
                            }
                        }

                        ActiveIcon = null;
                    }

                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenWindow();
            }
        }
    }

    public void OpenWindow() // функция открытия окна
    {
        CloseWindow(); // на всякий пожарный вызываю функцию закрытия окна
        Unit WorkUnit = Core_Procces.M.LocalPlayersUnit;
        Active = true;
        WindowObj.SetActive(true);
        TempRoot = new GameObject("TempRoot").transform;

        int PocketItemsCount = WorkUnit.Pocket.Count-1;// получаю количество иразных итемов в кармане персонажа, оно будет использовано как индекс итема из списка
        foreach (Icon go in WindowObj.transform.GetComponentsInChildren<Icon>()) // обрабатываю в цикле все объекты иконок-слотов принадлежащие открытому окну
        {
            if (go.Slot)// если это именно слот
            {
                bool Occuped = false;
                if(go.MyItem.ArcType == "Any")// если архетип у слота "любой" то заполняю его итемом из кармана персонажа
                {
                    if (PocketItemsCount >= 0)
                    {
                        // создаю новую иконку для итема и помещаю ее в слот
                        Icon n1 = MakeIconForItem(WorkUnit.Pocket[PocketItemsCount]);
                        Vector3 n1Pos = go.transform.position;
                        n1Pos.z = 0;
                        n1.transform.position = n1Pos;
                        Occuped = true;
                        PocketItemsCount -= 1;// уменьшаю переменную индекса чтобы получить следующий итем из кармана в следующий раз
                    }


                }
                else// если слот не "любой" ищу нужный итем уже в слотах снаряжения персонажа
                {
                    Item wrk = WorkUnit.GetItemFromSlotByArcType(go.MyItem.ArcType);
                    if(wrk!= null)
                    {
                        Icon n1 = MakeIconForItem(wrk);
                        Vector3 n1Pos = go.transform.position;
                        n1Pos.z = 0;
                        n1.transform.position = n1Pos;
                        Occuped = true;
                    }
                }

                go.Occuped = Occuped;
            }
        }


    }

    Icon MakeIconForItem(Item itm) // функция создания объекта иконки для итема
    {
        Icon tmp = GameObject.Instantiate(Core_Base.M.IconObjectPrefab).transform.GetComponent<Icon>();
        tmp.MyItem = itm;

        tmp.transform.parent = TempRoot;
        tmp.transform.GetComponent<SpriteRenderer>().sprite = itm.Ico;

        return tmp;
    }

    public void CloseWindow() // функция закрытия окна
    {
        if (TempRoot != null)
        {
            Destroy(TempRoot.gameObject);
        }

        ActiveIcon = null;
        IconUnderCursor = null;
        SlotUnderCursor = null;

        WindowObj.SetActive(false);
        Active = false;
    }
}
