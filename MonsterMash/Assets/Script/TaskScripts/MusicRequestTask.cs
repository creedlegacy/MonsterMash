using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MusicRequestTask : MonoBehaviour
{
    //[System.Serializable]

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public float initialCountdown = 5f, taskActivatedCountdown = 10f;
    public string requestedShape;
    private float tempInitialCountdown = 5f, tempTaskActivatedCountdown = 10f;
    private bool collidedPlayer = false;
    public bool inDanger = false, taskActivated = false;
    private GameObject successReaction, failReaction, countdownDial, countdownDialFill, taskAlert, bubble, shape;

    PartyManager pm;
    MusicPlayer mp;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        mp = FindObjectOfType<MusicPlayer>();
        successReaction = gameObject.transform.Find("TaskSuccessReaction").gameObject;
        failReaction = gameObject.transform.Find("TaskFailReaction").gameObject;
        countdownDial = gameObject.transform.Find("Canvas/CountdownDial").gameObject;
        countdownDialFill = countdownDial.transform.Find("CountdownDialFill").gameObject;
        taskAlert = gameObject.transform.Find("TaskAlert").gameObject;
        bubble = gameObject.transform.Find("Canvas/BubbleSprite").gameObject;
        shape = gameObject.transform.Find("Canvas/Shape").gameObject;


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
                GetRandomShape();
                inDanger = false;
                taskActivated = true;
                mp.allowInteract = !mp.allowInteract;
                taskAlert.SetActive(false);
                bubble.SetActive(true);
                shape.SetActive(true);
                
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
                bubble.SetActive(false);
                shape.SetActive(false);
                failReaction.SetActive(true);
                failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
                pm.partymeter.value -= decrementMeter;
                Debug.Log(pm.partymeter.value);
                //call function in music player class to turn off music player elements such as UI and returning player movement
                mp.MusicPlayerOff();
                EventOccurCoroutine();

            }
        }
    }

    private void GetRandomShape()
    {   

        int shapeIndex = Random.Range(0,mp.ListOfShapes.Count);
        requestedShape = mp.ListOfNames[shapeIndex];
        shape.GetComponent<Image>().sprite = mp.ListOfShapes[shapeIndex];
        shape.GetComponent<Image>().color = mp.ListOfColors[shapeIndex];
  
    }

    public void MusicRequestSuccess()
    {
        taskActivated = false;
        countdownDial.SetActive(false);
        bubble.SetActive(false);
        shape.SetActive(false);
        pm.partymeter.value += incrementMeter;
        Debug.Log(pm.partymeter.value);
        successReaction.SetActive(true);
        successReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
        EventOccurCoroutine();
    }

    public void MusicRequestFail()
    {
        taskActivated = false;
        countdownDial.SetActive(false);
        bubble.SetActive(false);
        shape.SetActive(false);
        pm.partymeter.value -= incrementMeter;
        Debug.Log(pm.partymeter.value);
        failReaction.SetActive(true);
        failReaction.GetComponent<Animator>().Play("TaskReactionAnimation", -1, 0f);
        EventOccurCoroutine();
    }

    //private void OnCollisionEnter2D(Collision2D collision)

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.tag == "Player")
        {

            collidedPlayer = true;
            if (taskActivated)
            {
                bubble.SetActive(true);
                shape.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collidedPlayer = false;
            bubble.SetActive(false);
            shape.SetActive(false);
        }
        
    }
}
