using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsInterface : MonoBehaviour
{
    public GUISkin PlayerStats;
    public bool Visible = false;

    public int LvL;
    public int SkillPoint;

    public int HPpoint = 1;
    public int StaminaPoint = 1;
    public int ManaPoint = 1;
    public int ResistPoint = 1;

    public int THPpoint;
    public int TStaminaPoint;
    public int TManaPoint;
    public int TResistPoint;

    public int TLvL;
    public int TSkillPoint;




    void Start()
    {
        TLvL = LvL;

        THPpoint = HPpoint;
        TStaminaPoint = StaminaPoint;
        TManaPoint = ManaPoint;
        TResistPoint = ResistPoint;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) Visible = true;




    }
    private void OnGUI()
    {
        if (Visible)
        {
            GUI.skin = PlayerStats;

            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 400, 360), " ", GUI.skin.GetStyle("Box"));
      //      GUI.Box(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 90, 200, 100), " ", GUI.skin.GetStyle("NameStat"));
       //     GUI.Box(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 75, 200, 100), " ", GUI.skin.GetStyle("NameStat"));
         //   GUI.Box(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 60 , 200, 100), " ", GUI.skin.GetStyle("NameStat"));
      //      GUI.Box(new Rect(Screen.width / 2 - 90, Screen.height / 2 - 45, 200, 100), " ", GUI.skin.GetStyle("NameStat"));

            GUI.Label(new Rect(Screen.width / 2 - 80 , Screen.width / 2 - 350, 200, 100), "Здоровье");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 330, 200, 100), "Стамина");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 310, 200, 100), "Мана");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 290, 200, 100), "Резисты");

            GUI.Button(new Rect(Screen.width / 2, Screen.width / 2 - 350, 15, 15), "+");
            GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 350, 15, 15), "-");

            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.width / 2 - 350, 15, 15), HPpoint.ToString());

        }


    }
}
