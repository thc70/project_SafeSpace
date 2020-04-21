﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int health = 100;
    public bool isDead = false;
    public GameObject objective;
    Objectives objectiveScript;

    private void Start()
    {
        objectiveScript = objective.GetComponent<Objectives>();
        //objectiveScript.popNotification("Escape!");
    }

    void Update()
    {
      
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            //ApplyDamage(30);
            objectiveScript.collectedKey(2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //ApplyDamage(30);
            objectiveScript.collectedKey(0);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //ApplyDamage(30);
            objectiveScript.collectedKey(1);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            //ApplyDamage(30);
            objectiveScript.collectedKey(3);
        }*/
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Dead();
        }

    }

    void Dead()
    {
        Debug.Log("Player Dead.");
        isDead = true;
    }
}
