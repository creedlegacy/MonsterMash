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
    
   
    PartyManager pm;
    SpriteRenderer sr;

    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        sr = GetComponent<SpriteRenderer>();
        //if (taskType == TaskType.Repeating)
        //{
            pm.RepeatingActionCoroutine('+', incrementMeter);
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
        pm.StopRepeatingActionCoroutine();
        pm.RepeatingActionCoroutine('-', decrementMeter);

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
                pm.StopRepeatingActionCoroutine();
                pm.RepeatingActionCoroutine('+', incrementMeter);
                
            }
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
        if (collision.gameObject.name == "Player")
        {
            collidedPlayer = false;
        }
    }
}
