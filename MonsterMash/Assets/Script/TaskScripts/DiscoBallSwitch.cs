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
    PartyManager pm;
    PlayerController pc;

    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        pm = FindObjectOfType<PartyManager>();
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
                pc.anim.SetTrigger("isInteract");
                audioSource.Play();
                discoBallTask.audioSource.clip = discoBallTask.successSFX;
                discoBallTask.audioSource.Play();
                discoBallTask.inDanger = false;
                discoBallTask.StopContinuousActionCoroutine();

                //if this disco ball is a tutorial do these steps;
                if (discoBallTask.isTutorial)
                {
                    //switchHandleAnim.Play("SwitchOnAnimation");
                    //discoBallTask.tutorialFinished = true;
                    //discoBallTask.taskStarted = false;
                    //discoBallTask.isTutorial = false;
                    StartCoroutine(WaitTutorialDiscoBall());
                }
                else
                {
                    discoBallTask.discoLights.SetActive(true);
                    discoBallTask.discoBallAnim.SetBool("DiscoBallOn", true);
                    switchHandleAnim.SetBool("switchPosition", true);
                    pm.discoDone++;
                    discoBallTask.ContinuousActionCoroutine('+', discoBallTask.currentIncrementMeter, true);
                    discoBallTask.EventOccurCoroutine();

                }

            }
        }
    }

    IEnumerator WaitTutorialDiscoBall()
    {
        discoBallTask.discoLights.SetActive(true);
        discoBallTask.discoBallAnim.SetBool("DiscoBallOn", true);
        switchHandleAnim.SetBool("switchPosition", true);
        discoBallTask.ContinuousActionCoroutine('+', discoBallTask.currentIncrementMeter, true);
        yield return new WaitForSeconds(4);
        discoBallTask.discoLights.SetActive(false);
        discoBallTask.discoBallAnim.SetBool("DiscoBallOn", false);
        switchHandleAnim.SetBool("switchPosition", false);
        audioSource.Play();
        discoBallTask.StopContinuousActionCoroutine();
        discoBallTask.tutorialFinished = true;
        discoBallTask.taskStarted = false;
        discoBallTask.isTutorial = false;
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
