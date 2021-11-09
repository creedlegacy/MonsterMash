using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiscoBallSwitch : MonoBehaviour
{
    private bool collidedPlayer = false;
    public Animator switchHandleAnim;
    DiscoBallTask discoBallTask;

    void Start()
    {
        discoBallTask = FindObjectOfType<DiscoBallTask>();
        switchHandleAnim = gameObject.transform.Find("SwitchHandle").gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerInteract();
    }


    void PlayerInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && discoBallTask.inDanger)
            {
                
                discoBallTask.inDanger = false;
                discoBallTask.StopContinuousActionCoroutine();

                //if this disco ball is a tutorial do these steps;
                if (discoBallTask.isTutorial)
                {
                    switchHandleAnim.Play("SwitchOnAnimation");
                    discoBallTask.tutorialFinished = true;
                    discoBallTask.taskStarted = false;
                    discoBallTask.isTutorial = false;
                }
                else
                {
                    switchHandleAnim.SetBool("switchPosition", true);
                    discoBallTask.ContinuousActionCoroutine('+', discoBallTask.currentIncrementMeter, true);
                    discoBallTask.EventOccurCoroutine();

                }

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.tag == "Player")
        {

            collidedPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collidedPlayer = false;
        }
    }
}
