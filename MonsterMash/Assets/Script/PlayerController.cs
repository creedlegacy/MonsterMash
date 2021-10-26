using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Slider sprintMeter;
    public float normalSpeed = 1,fastSpeed = 1, slowSpeed = 1, speed = 3;
    public bool allowMovement = true, isWalking = false, isIdle = false, pickupFull = false;
    public string pickupItemName;

    private bool collidedPickup = false, slowMode = false, fastMode = false;
    private GameObject pickedUpItem, pickupableGameObject;

    public Animator anim;

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
        AlterMovementSpeed();

    }

    void FixedUpdate()
    {
       
        PlayerMovement();
    }

    void AlterMovementSpeed()
    {
        if (Input.GetButton("Sprint"))
        {
            if(sprintMeter.value > 0)
            {

                fastMode = true;
            }
            else
            {
                fastMode = false;
            }
            
        }
        else
        {
            fastMode = false;
        }
        if (slowMode || fastMode)
        {
            if (slowMode && !fastMode)
            {
                speed = slowSpeed;
            }
            else if (!slowMode && fastMode)
            {
                speed = fastSpeed;
            }
            else
            {
                speed = fastSpeed * slowSpeed / normalSpeed;
            }
        }
        else
        {
            speed = normalSpeed;
        }
  
    }

    

    void PlayerMovement()
    {
        if (allowMovement)
        {

            //Controls horizontal and vertical movement of the player
            Vector2 axisMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            if(axisMovement.x > 0)
            {
                float minusScale;
                if (gameObject.transform.localScale.x > 0)
                {
                    minusScale = -gameObject.transform.localScale.x;
                    gameObject.transform.localScale = new Vector3(minusScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
                  
            }
            else if(axisMovement.x < 0)
            {
                float plusScale;
                if (gameObject.transform.localScale.x < 0)
                {
                    plusScale = Mathf.Abs(gameObject.transform.localScale.x);
                    gameObject.transform.localScale = new Vector3(plusScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
            }

            if(axisMovement.x > 0 || axisMovement.x < 0 || axisMovement.y > 0 || axisMovement.y < 0)
            {
                //if sprinting reduce sprint meter
                if (fastMode)
                {
                    sprintMeter.value -= Time.deltaTime;
                }
                
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
                        pickupItemName = pickupableGameObject.GetComponent<PickupItemClass>().uniqueName;
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
                pickupItemName = null;
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

        if (collision.gameObject.tag == "SlowMonsters")
        {   
            slowMode = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickupable")
        {
            collidedPickup = false;
        }

        if (collision.gameObject.tag == "SlowMonsters")
        {   
            slowMode = false;
        }
    }


}
