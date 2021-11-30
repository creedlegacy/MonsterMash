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

    [Header("Party Manager Variables")]
    public Slider partymeter;
    public bool continueCoroutine = true, partyTimeStarted = false, dangerState = false;
    private bool heartBeatSoundOn = false,allowPartyTimerStart = false,partyEnded = false,queenfaceRunning;
    public int defaultPartyMeter = 20;
    public float partyTime = 121f, dangerTime = 6f;
    private float partyTimeStatic, defaultDangerTime = 6f;
    private GameObject partyTimer,dangerCountdown;
    public GameObject PartyStartSplash,PartyEndSplash,QueenFace;
    [HideInInspector]
    public int discoDone, skeletonDone, foodDone, musicDone;
    [HideInInspector]
    public float highScore;



    private AudioSource audioSource;
    [Header("Sound")]
    public AudioClip heartbeatSFX;

    PlayerController pc;
    HiResScreenShots ScreenShot;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        pc = FindObjectOfType<PlayerController>();
        partyTimeStatic = partyTime;
        defaultDangerTime = dangerTime;
        partymeter.value = defaultPartyMeter;
        partyTimer = GameObject.Find("PartyTimer");
        dangerCountdown = GameObject.Find("DangerCountdown");

        //Delete previous party score data when the level loads
        PlayerPrefs.DeleteKey("LastPartyScore");
        PlayerPrefs.DeleteKey("LastPartyScene");

        
        ScreenShot = GameObject.FindGameObjectWithTag("ScreenShot").GetComponent<HiResScreenShots>();

    }

    // Update is called once per frame
    void Update()
    {
        PartyTime();
        EmptyPartyMeterCheck();
        StageChange();
        QueenFaceManager();
        HighScoreManager();

    }

    void StageChange()
    {
        if(currentStage == Stage.stage1)
        {
            foreach (GameObject stage1task in stage1Tasks)
            {
                if (stage1task != null)
                    stage1task.SetActive(true);
            }
        }
        else if (currentStage == Stage.stage2)
        {
            //In a couroutine to allow the splash image to start first then the game
            StartCoroutine(PartyStart());
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

    IEnumerator PartyStart()
    {
        //In a couroutine to allow the splash image to start first then the game
        PartyStartSplash.SetActive(true);
        yield return new WaitForSeconds(3);
        allowPartyTimerStart = true;
        foreach (GameObject stage2task in stage2Tasks)
        {
            if (stage2task != null)
                stage2task.SetActive(true);
        }

    }

    IEnumerator PartyEnd()
    {

        //In a couroutine to allow the splash image to show then end the game
        partyEnded = true;
        //ScreenShot.TakeHiResShot();
        PlayerPrefs.SetInt("LastPartyScore", (int)partymeter.value);
        PlayerPrefs.SetString("LastPartyScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("LastDiscoDone", discoDone);
        PlayerPrefs.SetInt("LastSkeletonDone", skeletonDone);
        PlayerPrefs.SetInt("LastFoodDone", foodDone);
        PlayerPrefs.SetInt("LastMusicDone", musicDone);
        PlayerPrefs.SetInt("LastHighScore", (int)highScore);

        allowPartyTimerStart = false;
        audioSource.Stop();
        heartBeatSoundOn = false;

        PartyEndSplash.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("PartyEndScene");

    }

    void QueenFaceManager()
    {
        if(currentStage != Stage.stage1)
        {
            if (!queenfaceRunning)
            {
                queenfaceRunning = true;
                Animator QueenAnimator = QueenFace.GetComponent<Animator>();
                if (partymeter.value <= 150)
                {
                    QueenAnimator.SetInteger("faceStatus", 2);
                }
                else if (partymeter.value > 150 && partymeter.value < 400)
                {
                    QueenAnimator.SetInteger("faceStatus", 0);
                }
                else
                {
                    QueenAnimator.SetInteger("faceStatus", 1);
                }


                queenfaceRunning = false;


                ///Previous Method
                //queenfaceRunning = true;
                //float prevMeterValue = partymeter.value;
                //yield return new WaitForSeconds(10);
                //float meterDifference;
                //if (prevMeterValue > partymeter.value)
                //{
                //    meterDifference = partymeter.value - prevMeterValue;
                //}
                //else
                //{
                //    meterDifference = prevMeterValue - partymeter.value;
                //}
                //Animator QueenAnimator = QueenFace.GetComponent<Animator>();
                //if (meterDifference >= -20)
                //{
                //    QueenAnimator.SetInteger("faceStatus", 1);
                //}
                //else if (meterDifference <= -80)
                //{
                //    QueenAnimator.SetInteger("faceStatus", 2);
                //}
                //else
                //{
                //    QueenAnimator.SetInteger("faceStatus", 0);
                //}
                //Debug.Log("herrrrrrr " + meterDifference);
                //queenfaceRunning = false;
            }
        }
       

    }

    void HighScoreManager()
    {
        if (currentStage != Stage.stage1 && !partyEnded)
        {
            if(partymeter.value > highScore)
            {
                highScore = partymeter.value;
            }
            
        }


    }

    void PartyTime()
    {
        if (currentStage != Stage.stage1 && allowPartyTimerStart)
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
                if (partyTime > partyTimeStatic - stage2TimeTreshold)
                {
                    currentStage = Stage.stage2;
                }
                else if (partyTime <= (partyTimeStatic - stage2TimeTreshold) && partyTime > (partyTimeStatic - stage3TimeTreshold))
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
                if (!partyEnded)
                {
                    StartCoroutine(PartyEnd());
                }
                 
            }
        }
    }

    public void EnterStage2()
    {
        currentStage = Stage.stage2;
        partymeter.value = defaultPartyMeter;
        dangerTime = defaultDangerTime;
        pc.sprintMeter.value = pc.defaultSprintMeter;
        foreach (GameObject stage1task in stage1Tasks)
        {
            if (stage1task != null)
                stage1task.SetActive(false);
        }

    }

    void EmptyPartyMeterCheck()
    {
        if (!partyEnded && currentStage != Stage.stage1)
        {
            if (partymeter.value <= 0)
            {
                if (!heartBeatSoundOn)
                {
                    heartBeatSoundOn = true;
                    audioSource.clip = heartbeatSFX;
                    audioSource.Play();
                }
                dangerTime -= Time.deltaTime;
                if (dangerTime < 1)
                {
                    if (!partyEnded)
                    {
                        StartCoroutine(PartyEnd());
                    }
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
                //reset danger time when party meter goes above 0 again
                dangerTime = defaultDangerTime;
                audioSource.Stop();
                heartBeatSoundOn = false;

                dangerState = false;
                dangerCountdown.GetComponent<Text>().enabled = false;
            }
        }
        
    }


}
