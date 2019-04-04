using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarSkript : MonoBehaviour
{
    public GUISkin HPbar;
    public bool Visible = true;
    public PlayerStat Char;

    public float HPBarLane;
    public float ManaBarLane;
    public float StaminaBarLane;


    void Start()
    {

    }
    void OnGUI()
    {
        if (Visible)
        {
            GUI.skin = HPbar;
            PlayerStat PlayerSt = (PlayerStat)Char.GetComponent("PlayerStat");
            float MaxHP = (float)PlayerSt.MaxHP;
            float MaxMana = (float)PlayerSt.MaxMana;
            float MaxStamina = (float)PlayerSt.MaxStamina;
            float THP = (float)PlayerSt.THP;
            float TMana = (float)PlayerSt.TMana;
            float TStamina = (float)PlayerSt.TStamina;

            float HPBarLane = THP / MaxHP;
            float ManaBarLane = TMana / MaxMana;
            float StaminaBarLane = TStamina / MaxStamina;

            GUI.Box(new Rect(0, 0, 256, 86), " ", GUI.skin.GetStyle("Bar"));
            GUI.Box(new Rect(0, 0, 256, 86), " ", GUI.skin.GetStyle("ManaPool"));
            GUI.Box(new Rect(71, 20, 256 * HPBarLane, 86), " ", GUI.skin.GetStyle("HP"));
            GUI.Box(new Rect(59, 50, 256 * ManaBarLane, 86), " ", GUI.skin.GetStyle("Mana"));
            GUI.Box(new Rect(65, 35, 256 * StaminaBarLane, 86), " ", GUI.skin.GetStyle("Stamina"));
        }
    }


    void Update()
    {
        if (HPBarLane < 0) HPBarLane = 0;
    }
}
