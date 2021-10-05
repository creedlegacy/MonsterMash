using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractTask : MonoBehaviour
{
    //[System.Serializable]
   
    //public enum TaskType
    //{
    //    Repeating,
    //    Random
    //}
    //public TaskType taskType;

    //public enum TaskAction
    //{
    //    Come_and_Interact
    //}
    //public TaskAction taskAction;

    public int minOccurTime = 5;
    public int maxOccurTime = 15;
    public int decrementMeter = 5;
    public int incrementMeter = 10;
    private bool collidedPlayer = false;
    public bool inDanger = false;
    private IEnumerator repeatingActionCoroutine;


    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = GetComponent<SpriteRenderer>();
        //if (taskType == TaskType.Repeating)
        //{
            RepeatingActionCoroutine('+', incrementMeter);
            EventOccurCoroutine();


        //}
    }

    public void EventOccurCoroutine()
    {
        StartCoroutine(EventOccur());
    }

    IEnumerator EventOccur()
    {
        
        int randomOccurTime = Random.Range(minOccurTime, maxOccurTime);
        yield return new WaitForSeconds(randomOccurTime);
        sr.color = Color.red;
        inDanger = true;
        StopRepeatingActionCoroutine();
        RepeatingActionCoroutine('-', decrementMeter);

    }


    // Update is called once per frame
    void Update()
    {
        PlayerTaskInteract();
    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && inDanger)
            {
              
                sr.color = Color.white;
                inDanger = false;
                EventOccurCoroutine();
                StopRepeatingActionCoroutine();
                RepeatingActionCoroutine('+', incrementMeter);
                
            }
        }
    }

<<<<<<< HEAD
    public void RepeatingActionCoroutine(char type, int value)
    {
        repeatingActionCoroutine = RepeatingAction(type, value);
        StartCoroutine(repeatingActionCoroutine);
    }

    public void StopRepeatingActionCoroutine()
    {
        StopCoroutine(repeatingActionCoroutine);
    }

    IEnumerator RepeatingAction(char type, int value)

    {
        while (pm.continueCoroutine)
        {
            if (type == '+')
                pm.partyMeterValue += value;
            else
                pm.partyMeterValue -= value;
            pm.partymeter.value = pm.partyMeterValue;
            Debug.Log(pm.partymeter.value);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
=======
    private void OnTriggerEnter2D(Collider2D collision)
>>>>>>> origin/YiboP
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
