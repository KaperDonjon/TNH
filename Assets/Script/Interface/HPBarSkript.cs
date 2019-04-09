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
    public float ExpBarLane;


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
            float MaxXP = (float)PlayerSt.MaxXP;
            float TXP = (float)PlayerSt.TXP;



            float HPBarLane = THP / MaxHP;
            float ManaBarLane = TMana / MaxMana;
            float StaminaBarLane = TStamina / MaxStamina;
            float ExpBarLane = (TXP / MaxXP) * 100;

            // 86 = 100%  0.86 = 1%

            GUI.Box(new Rect(0, 0, 256, 86), " ", GUI.skin.GetStyle("Bar"));
            GUI.Box(new Rect(0, 0, 256, 86), " ", GUI.skin.GetStyle("ManaPool"));
            GUI.Box(new Rect(71, 20, 256 * HPBarLane, 86), " ", GUI.skin.GetStyle("HP"));
            GUI.Box(new Rect(59, 50, 256 * ManaBarLane, 86), " ", GUI.skin.GetStyle("Mana"));
            GUI.Box(new Rect(65, 35, 256 * StaminaBarLane, 86), " ", GUI.skin.GetStyle("Stamina"));
            GUI.Box(new Rect(36, 8, 262, 0 + ExpBarLane), " ", GUI.skin.GetStyle("Experience"));
        }
    }


    void Update()
    {
        if (HPBarLane < 0) HPBarLane = 0;
    }
}
