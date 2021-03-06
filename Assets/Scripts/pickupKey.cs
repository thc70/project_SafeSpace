//Make an empty GameObject and call it "Door"
//Drag and drop your Door model into Scene and rename it to "Body"
//Make sure that the "Door" Object is at the side of the "Body" object (The place where a Door Hinge should be)
//Move the "Body" Object inside "Door"
//Add a Collider (preferably SphereCollider) to "Door" object and make it much bigger then the "Body" model, mark it as Trigger
//Assign this script to a "Door" Object (the one with a Trigger Collider)
//Make sure the main Character is tagged "Player"
//Upon walking into trigger area press "F" to open / close the door

using TMPro;
using UnityEngine;

public class pickupKey : MonoBehaviour
{

    // pick up keycard

    int numberOfCards = 0;
    public GameObject item;
    public GameObject itemPlayer;
    public PlayerInfo playerInfo;
    private TextMeshProUGUI interact;

    bool enter = false;

    private void Start()
    {
        interact = GameObject.Find("GameUI").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }


    // Main function
    void Update()
    {
        if (GameManager.Instance.playerDead)
        {
            this.enabled = false;
        }

      if (Input.GetKeyDown(KeyCode.F) && enter && !GameManager.Instance.isPaused && !GameManager.Instance.playerDead)
      {
        
        itemPlayer.GetComponent<ItemPlayer>().pickedUp = true;
        item.SetActive(false);
        numberOfCards++;
        playerInfo.collectedKey();
    
        FindObjectOfType<SoundManager>().Play("PickUp");

        }
    }

    // Display a simple info message when player is inside the trigger area
    /*void OnGUI()
    {
        if (enter && !GameManager.Instance.isPaused && !GameManager.Instance.playerDead)
        {
            Rect label = new Rect((Screen.width - 210) / 2, Screen.height - 100, 210, 50);
            GUI.Label(label, "Press 'F' to pick up keycard", GameManager.Instance.style);

        }
    }*/

    // Activate the Main function when Player enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interact.SetText("Press 'F' to pick up keycard");
            enter = true;
        }
    }

    // Deactivate the Main function when Player exit the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interact.SetText("");
            enter = false;
        }
    }

    private void OnDisable()
    {
        interact.SetText("");
    }

}
