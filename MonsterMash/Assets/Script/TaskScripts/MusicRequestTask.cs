using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicRequestTask : MonoBehaviour
{
    //[System.Serializable]

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public float initialCountdown = 5f, taskActivatedCountdown = 10f;
    private float tempInitialCountdown = 5f, tempTaskActivatedCountdown = 10f;
    private bool collidedPlayer = false;
    public bool inDanger = false, taskActivated = false;
    private IEnumerator continuousActionCoroutine;
    private GameObject successReaction, failReaction, countdownDial, countdownDialFill, taskAlert;

    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = gameObject.transform.Find("TaskSprite").gameObject.GetComponent<SpriteRenderer>();
        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;
        countdownDial = gameObject.transform.Find("Canvas/CountdownDial").gameObject;
        countdownDialFill = countdownDial.transform.Find("CountdownDialFill").gameObject;
        taskAlert = gameObject.transform.Find("TaskAlert").gameObject;
        

        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();


    }

    // Update is called once per frame
    void Update()
    {
        InitialTaskCountdown();
        TaskActivatedCountdown();
        PlayerTaskInteract();
    }

    public void EventOccurCoroutine()
    {
        StartCoroutine(EventOccur());
    }

    IEnumerator EventOccur()
    {
        
        int randomOccurTime = Random.Range(minOccurTime, maxOccurTime);
        yield return new WaitForSeconds(randomOccurTime);
        inDanger = true;
        tempInitialCountdown = initialCountdown;
        tempTaskActivatedCountdown = taskActivatedCountdown;
        taskAlert.SetActive(true);
        taskAlert.GetComponent<Animator>().Play("TaskAlertAnimation", -1, 0f);

    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && inDanger)
            {
                inDanger = false;
                taskActivated = true;
                taskAlert.SetActive(false);

                //countdownDial.SetActive(false);

                //pm.partymeter.value += incrementMeter;
                //Debug.Log(pm.partymeter.value);
                //successReaction.SetActive(true);
                //successReaction.GetComponent<Animator>().Play("TaskReactionMovement", -1, 0f);



            }
        }
    }

   

    private void InitialTaskCountdown()
    {
        if (inDanger)
        {
            if (tempInitialCountdown > 0)
            {
                tempInitialCountdown -= Time.deltaTime;
                countdownDial.SetActive(true);
                //Lerp to linearly interpolate between 1 to 0
                countdownDialFill.GetComponent<Image>().fillAmount = 1 - Mathf.Lerp(1, 0, tempInitialCountdown / initialCountdown);
            }
            else
            {
                inDanger = false;
                countdownDial.SetActive(false);
                taskAlert.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                pm.partymeter.value -= decrementMeter;
                Debug.Log(pm.partymeter.value);
                EventOccurCoroutine();

            }
        }
    }

    private void TaskActivatedCountdown()
    {
        if (taskActivated)
        {
            if (tempTaskActivatedCountdown > 0)
            {
                tempTaskActivatedCountdown -= Time.deltaTime;
                countdownDial.SetActive(true);
                //Lerp to linearly interpolate between 1 to 0
                countdownDialFill.GetComponent<Image>().fillAmount = 1 - Mathf.Lerp(1, 0, tempTaskActivatedCountdown / taskActivatedCountdown);
            }
            else
            {
                taskActivated = false;
                countdownDial.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                pm.partymeter.value -= decrementMeter;
                Debug.Log(pm.partymeter.value);
                EventOccurCoroutine();

            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.tag == "Player")
        {

            collidedPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collidedPlayer = false;
        }
    }
}
