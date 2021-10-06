using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractTask : MonoBehaviour
{
    //[System.Serializable]

    public enum TaskType
    {
        Repeating,
        Random
    }
    public TaskType taskType;

    public int minOccurTime = 5, maxOccurTime = 15, decrementMeter = 5, incrementMeter = 10;
    public float randomCountdown = 3f;
    private float tempRandomCountdown = 3f;
    private bool collidedPlayer = false;
    public bool inDanger = false;
    private IEnumerator repeatingActionCoroutine;


    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = GetComponent<SpriteRenderer>();
        // Start courutine to determine how many seconds until event for this task
        EventOccurCoroutine();
        // If repeating start adding party score on start per second
        if (taskType == TaskType.Repeating)
        {
            RepeatingActionCoroutine('+', incrementMeter); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        RandomTaskCountdown();
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
        if (taskType == TaskType.Repeating)
        {
            sr.color = Color.red;
            StopRepeatingActionCoroutine();
            RepeatingActionCoroutine('-', decrementMeter);
        }
        else
        {
            sr.color = Color.magenta;
            tempRandomCountdown = randomCountdown;
        }
            
    }

    void PlayerTaskInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && inDanger)
            {
                inDanger = false;
                if (taskType == TaskType.Repeating)
                {
                    sr.color = Color.white;
                    StopRepeatingActionCoroutine();
                    RepeatingActionCoroutine('+', incrementMeter);
                }
                else
                {
                    sr.color = Color.black;
                    pm.partymeter.value += incrementMeter;
                    Debug.Log(pm.partymeter.value);
                }
                
                
                EventOccurCoroutine();
               
                
            }
        }
    }

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
            {
                if (pm.partyMeterValue < pm.partymeter.maxValue)
                {
                    pm.partyMeterValue += value;
                    if(pm.partyMeterValue > pm.partymeter.maxValue)
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

    private void RandomTaskCountdown()
    {
        if (inDanger && taskType == TaskType.Random)
        {
            if (tempRandomCountdown > 0)
            {
                tempRandomCountdown -= Time.deltaTime;

            }
            else
            {
                inDanger = false;
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
