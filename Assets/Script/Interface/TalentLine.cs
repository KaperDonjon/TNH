using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentLine : MonoBehaviour
{
    public GUISkin PlayerStats;
    public PlayerStat ps;
    public PlayerStatsInterface PSI;

    public bool Visible = false;

    void Start()
    {
        ps = GameObject.Find("Char").GetComponent<PlayerStat>();
        PSI = GameObject.Find("Char").GetComponent<PlayerStatsInterface>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K)) Visible = true;
        if (ps.TSkill1)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                Debug.Log("Skill1");
            }
        }
        if (ps.SpecSkill)
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                ps.THP += 20;
            }
        }
    }

    void OnGUI()
    {
        if (Visible)
        {
            GUI.Box(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 460), " ");
            GUI.Box(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50), "War");
            GUI.Box(new Rect(Screen.width / 2 + 100, Screen.height / 2 - 25, 50, 50), "Craft");

            if (PSI.SkillPoint > 0)
            {
                if (ps.WTalent1)
                {
                }
                else
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 50, 50), "WT1"))
                    {
                        PSI.SkillPoint -= 5;
                        ps.WTalent1 = true;
                        ps.MaxHP += 100;
                    }
                }
            }
            if (PSI.SkillPoint > 0)
            {
                if (ps.WTalent2)
                {

                }
                else
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 100, 50, 50), "WT2"))
                    {
                        PSI.SkillPoint -= 5;
                        ps.WTalent2 = true;
                        ps.MaxMana += 100;
                    }
                }
            }
            if (PSI.SkillPoint > 0)
            {
                if (ps.CTalent1)
                {
                }
                else
                {
                    if (GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height / 2 - 100, 50, 50), "CT1"))
                    {
                        PSI.SkillPoint -= 5;
                        ps.CTalent1 = true;
                        ps.Resist += 2;
                    }
                }
            }
            if(PSI.SkillPoint > 0)
            {
                if (ps.WTalent1)
                {
                    if (ps.WTalent3)
                    {
                    }
                    else
                    {
                        if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 25, 50, 50), "WT3"))
                        {
                            PSI.SkillPoint -= 5;
                            ps.WTalent3 = true;
                            ps.Resist += 5;
                        }
                    }
                }
            }
            if(PSI.SkillPoint > 0)
            {
                if (ps.WTalent2)
                {
                    if (ps.TSkill1)
                    {

                    }
                    else
                    {
                        if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 150, 50, 50), "Skill1"))
                        {
                            PSI.SkillPoint -= 10;
                            ps.TSkill1 = true;

                        }
                    }
                }
            }  
            if(PSI.SkillPoint > 0)
            {
                if (ps.WTalent2)
                {
                    if (ps.WTalent1)
                    {
                        if (ps.CTalent1)
                        {
                            if (ps.SpecSkill)
                            {

                            }
                            else
                            {
                                if (GUI.Button(new Rect(Screen.width / 2 -300, Screen.height / 2 - 300, 50, 50), "Skill1"))
                                {
                                    PSI.SkillPoint -= 10;
                                    ps.SpecSkill = true;
                                }
                            }
                        }
                    }
                }

            }
            





























            //Выход
            if (GUI.Button(new Rect(Screen.width / 2 + 200, Screen.height / 2 + 100, 80, 30), "Выйти"))
            {
                Visible = false;
            }


        }
        


    }


}
