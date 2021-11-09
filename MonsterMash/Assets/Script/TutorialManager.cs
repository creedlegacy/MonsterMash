using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("List of tutorial popups")]
    public List<GameObject> tutorialPopups = new List<GameObject>();

    public List<GameObject> skeletonPileTasks = new List<GameObject>();
    public List<GameObject> musicRequestTasks = new List<GameObject>();
    public GameObject discoBallTask;
    private int currentTutorialIndex = 0, tutorialCount = 0;
    private bool tutorialPopupActive = false, doSkeletonPileTaskCheck = false, skeletonPileTutorialShown = false,
        doMusicRequestTaskCheck = false, musicRequestTutorialShown = false, doDiscoBallTaskCheck = false, discoBallTutorialShown = false;

    PartyManager pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PartyManager>();
        //foreach (Transform child in gameObject.transform)
        //{
        //    tutorialPopups.Add(child.gameObject);
        //}
        tutorialCount = tutorialPopups.Count - 1;
        ManageTutorialSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialPopupActive)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        SkeletonPileTaskCheck();
        MusicRequestTaskCheck();
        DiscoBallTaskCheck();
    }

    public void ManageTutorialSequence()
    {

        if (currentTutorialIndex > 0)
        {
            //close previous tutorial popup
            tutorialPopups[currentTutorialIndex - 1].SetActive(false);
        }
        if (currentTutorialIndex > tutorialCount)
        {
            //if all tutorials are depleted go here
            tutorialPopupActive = false;
            pm.EnterStage2();

        }
        else
        {
            if (tutorialPopups[currentTutorialIndex].name == "SkeletonPileTutorial")
            {
                if (!skeletonPileTutorialShown)
                {
                    tutorialPopupActive = true;
                    tutorialPopups[currentTutorialIndex].SetActive(true);
                    skeletonPileTutorialShown = true;
                }
                else
                {
                    tutorialPopupActive = false;
                    tutorialPopups[currentTutorialIndex].SetActive(false);
                    foreach (GameObject skeletonPileTask in skeletonPileTasks)
                    {
                        skeletonPileTask.GetComponent<SkeletonPileTask>().EventOccurCoroutine();
                    }
                    doSkeletonPileTaskCheck = true;
                    currentTutorialIndex++;
                }

            }
            else if (tutorialPopups[currentTutorialIndex].name == "MusicRequestTutorial")
            {
                if (!musicRequestTutorialShown)
                {
                    tutorialPopupActive = true;
                    tutorialPopups[currentTutorialIndex].SetActive(true);
                    musicRequestTutorialShown = true;
                }
                else
                {
                    tutorialPopupActive = false;
                    tutorialPopups[currentTutorialIndex].SetActive(false);
                    foreach (GameObject musicRequestTask in musicRequestTasks)
                    {
                        musicRequestTask.GetComponent<MusicRequestTask>().EventOccurCoroutine();
                    }
                    doMusicRequestTaskCheck = true;
                    currentTutorialIndex++;
                }

            }
            else if (tutorialPopups[currentTutorialIndex].name == "DiscoBallTutorial")
            {
                if (!discoBallTutorialShown)
                {
                    tutorialPopupActive = true;
                    tutorialPopups[currentTutorialIndex].SetActive(true);
                    discoBallTutorialShown = true;
                }
                else
                {
                    tutorialPopupActive = false;
                    tutorialPopups[currentTutorialIndex].SetActive(false);

                    discoBallTask.GetComponent<DiscoBallTask>().isTutorial = true;
                    
                    doDiscoBallTaskCheck = true;
                    currentTutorialIndex++;
                }

            }
            else
            {


                tutorialPopupActive = true;
                tutorialPopups[currentTutorialIndex].SetActive(true);
                currentTutorialIndex++;

            }
        }


    }

    void SkeletonPileTaskCheck()
    {
        if (doSkeletonPileTaskCheck)
        {
            int count = 0;
            foreach (GameObject skeletonPileTask in skeletonPileTasks)
            {
                //count null because object is destroyed after successfully completed in this instance
                if (skeletonPileTask == null)
                {
                    count++;
                }
                else
                {
                    if (skeletonPileTask.GetComponent<SkeletonPileTask>().tutorialFinished == true)
                    {
                        count++;
                    }
                }
                    
            }

            if(count == skeletonPileTasks.Count)
            {
                doSkeletonPileTaskCheck = false;
          
                ManageTutorialSequence();
            }
        }
        
    }

    void MusicRequestTaskCheck()
    {
        if (doMusicRequestTaskCheck)
        {
            int count = 0;
            foreach (GameObject musicRequestTask in musicRequestTasks)
            {
                if (musicRequestTask != null)
                    if (musicRequestTask.GetComponent<MusicRequestTask>().tutorialFinished == true)
                    {
                        count++;
                    }
            }

            if (count == musicRequestTasks.Count)
            {
                doMusicRequestTaskCheck = false;

                ManageTutorialSequence();
            }
        }

    }

    void DiscoBallTaskCheck()
    {
        if (doDiscoBallTaskCheck)
        {
           
            if (discoBallTask.GetComponent<DiscoBallTask>().tutorialFinished == true)
            {
                doDiscoBallTaskCheck = false;

                ManageTutorialSequence();
            }

        }

    }
}
