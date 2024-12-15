using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator anim;
    public float distanceToOpen;

    private PlayerController thePlayer;

    public string LevelToLoad;

    private bool playerExiting;
    
    public Transform exitPoint;
    public float movePlayerSpeed;

    void Start()
    {
        thePlayer = PlayerHealthController.instance.GetComponent<PlayerController>();
        
    }

    
    void Update()
    {
        if (Vector3.Distance(transform.position,thePlayer.transform.position) < distanceToOpen)
        {
            anim.SetBool("DoorOpen", true);
        }
        else
        {
            anim.SetBool("DoorOpen", false);
        }


        if (playerExiting)
        {
            thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position, exitPoint.position,movePlayerSpeed*Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player")
        {
            if (!playerExiting)
            {
                thePlayer.canMove = false;

                StartCoroutine(UseDoorCo());
            }
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExiting = true;
        thePlayer.anim.enabled = false;
        //Screen transition to black
        UIController.instance.StartFadeToBlack();


        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawn(exitPoint.position);
        thePlayer.canMove = true;
        thePlayer.anim.enabled = true;

        UIController.instance.StartFadeFromBlack();
        //Screen transition to scene
        SceneManager.LoadScene(LevelToLoad);
    }
}
