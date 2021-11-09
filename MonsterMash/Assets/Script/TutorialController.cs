using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
 
    public List<GameObject> pages = new List<GameObject>();
    private GameObject prevButton, nextButton;
    private int numberOfPages,currentPageIndex = 0;
    TutorialManager tm;
    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TutorialManager>();
        prevButton = gameObject.transform.Find("PrevButton").gameObject;
        nextButton = gameObject.transform.Find("NextButton").gameObject;
        GameObject parentPage = gameObject.transform.Find("Pages").gameObject;
        foreach(Transform child in parentPage.transform)
        {
            pages.Add(child.gameObject);
        }

        numberOfPages = pages.Count - 1;
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        pages[currentPageIndex].SetActive(true);

        

    }

    // Update is called once per frame
    void Update()
    {
        if(currentPageIndex == 0)
        {
            prevButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
        }

        //if (currentPageIndex == numberOfPages)
        //{
        //    nextButton.SetActive(false);
        //}
        //else
        //{
        //    nextButton.SetActive(true);
        //}
    }

    public void NextPage()
    {
        if(currentPageIndex == numberOfPages)
        {
            tm.ManageTutorialSequence();
        }
        else
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex++;
            pages[currentPageIndex].SetActive(true);
        }
        
    }

    public void PrevPage()
    {
        pages[currentPageIndex].SetActive(false);
        currentPageIndex--;
        pages[currentPageIndex].SetActive(true);
    }
}
