using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiscoBallSwitch : MonoBehaviour
{
    private bool collidedPlayer = false;
    [HideInInspector]
    public Animator switchHandleAnim;
    DiscoBallTask discoBallTask;
    [HideInInspector]
    public AudioSource audioSource;

    void Start()
    {
        discoBallTask = FindObjectOfType<DiscoBallTask>();
        audioSource = GetComponent<AudioSource>();
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
                audioSource.Play();
                discoBallTask.audioSource.clip = discoBallTask.successSFX;
                discoBallTask.audioSource.Play();
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
                    discoBallTask.discoLights.SetActive(true);
                    discoBallTask.discoBallAnim.SetBool("DiscoBallOn", true);
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
