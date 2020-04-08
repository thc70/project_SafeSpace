﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public AudioSource scream;
    NavMeshAgent agent;
    Transform player;
    Animator anim;
    public AudioClip footSounds;
    AudioSource sound;
    string state = "idle";
    public Transform vision;
    float waitSearch = 0f;
    float chaseTime = 0f;
    bool highAlert = false;
    float searchRadius = 20f;
    public GameObject deathcam;
    public Transform camPos;


    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sound = GetComponent<AudioSource>();
        agent.speed = 1.2f;
      

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude <= 0)
        {
            agent.updateRotation = false;
            //GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            agent.updateRotation = true;
           // GetComponent<Rigidbody>().freezeRotation = false;
        }
        
        Debug.Log(state);
        Debug.DrawLine(vision.position, player.transform.position, Color.green);
        anim.SetFloat("velocity", agent.velocity.magnitude);

        if (state == "idle")
        {
            Vector3 randomPos = Random.insideUnitSphere * searchRadius;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

            if (highAlert)
            {
                NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                searchRadius += 5f;

                if (searchRadius > 20f)
                {
                    highAlert = false;
                    agent.speed = 1.2f;
                }
            }
            agent.SetDestination(navHit.position);
            state = "walk";
        }
        if (state == "walk")
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                state = "search";
                waitSearch = 4f;
            }


        }

        if (state == "search")
        {
           


            if (waitSearch > 0f)
            {
                waitSearch -= Time.deltaTime;
            }
            else
            {
                state = "idle";
            }
        }

        //TODO: Implement a shout state

        if (state == "chase")
        {
            agent.speed = 2.5f;
            //anim.speed = 3.5f;
            chaseTime -= Time.deltaTime;
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance > 10f || chaseTime <= 0)
            {
                state = "hunt";
            }

            else if (distance <= 1.7f)
            {
                agent.isStopped = true;
                GetComponent<Rigidbody>().freezeRotation = true;
                state = "kill";
                player.GetComponent<FirstPersonAIO>().enabled = false;
                deathcam.SetActive(true);
                deathcam.transform.position = Camera.main.transform.position;
                deathcam.transform.rotation = Camera.main.transform.rotation;
                Camera.main.gameObject.SetActive(false);
                //TODO: Make monster do sound
                anim.SetTrigger("AttackPlayer");
                //Invoke("reset", .884f);//TODO: Chnage this to function that displays Game over screen
            }
            sight();


        }

        if (state == "hunt")
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)//How long he stops for and looks around
            {
                state = "search";
                waitSearch = 5f;
                highAlert = true;
                searchRadius = 5f;
                sight();
            }
        }

        if (state == "kill")
        {
            deathcam.transform.position = Vector3.Slerp(deathcam.transform.position, camPos.position, 20f * Time.deltaTime);
            deathcam.transform.rotation = Quaternion.Slerp(deathcam.transform.rotation, camPos.rotation, 20f * Time.deltaTime);

        }




    }

    private void LateUpdate()
    {
        /*if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }*/
    }

    void reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //TODO: Change this to transition to game over scene
    }

    public void sight()
    {
        RaycastHit hit;

        if(Physics.Linecast(vision.position, player.transform.position, out hit))
        {
            //Debug.Log("Hit " + hit.collider.gameObject.name);

            if(hit.collider.gameObject.tag == "Player")
            {
                if(state != "chase" && state != "kill")//TODO: change this when I add a shout state
                {
                    scream.Play();

                }

                if(state != "kill")
                {
                   
                    chaseTime = 15f;
                    state = "chase";
                    
               }
            }
        }
    }

    public void footstep()
    {
        /*sound.clip = footSounds;
        sound.Play();*/
    }
}
