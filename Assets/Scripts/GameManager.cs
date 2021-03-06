﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool playerDead;
    public bool isPaused;
    public bool isEnd = false;
    public  GUIStyle style;
    public GUIStyle style2;
    public int eventNumber = 1;
     GameObject panel;
    GameObject panel2;
    float virtualWidth = 1920.0f;
    float virtualHeight = 1080.0f;
    public string killedBy;
    public int nextScene = 2;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        killedBy = "nothing";
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1.0f));
       
        Vector2 nativeSize = new Vector2(1920, 1080);
        isPaused = false;
        playerDead = false;
        isEnd = false;
        GUIStyle style = new GUIStyle();
        GUIStyle style2 = new GUIStyle();
        GUI.matrix = matrix;
       
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        style2.normal.textColor = Color.white;
        style2.alignment = TextAnchor.MiddleCenter;
    

        eventNumber = 1;
        DontDestroyOnLoad(gameObject);
    }

    public void fadeIn()
    {
        FindObjectOfType<SoundManager>().Play("BloodHit");
        FindObjectOfType<SoundManager>().StopFade("Music");
       
        FindObjectOfType<SoundManager>().StopFade("ChaseMusic");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(panel == null)
        {
            GameObject Go = GameObject.Find("GameUI").gameObject;
            panel = FindObject(Go, "Died");
           
        }
        panel.SetActive(true);
    }

     GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void fadeInBug()
    {
      
        FindObjectOfType<SoundManager>().StopFade("Music");
        FindObjectOfType<SoundManager>().StopFade("ChaseMusic");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (panel2 == null)
        {
            GameObject Go = GameObject.Find("GameUI").gameObject;
            panel2 = FindObject(Go, "BugDied");

        }
        panel2.SetActive(true);
    }



   
  
}
