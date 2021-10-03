using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    PartyManager pm;
    TaskController tc;
    public float speed = 1;
    private bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = FindObjectOfType<PartyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerTaskInteract();
    }

    void PlayerMovement()
    {
        //Controls horizontal and vertical movement of the player
        Vector2 axisMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        axisMovement.Normalize();
        transform.Translate(axisMovement * speed * Time.deltaTime);
    }

    void PlayerTaskInteract()
    {
        if (collided)
        {
            if (Input.GetButtonDown("Interact"))
            {
                tc.PlayerInteracted();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TaskPrefab")
        {
            tc = collision.gameObject.GetComponent<TaskController>();
            collided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TaskPrefab")
        {
            collided = false;
        }
    }
}
