using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartyEndManager : MonoBehaviour
{
    private GameObject partyScore, stars, extraInfo;
    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject star1, star2, star3;
    public float countUpSpeed = 0.050f;
    Text partyScoreText, highScore, discoTask, skeletonTask, foodTask, musicTask;
    private float partyScoreValue = 0;
    private AudioSource audioSource;
    private bool countUpCompleted = false,drumRollOn;
    public AudioClip drumRoll,winSound,failSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        partyScore = GameObject.Find("Canvas/Panel/PartyScore");
        stars = GameObject.Find("Canvas/Panel/stars").gameObject;
        extraInfo = GameObject.Find("Canvas/Panel/ExtraInfo").gameObject;
        highScore = GameObject.Find("Canvas/Panel/ExtraInfo/HighScoreResult").gameObject.GetComponent<Text>(); 
        discoTask = GameObject.Find("Canvas/Panel/ExtraInfo/DiscoResult").gameObject.GetComponent<Text>();
        skeletonTask = GameObject.Find("Canvas/Panel/ExtraInfo/SkeletonsResult").gameObject.GetComponent<Text>();
        foodTask = GameObject.Find("Canvas/Panel/ExtraInfo/FoodOrdersResult").gameObject.GetComponent<Text>(); 
        musicTask = GameObject.Find("Canvas/Panel/ExtraInfo/SongResult").gameObject.GetComponent<Text>(); 
        partyScoreText = partyScore.GetComponent<Text>();

        StartCoroutine(CountUpScore());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStars();

        if (countUpCompleted)
        {
            stars.GetComponent<Animator>().Play("StarAnimation");
        }
    }

    IEnumerator CountUpScore()
    {
        if (PlayerPrefs.HasKey("LastPartyScore"))
        {
            highScore.text = PlayerPrefs.HasKey("LastHighScore") ? PlayerPrefs.GetInt("LastHighScore").ToString() : "0";
            skeletonTask.text = PlayerPrefs.HasKey("LastSkeletonDone") ? PlayerPrefs.GetInt("LastSkeletonDone").ToString() : "0";
            discoTask.text = PlayerPrefs.HasKey("LastDiscoDone") ? PlayerPrefs.GetInt("LastDiscoDone").ToString() : "0";
            foodTask.text = PlayerPrefs.HasKey("LastFoodDone") ? PlayerPrefs.GetInt("LastFoodDone").ToString() : "0";
            musicTask.text = PlayerPrefs.HasKey("LastMusicDone") ? PlayerPrefs.GetInt("LastMusicDone").ToString() : "0";

            if (PlayerPrefs.GetInt("LastPartyScore") > 0)
            {

                while (partyScoreValue != PlayerPrefs.GetInt("LastPartyScore"))
                {
                    partyScoreValue = partyScoreValue + 1;
                    partyScoreText.text = partyScoreValue.ToString();
                    if (!drumRollOn)
                    {
                        drumRollOn = true;
                        audioSource.loop = true;
                        audioSource.clip = drumRoll;
                        audioSource.Play();

                    }

                    if (partyScoreValue >= PlayerPrefs.GetInt("LastPartyScore"))
                    {
                        drumRollOn = false;
                        audioSource.loop = false;
                        audioSource.clip = winSound;
                        audioSource.Play();
                        countUpCompleted = true;
                        
                        extraInfo.SetActive(true);
                    }

                    yield return new WaitForSeconds(countUpSpeed);
                }
            }
            else
            {
                extraInfo.SetActive(true);
            }

        }
        else
        {
            partyScoreText.text = 0.ToString();
        }

    }

        void UpdateStars()
    {
        if(partyScoreValue < 200 && partyScoreValue > 0)
        {
            star1.GetComponent<SpriteRenderer>().sprite = filledStar;
        }
        else if (partyScoreValue >= 200 && partyScoreValue < 400)
        {
            star1.GetComponent<SpriteRenderer>().sprite = filledStar;
            star2.GetComponent<SpriteRenderer>().sprite = filledStar;
        }
        else if (partyScoreValue >= 400)
        {
            star1.GetComponent<SpriteRenderer>().sprite = filledStar;
            star2.GetComponent<SpriteRenderer>().sprite = filledStar;
            star3.GetComponent<SpriteRenderer>().sprite = filledStar;
        }
  
    }

   

    public void Restart()
    {
        if (PlayerPrefs.HasKey("LastPartyScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("LastPartyScene"));
        }
        else
        {
            SceneManager.LoadScene("PartyLevelScene");
        }
            
    }

    public void MainMenu()
    {

        SceneManager.LoadScene("MainMenuScene");


    }
}
