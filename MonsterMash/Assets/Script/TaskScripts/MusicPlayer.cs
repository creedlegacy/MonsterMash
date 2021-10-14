using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public List<Sprite> ListOfShapes = new List<Sprite>();
    public List<Color> ListOfColors = new List<Color>();
    public List<string> ListOfNames = new List<string>();
    private bool collidedPlayer = false;
    private int random1, random2, random3;
    private GameObject position1, position2, position3;
    // Start is called before the first frame update
    void Start()
    {
        position1 = gameObject.transform.Find("Canvas/Position1").gameObject;
        position2 = gameObject.transform.Find("Canvas/Position2").gameObject;
        position3 = gameObject.transform.Find("Canvas/Position3").gameObject;

        //random1 = Random.Range(0, 4);
        //random2 = Random.Range(0, 4);
        //random3 = Random.Range(0, 4);

        //while(random2 == random1 || random2 == random3)
        //{
        //    random2 = Random.Range(0, 4);
        //}

        //while (random3 == random1 || random3 == random2)
        //{
        //    random3= Random.Range(0, 4);
        //}


        position1.SetActive(true);
        position1.GetComponent<Image>().sprite = ListOfShapes[1];
        position1.GetComponent<Image>().color = ListOfColors[1];
        position2.SetActive(true);
        position2.GetComponent<Image>().sprite = ListOfShapes[2];
        position2.GetComponent<Image>().color = ListOfColors[2];
        position3.SetActive(true);
        position3.GetComponent<Image>().sprite = ListOfShapes[3];
        position3.GetComponent<Image>().color = ListOfColors[3];

    }

    // Update is called once per frame
    void Update()
    {
        PlayerInteracted();
    }

    void PlayerInteracted()
    {
        if (collidedPlayer)
        {
            if (Input.GetButtonDown("Left"))
            {
                //int tempRandom1 = random1;
                //int tempRandom2 = random2;
                //int tempRandom3 = random3;
                //random1 -= 1;
                //random2 -= 1;
                //random3 -= 1;
                //if (random1 < 0)
                //{
                //    random1 = ListOfShapes.Count -1;
                //}
                //else if (random2 < 0)
                //{
                //    random2 = ListOfShapes.Count - 1;
                //}
                //else if (random3 < 0)
                //{
                //    random3 = ListOfShapes.Count - 1;

                //}


                ListOfShapes[1] = ListOfShapes[2];
                ListOfShapes[2] = ListOfShapes[3];
                ListOfShapes[3] = ListOfShapes[0];

                ListOfColors[1] = ListOfColors[2];
                ListOfColors[2] = ListOfColors[3];
                ListOfColors[3] = ListOfColors[0];

                position1.GetComponent<Image>().sprite = ListOfShapes[1];
                position1.GetComponent<Image>().color = ListOfColors[1];
                position2.GetComponent<Image>().sprite = ListOfShapes[2];
                position2.GetComponent<Image>().color = ListOfColors[2];
                position3.GetComponent<Image>().sprite = ListOfShapes[3];
                position3.GetComponent<Image>().color = ListOfColors[3];

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
