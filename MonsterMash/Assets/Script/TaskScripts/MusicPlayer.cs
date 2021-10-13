using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public List<Sprite> ListOfShapes = new List<Sprite>();
    public List<Color> ListOfColors = new List<Color>();

    private GameObject position1, position2, position3;
    // Start is called before the first frame update
    void Start()
    {
        position1 = gameObject.transform.Find("Position1").gameObject;
        position2 = gameObject.transform.Find("Position2").gameObject;
        position3 = gameObject.transform.Find("Position3").gameObject;

        int random1 = Random.Range(0, 4);
        int random2 = Random.Range(0, 4);
        int random3 = Random.Range(0, 4);

        while(random2 == random1 || random2 == random3)
        {
            random2 = Random.Range(0, 4);
        }

        while (random3 == random1 || random3 == random2)
        {
            random3= Random.Range(0, 4);
        }




        position1.SetActive(true);
        position1.GetComponent<Image>().sprite = ListOfShapes[random1];
        position1.GetComponent<Image>().color = ListOfColors[random1];
        position2.SetActive(true);
        position2.GetComponent<Image>().sprite = ListOfShapes[random2];
        position2.GetComponent<Image>().color = ListOfColors[random2];
        position3.SetActive(true);
        position3.GetComponent<Image>().sprite = ListOfShapes[random3];
        position3.GetComponent<Image>().color = ListOfColors[random3];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
