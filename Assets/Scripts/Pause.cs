﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool isPaused = false;
    GameObject player;
    public GameObject pauseMenu;
    AudioSource[] audioSources;
    //HashSet<AudioSource> playingAudioSources;


    private void Awake()
    {
        
        //HashSet<AudioSource> playingAudioSources = new HashSet<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.playerDead)
        {
          
            if (GameManager.Instance.isPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void pauseAudio()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in audioSources)
        {
            
            audioS.Pause();
          
        }
    }

    void StopAudio()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in audioSources)
        {
    
                audioS.Stop();

        }
    }

    void unpauseAudio()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in audioSources)
        {

            audioS.UnPause();

        }
    }

    public void Resume()
    {
        FindObjectOfType<SoundManager>().Play("ButtonClick");
        unpauseAudio();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        GameManager.Instance.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void  QuitGame()
    {
        FindObjectOfType<SoundManager>().Play("ButtonClick");
        GameManager.Instance.isPaused = false;
        GameManager.Instance.playerDead = false;
        Time.timeScale = 1f;
        StopAudio();
        FindObjectOfType<SoundManager>().StopFade("Music");
        FindObjectOfType<SoundManager>().StopFade("ChaseMusic");
        GameManager.Instance.killedBy = "nothing";
        GameManager.Instance.nextScene = 1;
        SceneManager.LoadScene(2);
        GameManager.Instance.eventNumber = 1;
       
    }

    void PauseGame()
    {
     
        pauseAudio();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        GameManager.Instance.isPaused = true;
    }

    public void Restart()
    {
        FindObjectOfType<SoundManager>().Play("ButtonClick");
        GameManager.Instance.isPaused = false;
        GameManager.Instance.playerDead = false;
        StopAudio();
        Time.timeScale = 1;
        GameManager.Instance.killedBy = "nothing";
        GameManager.Instance.nextScene = 3;
        SceneManager.LoadScene(2);
        GameManager.Instance.eventNumber++;
    }
}
