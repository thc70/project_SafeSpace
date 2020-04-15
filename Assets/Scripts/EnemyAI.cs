﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public AudioClip [] screams;
    public AudioClip[] footsteps;
    NavMeshAgent agent;
    Transform player;
    Animator anim;
    public AudioClip footSounds;
    public AudioSource sound;
    public AudioSource soundFoot;
    string state = "idle";
    public Transform vision;
    float waitSearch = 0f;
    float chaseTime = 0f;
    bool highAlert = false;
    float searchRadius = 20f;
    public GameObject deathcam;
    public Transform camPos;
    public Camera mainCamera;
    Rigidbody BossRb;
    Rigidbody PlayerRb;
    bool searching = false;
    //TODO: Change tags of walls to "Barrier" in the final version of the maps

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 1.2f;
        agent.updateRotation = true;
        BossRb = GetComponent<Rigidbody>();
        PlayerRb = player.GetComponent<Rigidbody>();



    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log(waitSearch);

       // Debug.Log(searchRadius);
        Debug.Log(state);
        Debug.DrawLine(vision.position, player.transform.position, Color.green);
        anim.SetFloat("velocity", agent.desiredVelocity.magnitude);
        // Debug.Log(agent.velocity.magnitude);


     

        if (state == "idle")
        {
            Vector3 randomPos = Random.insideUnitSphere * searchRadius;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

            if (highAlert)
            {
                NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                searchRadius += 2.5f;

                if (searchRadius > 20f)
                {
                    Debug.Log("WWWWWWWWWWWWWWWWWOOOOOOOOOOOOOOOOOOWWWWWWWWWWWWWWWW");
                    highAlert = false;
                    agent.speed = 1.2f;
                }
            }
            agent.SetDestination(navHit.position);
            agent.isStopped = false;
            state = "walk";
        }

        if (state == "walk")
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                state = "search";
                agent.isStopped = true;
                waitSearch = 5f;
            }


        }

        if (state == "search")//How long he stops for and looks around
        {
            RaycastHit hit;
            if (Physics.Raycast(vision.position, vision.forward, out hit, 3.5f))
            {
                if(hit.collider.gameObject.tag == "Barrier")
                {
                    Debug.Log("SOmewhere else");
                    //waitSearch = 5f;
                    searching = false;
                    state = "idle";
                }
            }
            

            if (waitSearch > 0f)
            {
                searching = true;
                waitSearch -= Time.deltaTime;
            }
            else
            {
                searching = false;
                state = "idle";
            }
        }

        if (state == "shout")
        {
            agent.ResetPath();
            anim.SetTrigger("scream");
            playScream(Random.Range(0, 4));
            state = "shouting";
 

        }

        if (state == "chase")
        {
            agent.speed = 4f;
            chaseTime -= Time.deltaTime;
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance > 25f || chaseTime <= 0)
            {
                state = "hunt";
            }

            else if (distance <= 2.3f)//TODO: alter this number for refinment (or adjust camPos position) depending on how tall the player is and how far the boss can reach. Stopping distance too
            {
                RaycastHit hit;
                if (Physics.Linecast(vision.position, player.transform.position, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        agent.isStopped = true;
                        agent.ResetPath();
                        GetComponent<Rigidbody>().freezeRotation = true;
                        BossRb.velocity = Vector3.zero;
                        BossRb.angularVelocity = Vector3.zero;
                        transform.LookAt(player.transform.position);
            
                        state = "kill";
                        player.GetComponent<FirstPersonAIO>().enabled = false;
                        PlayerRb.velocity = Vector3.zero;
                        PlayerRb.angularVelocity = Vector3.zero;
                        // deathcam.SetActive(true);
                        //deathcam.transform.position = Camera.main.transform.position;
                        //deathcam.transform.rotation = Camera.main.transform.rotation;
                        //Camera.main.gameObject.SetActive(false);
                        anim.SetTrigger("AttackPlayer");
                        anim.speed = .8f;
                    }
                }
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
            //deathcam.transform.position = Vector3.Slerp(deathcam.transform.position, camPos.position, 20f * Time.deltaTime);
            //deathcam.transform.rotation = Quaternion.Slerp(deathcam.transform.rotation, camPos.rotation, 20f * Time.deltaTime);
            Quaternion lookOnLook = Quaternion.LookRotation(camPos.transform.position - player.transform.position);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, lookOnLook, 5f * Time.deltaTime);

        }




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
            

                if(state == "search" || state == "walk")
                {
                    state = "shout";
                    
               }
            }
        }
    }

    public void footstep(int num)
    {
      
       soundFoot.PlayOneShot(footsteps[Random.Range(0,5)]);
        
    }



    public void endShout()
    {
        chaseTime = 15f;
        state = "chase";
    }

    public void playScream(int num)
    {
        sound.clip = screams[num];
        sound.Play();
    }

    public void hitByFlare()
    {
        anim.SetTrigger("hit");
        if (state != "stay")
        {
            agent.ResetPath();
            agent.isStopped = true;
            BossRb.velocity = Vector3.zero;
            BossRb.angularVelocity = Vector3.zero;
            state = "stay";
            Invoke("endHit", 15f);
        }
    }

    public void endHit()
    {
        agent.isStopped = false;
        searchRadius = 21f;
        highAlert = true;
        state = "idle";
        anim.SetTrigger("backToIdle");
    }

    public string getState()
    {
        return state;
    }
}
