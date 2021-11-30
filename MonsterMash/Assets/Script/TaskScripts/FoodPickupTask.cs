using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FoodPickupTask : MonoBehaviour
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

    [Header("Food Selections")]
    public List<string> foodNames = new List<string>();
    public List<Sprite> foodSprites = new List<Sprite>();

    [Header("Task Variables")]
    public string requiredItemName;
    private int currentStage;
    public int currentMinOccurTime = 5, currentMaxOccurTime = 15, currentDecrementMeter = 5, currentIncrementMeter = 10;
    public float currentSprintTime = 5f, initialCountdown = 5f;
    private float tempInitialCountdown = 5f;
    private bool collidedPlayer = false;
    [HideInInspector]
    public bool tutorialFinished = false;
    public bool inDanger = false, taskActivated = false, isTutorial = false;
    private GameObject successReaction, failReaction, countdownDial, countdownDialFill, bubble, food;

    private AudioSource audioSource;
    [Header("Sound")]
    public AudioClip successSFX;
    public AudioClip signalSFX;

    PartyManager pm;
    MusicPlayer mp;
    PlayerController pc;
    SoundManager sm;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        pm = FindObjectOfType<PartyManager>();
        mp = FindObjectOfType<MusicPlayer>();
        pc = FindObjectOfType<PlayerController>();
        sm = FindObjectOfType<SoundManager>();

        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;
        countdownDial = gameObject.transform.Find("Canvas/CountdownDial").gameObject;
        countdownDialFill = countdownDial.transform.Find("CountdownDialFill").gameObject;
        bubble = gameObject.transform.Find("Canvas/BubbleSprite").gameObject;
        food = gameObject.transform.Find("Canvas/BubbleSprite/FoodSprite").gameObject;



        //Checks what the current stage of the party is
        CheckStage();
        // Start courutine to determine how many seconds until event for this task
        if (!isTutorial)
        {
            EventOccurCoroutine();
        }

    }

    // Update is called once per frame
    void Update()
    {
        InitialTaskCountdown();
        //TaskActivatedCountdown();
        PlayerTaskInteract();
        CheckStage();
    }


    //Checks what the current stage of the party is
    void CheckStage()
    {
        if (pm.currentStage == PartyManager.Stage.stage1)
        {
            if (currentStage != 1)
            {
                currentStage = 1;

                currentMinOccurTime = stage1Modifiers.minOccurTime;
                currentMaxOccurTime = stage1Modifiers.maxOccurTime;
                currentDecrementMeter = stage1Modifiers.decrementMeter;
                currentIncrementMeter = stage1Modifiers.incrementMeter;
                currentSprintTime = stage1Modifiers.sprintTime;
            }

        }
        else if (pm.currentStage == PartyManager.Stage.stage2)
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

    public void EventOccurCoroutine()
    {
        StartCoroutine(EventOccur());
    }

    IEnumerator EventOccur()
    {
        
        int randomOccurTime = Random.Range(currentMinOccurTime, currentMaxOccurTime);
        yield return new WaitForSeconds(randomOccurTime);
        GetRandomFood();
        inDanger = true;
        tempInitialCountdown = initialCountdown;
        bubble.SetActive(true);
        bubble.GetComponent<Animator>().Play("FoodRequestAnimation", -1, 0f);
        audioSource.clip = signalSFX;
        audioSource.Play();

    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && inDanger && pc.pickupFull && pc.pickupItemName != "broom")
            {
                if(pc.pickupItemName == requiredItemName)
                {
                    audioSource.clip = successSFX;
                    audioSource.Play();
                    

                    pm.partymeter.value += currentIncrementMeter;
                    pc.sprintMeter.value += currentSprintTime;

                    Debug.Log(pm.partymeter.value);
                    successReaction.SetActive(true);
                    successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);


                    //Declare this task's tutorial is finished because succesfully dealt with
                    if (isTutorial)
                    {
                        tutorialFinished = true;
                    }
                    else
                    {
                        pm.foodDone++;
                        EventOccurCoroutine();
                    }
                }
                else
                {

                    pm.partymeter.value -= currentDecrementMeter;

                    Debug.Log(pm.partymeter.value);
                    failReaction.SetActive(true);
                    failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);

                    EventOccurCoroutine();

                }
                inDanger = false;
                countdownDial.SetActive(false);
                bubble.SetActive(false);
                pc.EmptyHand();
                

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
                bubble.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                pm.partymeter.value -= currentDecrementMeter;
                Debug.Log(pm.partymeter.value);
                EventOccurCoroutine();

            }
        }
    }

    private void GetRandomFood()
    {   

        int foodIndex = Random.Range(0,foodNames.Count);
        requiredItemName = foodNames[foodIndex];
        food.GetComponent<Image>().sprite = foodSprites[foodIndex];
  
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
