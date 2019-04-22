using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{

    public Item MyItem; // переменная для итема, если иконка - это иконка, то там будет храниться именно итем из инвентаря, если же это слот то переменная будет использоваться для определения какой тип итемов помещается в этот слот

    public bool Slot; // является ли объект иконки слотом
    public bool Occuped; // занят ли слот

    public string Key; // ключ для уточнения типа слота карман-снаряжение

}
