using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public List<Sprite> ListOfShapes = new List<Sprite>();
    public List<Color> ListOfColors = new List<Color>();
    public List<string> ListOfNames = new List<string>();
    public bool allowInteract = false;
    //it is implied that selected shape is always the shape on position 2 order, because position 2 is the middle shape in the order
    public string selectedShape;
    private bool collidedPlayer = false, interactedState = false;
    private int position1Order = 0 , position2Order = 1, position3Order = 2;
    private GameObject position1, position2, position3, arrowLeft, arrowRight, bubbleSprite;
    PlayerController pc;
    MusicRequestTask mrt;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        mrt = FindObjectOfType<MusicRequestTask>();
        position1 = gameObject.transform.Find("Canvas/Position1").gameObject;
        position2 = gameObject.transform.Find("Canvas/Position2").gameObject;
        position3 = gameObject.transform.Find("Canvas/Position3").gameObject;
        arrowLeft = gameObject.transform.Find("Canvas/ArrowLeft").gameObject;
        arrowRight = gameObject.transform.Find("Canvas/ArrowRight").gameObject;
        bubbleSprite = gameObject.transform.Find("Canvas/BubbleSprite").gameObject;


        
        position1.GetComponent<Image>().sprite = ListOfShapes[position1Order];
        position1.GetComponent<Image>().color = ListOfColors[position1Order];

        position2.GetComponent<Image>().sprite = ListOfShapes[position2Order];
        position2.GetComponent<Image>().color = ListOfColors[position2Order];

        position3.GetComponent<Image>().sprite = ListOfShapes[position3Order];
        position3.GetComponent<Image>().color = ListOfColors[position3Order];

        //it is implied that selected shape is always the shape on position 2 order, because position 2 is the middle shape in the order
        selectedShape = ListOfNames[position2Order];


    }

    // Update is called once per frame
    void Update()
    {
        PlayerInteracted();
    }

    void PlayerInteracted()
    {
        if (collidedPlayer && allowInteract)
        {
            if (Input.GetButtonDown("Interact"))
            {
                pc.allowMovement = !pc.allowMovement;
                interactedState = !interactedState;
                position1.SetActive(interactedState);
                position2.SetActive(interactedState);
                position3.SetActive(interactedState);
                arrowLeft.SetActive(interactedState);
                arrowRight.SetActive(interactedState);
                bubbleSprite.SetActive(interactedState);
                if (!interactedState)
                {
                    if(mrt.requestedShape == selectedShape)
                    {
                        mrt.MusicRequestSuccess();
                        MusicPlayerOff();
                    }
                    else
                    {
                        mrt.MusicRequestFail();
                        MusicPlayerOff();
                    }
                }
            }
            if (interactedState)
            {
                if (Input.GetButtonDown("Left"))
                {

                    StartCoroutine(ButtonDownColorChange("left"));
                    position1Order += 1;
                    position2Order += 1;
                    position3Order += 1;
                    if (position1Order > ListOfShapes.Count - 1)
                    {
                        position1Order = 0;
                    }
                    else if (position2Order > ListOfShapes.Count - 1)
                    {
                        position2Order = 0;
                    }
                    else if (position3Order > ListOfShapes.Count - 1)
                    {
                        position3Order = 0;

                    }

                    position1.GetComponent<Image>().sprite = ListOfShapes[position1Order];
                    position1.GetComponent<Image>().color = ListOfColors[position1Order];
                    position2.GetComponent<Image>().sprite = ListOfShapes[position2Order];
                    position2.GetComponent<Image>().color = ListOfColors[position2Order];
                    position3.GetComponent<Image>().sprite = ListOfShapes[position3Order];
                    position3.GetComponent<Image>().color = ListOfColors[position3Order];

                    selectedShape = ListOfNames[position2Order];
                }
                else if (Input.GetButtonDown("Right"))
                {
                    StartCoroutine(ButtonDownColorChange("right"));
                    position1Order -= 1;
                    position2Order -= 1;
                    position3Order -= 1;
                    if (position1Order < 0)
                    {
                        position1Order = ListOfShapes.Count - 1;
                    }
                    else if (position2Order < 0)
                    {
                        position2Order = ListOfShapes.Count - 1;
                    }
                    else if (position3Order < 0)
                    {
                        position3Order = ListOfShapes.Count - 1;

                    }

                    position1.GetComponent<Image>().sprite = ListOfShapes[position1Order];
                    position1.GetComponent<Image>().color = ListOfColors[position1Order];
                    position2.GetComponent<Image>().sprite = ListOfShapes[position2Order];
                    position2.GetComponent<Image>().color = ListOfColors[position2Order];
                    position3.GetComponent<Image>().sprite = ListOfShapes[position3Order];
                    position3.GetComponent<Image>().color = ListOfColors[position3Order];

                    selectedShape = ListOfNames[position2Order];
                }
                else if (Input.GetButtonDown("close_task"))
                {
                    pc.allowMovement = true;
                    interactedState = false;
                    position1.SetActive(interactedState);
                    position2.SetActive(interactedState);
                    position3.SetActive(interactedState);
                    arrowLeft.SetActive(interactedState);
                    arrowRight.SetActive(interactedState);
                    bubbleSprite.SetActive(interactedState);
                }
            }
        }
           
    }

    public void MusicPlayerOff()
    {
        allowInteract = false;
        pc.allowMovement = true;
        interactedState = false;
        position1.SetActive(interactedState);
        position2.SetActive(interactedState);
        position3.SetActive(interactedState);
        arrowLeft.SetActive(interactedState);
        arrowRight.SetActive(interactedState);
        bubbleSprite.SetActive(interactedState);
    }

    IEnumerator ButtonDownColorChange(string direction)
    {
        if(direction == "left")
        {
            arrowLeft.GetComponent<Image>().color = new Color(1, 0.8420377f, 0);
            yield return new WaitForSeconds(0.3f);
            arrowLeft.GetComponent<Image>().color = Color.black;
        }
        else
        {
            arrowRight.GetComponent<Image>().color = new Color(1, 0.8420377f, 0);
            yield return new WaitForSeconds(0.3f);
            arrowRight.GetComponent<Image>().color = Color.black;
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
