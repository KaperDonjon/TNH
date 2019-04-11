using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    //Максимальные значения
    public int MaxHP;
    public int MaxMana;
    public int MaxStamina;
    public int MaxXP;
    //Текущие значения
    public int THP;
    public int TMana;
    public int TStamina;
    public int TXP;

    //Тест версия резистов
    public int Resist;

    //Таланты
    public bool TalentWar;
    public bool TalantCraft;
    public bool WTalent1 = false;
    public bool WTalent2 = false;
    public bool WTalent3 = false;
    public bool TSkill1 = false;
    public bool CTalent1 = false;
    public bool CTalent2 = false;


    public bool SpecSkill = false;

    void Start()
    {

    }


    void Update()
    {
        if (MaxHP < THP) THP = MaxHP;
        if (MaxMana < TMana) TMana = MaxMana;
        if (MaxStamina < TStamina) TStamina = MaxStamina;
        if (THP < 0) THP = 0;
        if (TMana < 0) TMana = 0;
        if (TStamina < 0) TStamina = 0;
    }
}
