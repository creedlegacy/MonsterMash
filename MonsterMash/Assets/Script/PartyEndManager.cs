using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartyEndManager : MonoBehaviour
{
    private GameObject partyScore,stars;
    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject star1, star2, star3;
    public float countUpSpeed = 0.050f;
    Text partyScoreText;
    private float partyScoreValue = 0;
    private AudioSource audioSource;
    private bool countUpCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        partyScore = GameObject.Find("Canvas/Panel/PartyScore");
        stars = GameObject.Find("Canvas/Panel/stars").gameObject;
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
            while (partyScoreValue != PlayerPrefs.GetInt("LastPartyScore"))
            {
                partyScoreValue = partyScoreValue + 1;
                partyScoreText.text = partyScoreValue.ToString();

                if(partyScoreValue >= PlayerPrefs.GetInt("LastPartyScore"))
                {
                    countUpCompleted = true;
                }

                yield return new WaitForSeconds(0.010f);
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
