using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiscoBallSwitch : MonoBehaviour
{
    private bool collidedPlayer = false;
    DiscoBallTask discoBallTask;

    void Start()
    {
        discoBallTask = FindObjectOfType<DiscoBallTask>();


    }

    // Update is called once per frame
    void Update()
    {
        PlayerInteract();
    }

    void PlayerInteract()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Interact") && discoBallTask.inDanger)
            {
                discoBallTask.inDanger = false;
                discoBallTask.StopContinuousActionCoroutine();
                discoBallTask.ContinuousActionCoroutine('+', discoBallTask.incrementMeter,true);
                discoBallTask.EventOccurCoroutine();


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
        if (collision.gameObject.tag == "Player")
        {
            collidedPlayer = false;
        }
    }
}
