using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TaskController : MonoBehaviour
{
    [System.Serializable]
   
    public enum TaskType
    {
        Repeating,
        Random
    }
    public TaskType taskType;

    public enum TaskAction
    {
        Come_and_Interact
    }
    public TaskAction taskAction;

    public int minOccurTime = 5;
    public int maxOccurTime = 15;
    public int decrementMeter = 5;
    public int incrementMeter = 10;

    public bool inDanger = false;
    
   
    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = GetComponent<SpriteRenderer>();
        if (taskType == TaskType.Repeating)
        {
            pm.RepeatingActionCoroutine('+', incrementMeter);
            EventOccurCoroutine();


        }
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
        pm.StopRepeatingActionCoroutine();
        pm.RepeatingActionCoroutine('-', decrementMeter);

    }

    public void PlayerInteracted()
    {
        if (inDanger)
        {
            sr.color = Color.white;
            inDanger = false;
            EventOccurCoroutine();
            pm.StopRepeatingActionCoroutine();
            pm.RepeatingActionCoroutine('+', incrementMeter);
        }

    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}
