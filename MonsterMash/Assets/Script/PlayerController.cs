using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public bool allowMovement = true, isWalking = false, isIdle = false;
    private bool collidedPickup = false, pickupFull = false;
    private GameObject pickedUpItem, pickupableGameObject;

    private Animator anim;

    SpriteRenderer pickedUpSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
    	anim = GetComponent<Animator>();
        pickedUpItem = gameObject.transform.Find("PickedUpItem").gameObject;
        pickedUpSpriteRenderer = pickedUpItem.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPickUp();
    }

    void FixedUpdate()
    {
    	PlayerMovement();
    }
    void PlayerMovement()
    {
        if (allowMovement)
        {
            //Controls horizontal and vertical movement of the player
            Vector2 axisMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(axisMovement.x > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(axisMovement.x < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            if(axisMovement.x > 0 || axisMovement.x < 0 || axisMovement.y > 0 || axisMovement.y < 0)
            {
            	anim.SetBool("isRunning",true);
                isWalking = true;
                isIdle = false;
            }
            else
            {
            	anim.SetBool("isRunning",false);
                isWalking = false;
                isIdle = true;
            }
            axisMovement.Normalize();
            transform.Translate(axisMovement * speed * Time.deltaTime);

        }
    }

    void PlayerPickUp()
    {
        
        if (Input.GetButtonDown("Pickup"))
        {
            if (!pickupFull)
            {
                if (collidedPickup)
                {
                    if (pickupableGameObject != null)
                    {
                        pickupableGameObject.SetActive(false);
                        pickedUpItem.SetActive(true);
                        pickedUpSpriteRenderer.sprite = pickupableGameObject.GetComponent<SpriteRenderer>().sprite;
                        //if item y dimension is longer than x, flip item
                        if (pickedUpSpriteRenderer.bounds.size.x < pickedUpSpriteRenderer.bounds.size.y)
                        {
                            pickedUpItem.transform.Rotate(0, 0, 90);
                        }
                        pickupFull = true;
                    }
                }
            }
            else
            {
                pickupableGameObject.transform.position = gameObject.transform.position;
                pickupableGameObject.SetActive(true);
                pickedUpItem.SetActive(false);
                pickupableGameObject = null;
                if (pickedUpSpriteRenderer.bounds.size.x < pickedUpSpriteRenderer.bounds.size.y)
                {
                    pickedUpItem.transform.Rotate(0, 0, 0);
                }
                pickupFull = false;
            }
                  
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickupable")
        {
            if (!pickupFull)
            {
                pickupableGameObject = collision.gameObject;
            }
            
            collidedPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickupable")
        {
            collidedPickup = false;
            Debug.Log(collidedPickup);
        }
    }


}
