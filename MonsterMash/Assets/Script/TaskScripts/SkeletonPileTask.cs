using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkeletonPileTask : MonoBehaviour
{
    [System.Serializable]
    //The variables inside the StageModifiers class is used to define what value is used on that specific stage
    public class StageModifiers
    {
        public int minOccurTime = 0, maxOccurTime = 0, decrementMeter = 0, incrementMeter = 0;
        public float sprintTime = 0f;

    }
    [Header("Variable Values by Stage")]
    public StageModifiers stage1Modifiers;
    public StageModifiers stage2Modifiers;
    public StageModifiers stage3Modifiers;
    public StageModifiers stage4Modifiers;

    [Header("Task Variables")]
    public string requiredItemName;
    public int currentMinOccurTime = 5, currentMaxOccurTime = 15, currentDecrementMeter = 5, currentIncrementMeter = 10;
    private int currentStage;
    public float currentSprintTime = 3f;
    private bool collidedPlayer = false;
    public bool inDanger = false, isTutorial = false;
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


        CheckStage();

        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTaskInteract();
        CheckStage();
    }

    public void EventOccurCoroutine()
    {
        StartCoroutine(EventOccur());
    }

    //Checks what the current stage of the party is
    void CheckStage()
    {
        if(pm.currentStage == PartyManager.Stage.stage1)
        {
            if(currentStage != 1)
            {
                currentStage = 1;

                currentMinOccurTime = stage1Modifiers.minOccurTime;
                currentMaxOccurTime = stage1Modifiers.maxOccurTime;
                currentDecrementMeter = stage1Modifiers.decrementMeter;
                currentIncrementMeter = stage1Modifiers.incrementMeter;
                currentSprintTime = stage1Modifiers.sprintTime;
            }
            
        }
        else if(pm.currentStage == PartyManager.Stage.stage2)
        {
            if (currentStage != 2)
            {
                currentStage = 2;

                currentMinOccurTime = stage2Modifiers.minOccurTime;
                currentMaxOccurTime = stage2Modifiers.maxOccurTime;
                currentDecrementMeter = stage2Modifiers.decrementMeter;
                currentIncrementMeter = stage2Modifiers.incrementMeter;
                currentSprintTime = stage2Modifiers.sprintTime;
            }
        }
        else if (pm.currentStage == PartyManager.Stage.stage3)
        {
            if (currentStage != 3)
            {
                currentStage = 3;

                currentMinOccurTime = stage3Modifiers.minOccurTime;
                currentMaxOccurTime = stage3Modifiers.maxOccurTime;
                currentDecrementMeter = stage3Modifiers.decrementMeter;
                currentIncrementMeter = stage3Modifiers.incrementMeter;
                currentSprintTime = stage3Modifiers.sprintTime;
            }
        }
        else if (pm.currentStage == PartyManager.Stage.stage4)
        {
            if (currentStage != 4)
            {
                currentStage = 4;

                currentMinOccurTime = stage4Modifiers.minOccurTime;
                currentMaxOccurTime = stage4Modifiers.maxOccurTime;
                currentDecrementMeter = stage4Modifiers.decrementMeter;
                currentIncrementMeter = stage4Modifiers.incrementMeter;
                currentSprintTime = stage4Modifiers.sprintTime;
            }
        }
    }

    IEnumerator EventOccur()
    {
        
        int randomOccurTime = Random.Range(currentMinOccurTime, currentMaxOccurTime);
        yield return new WaitForSeconds(randomOccurTime);
        inDanger = true;
        
        failReaction.SetActive(true);
        skeletonSprite.GetComponent<SpriteRenderer>().sprite = spriteChange;
        ContinuousActionCoroutine('-', currentDecrementMeter);
        
     
            
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
                pm.partymeter.value += currentIncrementMeter;

                pc.sprintMeter.value += currentSprintTime;
   
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
                failReaction.SetActive(false);
                successReaction.SetActive(true);
                successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                if (pm.partymeter.value < pm.partymeter.maxValue)
                {
                    pm.partymeter.value += value;
                  
                }

            }
            else
            {
                successReaction.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                if (pm.partymeter.value > pm.partymeter.minValue)
                {

                    pm.partymeter.value -= value;
 
                }
               
            }
               
            Debug.Log(pm.partymeter.value);
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
