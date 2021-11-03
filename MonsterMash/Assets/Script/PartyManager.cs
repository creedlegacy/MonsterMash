using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{
    //Stages in the level that controls the difficulty
    public enum Stage
    {
        stage1,
        stage2,
        stage3,
        stage4
    }
       
    public Stage currentStage;

    //Determines at what stage does the task appear
    [Header("Tasks stage spawn list")]
    public List<GameObject> stage1Tasks = new List<GameObject>();
    public List<GameObject> stage2Tasks = new List<GameObject>();
    public List<GameObject> stage3Tasks = new List<GameObject>();
    public List<GameObject> stage4Tasks = new List<GameObject>();
    [Header("Stage time treshold")]
    public float stage2TimeTreshold = 61f;
    public float stage3TimeTreshold = 121f;

    [Header("")]
    public Slider partymeter;
    public bool continueCoroutine = true, partyTimeStarted = false, dangerState = false;
    public float partyTime = 121f, dangerTime = 6f;
    private float partyTimeStatic;
    private GameObject partyTimer,dangerCountdown;

    // Start is called before the first frame update
    void Start()
    {
        partyTimeStatic = partyTime;
        //partybar = GetComponent<Slider>();
        partymeter.value = 20;
        partyTimer = GameObject.Find("PartyTimer");
        dangerCountdown = GameObject.Find("DangerCountdown");

        //Delete previous party score data when the level loads
        PlayerPrefs.DeleteKey("LastPartyScore");
        PlayerPrefs.DeleteKey("LastPartyScene");

        


    }

    // Update is called once per frame
    void Update()
    {
        PartyTime();
        EmptyPartyMeterCheck();
        StageChange();
    }

    void StageChange()
    {
        if (currentStage == Stage.stage2)
        {
            foreach (GameObject stage2task in stage2Tasks)
            {
                if (stage2task != null)
                    stage2task.SetActive(true);
            }

        }
        else if (currentStage == Stage.stage3)
        {
            foreach (GameObject stage3task in stage3Tasks)
            {
                if (stage3task != null)
                    stage3task.SetActive(true);
            }
        }
        else if (currentStage == Stage.stage4)
        {
            foreach (GameObject stage4task in stage4Tasks)
            {
                if (stage4task != null)
                    stage4task.SetActive(true);
            }
        }
    }

    void PartyTime()
    {
        if (partyTime >= 1)
        {
            partyTimeStarted = true;
            partyTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(partyTime / 60F);
            int seconds = Mathf.FloorToInt(partyTime - minutes * 60);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            partyTimer.GetComponent<Text>().text = formattedTime;

            //Debug.Log(currentStage);
            //This is where what stage you are in are determined
            if(partyTime > partyTimeStatic - stage2TimeTreshold)
            {
                currentStage = Stage.stage2;
            }
            else if(partyTime <= (partyTimeStatic - stage2TimeTreshold) && partyTime > (partyTimeStatic - stage3TimeTreshold))
            {
                currentStage = Stage.stage3;
            }
            else if (partyTime <= (partyTimeStatic - stage3TimeTreshold))
            {
                currentStage = Stage.stage4;
            }
        }
        else
        {
            partyTimeStarted = false;
            SceneManager.LoadScene("PartyEndScene");
            
            PlayerPrefs.SetInt("LastPartyScore", (int)partymeter.value);
            PlayerPrefs.SetString("LastPartyScene", SceneManager.GetActiveScene().name);
        }
    }

    void EmptyPartyMeterCheck()
    {
        if(partymeter.value <= 0)
        {
            dangerTime -= Time.deltaTime;
            if (dangerTime < 1)
            {
                SceneManager.LoadScene("PartyEndScene");
                PlayerPrefs.SetInt("LastPartyScore", (int)partymeter.value);
                PlayerPrefs.SetString("LastPartyScene", SceneManager.GetActiveScene().name);
            }
            dangerState = true;
            dangerCountdown.GetComponent<Text>().enabled = true;
           
            int minutes = Mathf.FloorToInt(dangerTime / 60F);
            int seconds = Mathf.FloorToInt(dangerTime - minutes * 60);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            dangerCountdown.GetComponent<Text>().text = formattedTime;
        }
        else
        {
            dangerState = false;
            dangerCountdown.GetComponent<Text>().enabled = false;
        }
    }

}
