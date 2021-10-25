using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiscoBallTask : MonoBehaviour
{

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public bool inDanger = false;
    private IEnumerator continuousActionCoroutine;
    private GameObject successReaction, failReaction;

    PartyManager pm;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;

        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();
        // If continuous start adding party score on start per second
        successReaction.SetActive(true);
        ContinuousActionCoroutine('+', incrementMeter); 
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
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
        StopContinuousActionCoroutine();
        failReaction.SetActive(true);
        ContinuousActionCoroutine('-', decrementMeter);
        
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

}
