using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsInterface : MonoBehaviour
{
    public GUISkin PlayerStats;
    public bool Visible = false;
    public PlayerStat Char;

    public int LvL;
    public int SkillPoint;

    public int HPpoint;
    public int StaminaPoint ;
    public int ManaPoint ;
    public int ResistPoint;

    public int THPpoint;
    public int TStaminaPoint;
    public int TManaPoint;
    public int TResistPoint;

    public int Fire;
    public int Woter;
    public int Nature;

    public int TFire;
    public int TWoter;
    public int TNature;

    public int TMaxHP;
    public int TMaxMana;
    public int TMaxStamina;
    public int TResist;

    public int TLvL = 5;
    public int TSkillPoint;

    private PlayerStat ps;


    void Start()
    {
        TLvL = LvL;

        THPpoint = HPpoint;
        TStaminaPoint = StaminaPoint;
        TManaPoint = ManaPoint;
        TResistPoint = ResistPoint;

        ps = GameObject.Find("Char").GetComponent<PlayerStat>();
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) Visible = true;

        if (TLvL != LvL)
        {
            TSkillPoint += 5;
            LvL = TLvL;
            SkillPoint = TSkillPoint;
        }



    }
    void OnGUI()
    {
        if (Visible)
        {
            GUI.skin = PlayerStats;



            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 400, 360), " ", GUI.skin.GetStyle("Box"));
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 350, 200, 100), "Здоровье");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 330, 200, 100), "Стамина");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 310, 200, 100), "Мана");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 290, 200, 100), "Резисты");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 130, 200, 100), "СкилПоинты:");
            GUI.Label(new Rect(Screen.width / 2, Screen.width / 2 - 130, 200, 100), TSkillPoint.ToString());

            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 250, 200, 100), "Огонь:");
            GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 230, 200, 100), "Вода:");
            GUI.Label(new Rect(Screen.width / 2, Screen.width / 2 - 250, 200, 100), "Природа:");

            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.width / 2 - 250, 200, 100), Fire.ToString());
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.width / 2 - 230, 200, 100), Woter.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 250, 200, 100), Nature.ToString());

            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.width / 2 - 350, 15, 15), HPpoint.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.width / 2 - 330, 15, 15), StaminaPoint.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.width / 2 - 310, 15, 15), ManaPoint.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.width / 2 - 290, 15, 15), ResistPoint.ToString());

            GUI.Label(new Rect(Screen.width / 2 + 80, Screen.width / 2 - 350, 50, 50), ps.MaxHP.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 80, Screen.width / 2 - 330, 50, 50), ps.MaxStamina.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 80, Screen.width / 2 - 310, 50, 50), ps.MaxMana.ToString());
            GUI.Label(new Rect(Screen.width / 2 + 80, Screen.width / 2 - 290, 50, 50), ps.Resist.ToString());


            //Здоровье
            if (TSkillPoint != 0)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.width / 2 - 350, 15, 15), "+"))
                {
                    TSkillPoint -= 1;
                    HPpoint += 1;
                    ps.MaxHP = ps.MaxHP + (HPpoint * 20);
                }

            }
            if (THPpoint != HPpoint)
            {
                if (GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 350, 15, 15), "-"))
                {
                    TSkillPoint += 1;
                    HPpoint -= 1;
                    ps.MaxHP = ps.MaxHP - (HPpoint * 20) - 20;
                }

            }

            //Стамина
            if (TSkillPoint != 0)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.width / 2 - 330, 15, 15), "+"))
                {
                    TSkillPoint -= 1;
                    StaminaPoint += 1;
                    ps.MaxStamina = ps.MaxStamina + (StaminaPoint * 5);
                }

            }
            if (TStaminaPoint != StaminaPoint)
            {
                if (GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 330, 15, 15), "-"))
                {
                    TSkillPoint += 1;
                    StaminaPoint -= 1;
                    ps.MaxStamina = ps.MaxStamina - (StaminaPoint * 5) - 5;
                }

            }
            //Mana
            if (TSkillPoint != 0)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.width / 2 - 310, 15, 15), "+"))
                {
                    TSkillPoint -= 1;
                    ManaPoint += 1;
                    ps.MaxMana = ps.MaxMana + (ManaPoint * 10);
                }

            }
            if (TManaPoint != ManaPoint)
            {
                if (GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 310, 15, 15), "-"))
                {
                    TSkillPoint += 1;
                    ManaPoint -= 1;
                    ps.MaxMana = ps.MaxMana - (ManaPoint * 10) - 10;
                }

            }
            //Резисты
            if (TSkillPoint != 0)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.width / 2 - 290, 15, 15), "+"))
                {
                    TSkillPoint -= 1;
                    ResistPoint += 1;
                    ps.Resist = ps.Resist + 2;
                    Fire += 10;
                    Woter += 10;
                    Nature += 10;

                }

            }
            if (TResistPoint != ResistPoint)
            {
                if (GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 290, 15, 15), "-"))
                {
                    TSkillPoint += 1;
                    ResistPoint -= 1;
                    ps.Resist = ps.Resist - 2;
                    Fire -= 10;
                    Woter -= 10;
                    Nature -= 10;
                }

            }








            //Принять
            if (GUI.Button(new Rect(Screen.width / 2 + 60, Screen.width / 2 - 150, 60, 20), "Принять"))
            {
                SkillPoint = TSkillPoint;
                THPpoint = HPpoint;
                TStaminaPoint = StaminaPoint;
                TManaPoint = ManaPoint;
                TResistPoint = ResistPoint;
                TFire = Fire;
                TWoter = Woter;
                TNature = Nature;

            }
            //Отмена
            if (GUI.Button(new Rect(Screen.width / 2 + 130, Screen.width / 2 - 150, 75, 20), "Отменить"))
            {
                HPpoint = THPpoint;
                StaminaPoint = TStaminaPoint;
                ManaPoint = TManaPoint;
                ResistPoint = TResistPoint;
                TSkillPoint = SkillPoint;

                Fire = TFire;
                Woter = TWoter;
                Nature = TNature;

                ps.MaxHP = TMaxHP; 
                ps.MaxHP = TMaxHP;
                ps.MaxMana = TMaxMana;
                ps.MaxStamina = TMaxStamina;
                ps.Resist = TResist;
            }
           






                //Выход
                if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.width / 2 - 120, 50, 25), "Выйти"))
            {
                Visible = false;
            }
        }
        //ЛвЛ
         GUI.Label(new Rect(Screen.width / 2, 20 , 400, 400), TLvL.ToString());


        //Лвл ап
        if (GUI.Button(new Rect(Screen.width / 2 + 250, Screen.width / 2 - 150, 75, 20), "+LVL"))
        {
            TLvL += 1;
        }

    }

   

}
