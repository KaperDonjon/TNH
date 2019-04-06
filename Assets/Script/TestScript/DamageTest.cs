using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public int Damage;
    public int FireDamage;


    private PlayerStat PS;
    private PlayerStatsInterface PSI;




    void Start()
    {
        PS = GameObject.Find("Char").GetComponent<PlayerStat>();
        PSI = GameObject.Find("Char").GetComponent<PlayerStatsInterface>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Работает");
            PS.THP = PS.THP - (FireDamage - PSI.Fire);
            Debug.Log(PS.THP);
            PS.TXP += 50;
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
}
