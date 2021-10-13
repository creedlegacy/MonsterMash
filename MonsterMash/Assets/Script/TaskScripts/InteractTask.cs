using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractTask : MonoBehaviour
{
    //[System.Serializable]

    public enum TaskType
    {
        Instant,
        Continuous
    }
    public TaskType taskType;

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public float instantCountdown = 3f;
    private float tempInstantCountdown = 3f;
    private bool collidedPlayer = false;
    public bool inDanger = false;
    private IEnumerator continuousActionCoroutine;
    private GameObject successReaction, failReaction, countdownDial, countdownDialFill;


    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = GetComponent<SpriteRenderer>();
        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;
        countdownDial = gameObject.transform.Find("Canvas/CountdownDial").gameObject;
        countdownDialFill = countdownDial.transform.Find("CountdownDialFill").gameObject;
        //countdownDialFill.GetComponent<Image>().fillAmount = 
        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();
        // If continuous start adding party score on start per second
        if (taskType == TaskType.Continuous)
        {
            successReaction.SetActive(true);
            ContinuousActionCoroutine('+', incrementMeter); 
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        InstantTaskCountdown();
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
        if (taskType == TaskType.Continuous)
        {
            sr.color = Color.red;
            StopContinuousActionCoroutine();
            failReaction.SetActive(true);
            ContinuousActionCoroutine('-', decrementMeter);
        }
        else
        {
            sr.color = Color.magenta;
            tempInstantCountdown = instantCountdown;
        }
            
    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && inDanger)
            {
                inDanger = false;
                if (taskType == TaskType.Continuous)
                {
                    sr.color = Color.white;
                    StopContinuousActionCoroutine();
                    ContinuousActionCoroutine('+', incrementMeter);

                    
                }
                else
                {
                    countdownDial.SetActive(false);
                    sr.color = Color.black;
                    pm.partymeter.value += incrementMeter;
                    Debug.Log(pm.partymeter.value);
                    successReaction.SetActive(true);
                    successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                }
                
                
                EventOccurCoroutine();
               
                
            }
        }
    }

    public void ContinuousActionCoroutine(char type, int value)
    {
        continuousActionCoroutine = ContinuousAction(type, value);
        StartCoroutine(continuousActionCoroutine);
    }

    public void StopContinuousActionCoroutine()
    {
        StopCoroutine(continuousActionCoroutine);
    }

    IEnumerator ContinuousAction(char type, int value)

    {
        while (pm.continueCoroutine)
        {
            if (type == '+')
            {
                if (pm.partyMeterValue < pm.partymeter.maxValue)
                {
                    pm.partyMeterValue += value;
                    failReaction.SetActive(false);
                    successReaction.SetActive(true);
                    successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);

                    if (pm.partyMeterValue > pm.partymeter.maxValue)
                    {
                        pm.partyMeterValue = (int)pm.partymeter.maxValue;
                    }
                }
            }
            else
            {
                if (pm.partyMeterValue > pm.partymeter.minValue)
                {
                    pm.partyMeterValue -= value;
                    successReaction.SetActive(false);
                    failReaction.SetActive(true);
                    failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);

                    if (pm.partyMeterValue < pm.partymeter.minValue)
                    {
                        pm.partyMeterValue = (int)pm.partymeter.minValue;
                    }
                }
            }
               
            pm.partymeter.value = pm.partyMeterValue;
            Debug.Log(pm.partyMeterValue);
            yield return new WaitForSeconds(1f);
        }
    }

    private void InstantTaskCountdown()
    {
        if (inDanger && taskType == TaskType.Instant)
        {
            if (tempInstantCountdown > 0)
            {
                tempInstantCountdown -= Time.deltaTime;
                countdownDial.SetActive(true);
                //Lerp to linearly interpolate between 1 to 0
                countdownDialFill.GetComponent<Image>().fillAmount = 1 - Mathf.Lerp(1, 0, tempInstantCountdown / instantCountdown);
            }
            else
            {
                inDanger = false;
                countdownDial.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                sr.color = Color.black;
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
