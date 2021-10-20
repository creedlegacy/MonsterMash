using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkeletonPileTask : MonoBehaviour
{

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public string requiredItemName;
    private bool collidedPlayer = false;
    public bool inDanger = false;
    private IEnumerator continuousActionCoroutine;
    private GameObject successReaction, failReaction, skeletonSprite;
    public Sprite spriteChange;

    PartyManager pm;
    PlayerController pc;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        pc = FindObjectOfType<PlayerController>();

        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;
        skeletonSprite = gameObject.transform.Find("TaskSprite").gameObject;

        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();
       
        
    }

    // Update is called once per frame
    void Update()
    {
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
        
        failReaction.SetActive(true);
        skeletonSprite.GetComponent<SpriteRenderer>().sprite = spriteChange;
        ContinuousActionCoroutine('-', decrementMeter);
        
     
            
    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            //check if there is an item being carried and that item is appropriate
            if (Input.GetButtonDown("Interact") && inDanger && pc.pickupFull && pc.pickupItemName == requiredItemName)
            {
                StopContinuousActionCoroutine();
                inDanger = false;
                skeletonSprite.GetComponent<SpriteRenderer>().sprite = null;
                pm.partymeter.value += incrementMeter;
                Debug.Log(pm.partymeter.value);
                successReaction.SetActive(true);
                successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                Destroy(gameObject,1);
               
                
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
