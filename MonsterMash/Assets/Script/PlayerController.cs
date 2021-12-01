using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Slider sprintMeter;
    public int defaultSprintMeter = 5;
    public float normalSpeed = 1,fastSpeed = 1, slowSpeed = 1, speed = 3;
    public bool allowMovement = true, isWalking = false, isIdle = false, pickupFull = false;
    public string pickupItemName;

    private bool collidedPickup = false, slowMode = false, fastMode = false;
    private GameObject pickedUpItem, pickupableGameObject;

    public Animator anim;

    SpriteRenderer pickedUpSpriteRenderer;
    Sprite droppedFoodSprite;

    // Start is called before the first frame update
    void Start()
    {
        sprintMeter.value = defaultSprintMeter;
        anim = GetComponent<Animator>();
        pickedUpItem = gameObject.transform.Find("Body/PickedUpItem").gameObject;
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
                    anim.SetBool("isSprinting",true);
                } else {
                    anim.SetBool("isSprinting",false);
                }
                anim.SetBool("isRunning",true);
                isWalking = true;
                isIdle = false;
            }
            else
            {
                anim.SetBool("isSprinting",false);
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

                        anim.SetTrigger("isInteract");

                        //If item is a source item dont disable
                        if (!pickupableGameObject.GetComponent<PickupItemClass>().sourceItem)
                        {
                            pickupableGameObject.SetActive(false);
                        }
                            
                        pickedUpItem.SetActive(true);
                        pickedUpItem.GetComponent<AudioSource>().clip = pickupableGameObject.GetComponent<AudioSource>().clip;
                        pickedUpItem.GetComponent<AudioSource>().Play();
                        pickupItemName = pickupableGameObject.GetComponent<PickupItemClass>().uniqueName;
                        pickedUpSpriteRenderer.sprite = pickupableGameObject.GetComponent<SpriteRenderer>().sprite;

                        //If item is food change position and scale
                        if (pickupableGameObject.GetComponent<PickupItemClass>().isFood)
                        {
                            //default local position = 0,4.97,0
                            pickedUpItem.transform.localPosition = new Vector3(-2.69f, 1.02f, 0);
                            pickedUpItem.transform.localScale = new Vector3(0.41f, 0.41f, 0.41f);
                            droppedFoodSprite = pickupableGameObject.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            //old
                            //pickedUpItem.transform.localPosition = new Vector3(0, 4.97f, 0);
                            //pickedUpItem.transform.localScale = new Vector3(1, 1, 1);

                            pickedUpItem.transform.localPosition = new Vector3(-2.58f, 5.31f, 0);
                            pickedUpItem.transform.localScale = new Vector3(1, 1, 1);
                        }

                        //if item y dimension is longer than x, flip item
                        //if (pickedUpSpriteRenderer.bounds.size.x < pickedUpSpriteRenderer.bounds.size.y)
                        //{
                        //    pickedUpItem.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        //}
                        //else
                        //{
                        //    pickedUpItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        //}

                        pickupFull = true;
                    }
                }
            }
            else
            {
                if (!collidedPickup)
                {

                    if (!pickupableGameObject.GetComponent<PickupItemClass>().sourceItem)
                    {
                        pickupableGameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
                        pickupableGameObject.SetActive(true);
                        pickupableGameObject.GetComponent<AudioSource>().Play();
                    }
                    else
                    {
                        pickupableGameObject.GetComponent<AudioSource>().Play();
                        //if (GameObject.Find("DroppedFood") != null)
                        //{
                        //   Destroy(GameObject.Find("DroppedFood").gameObject);
                        //}
                        GameObject droppedFood = new GameObject();
                        droppedFood.name = "DroppedFood";
                        droppedFood.AddComponent<SpriteRenderer>();
                        droppedFood.GetComponent<SpriteRenderer>().sprite = droppedFoodSprite;
                        droppedFood.transform.position = gameObject.transform.position + new Vector3(0, -0.8f, 0);
                        droppedFood.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    }

                    EmptyHand();
                }
                    
                
            }
                  
        }
        
    }

    public void EmptyHand()
    {
        
        pickupableGameObject = null;

        pickedUpItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
        pickedUpItem.SetActive(false);
        pickupItemName = null;
        pickupFull = false;
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
