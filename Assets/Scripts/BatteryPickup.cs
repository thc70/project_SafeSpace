﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public GameObject battery;
    public GameObject flashlight;

    bool enter = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && enter)
        {
            Destroy(battery);
            flashlight.GetComponent<Flashlight_PRO>().batteries += 1;
            source.PlayOneShot(clip);
        }
    }

    void OnGUI()
    {
        if (enter)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), "Press 'F' to pick up");
        }
    }

    // Activate the Main function when Player enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    // Deactivate the Main function when Player exit the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}
