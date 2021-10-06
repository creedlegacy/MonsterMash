using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{
    public Slider partymeter;
    public int partyMeterValue = 20;
    public bool continueCoroutine = true, partyTimeStarted = false, dangerState = false;
    private GameObject partyTimer,dangerCountdown;
    public float partyTime = 121f, dangerTime = 6f;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void PartyTime()
    {
        if (partyTime >= 1)
        {
            partyTimeStarted = true;
            partyTime -= Time.deltaTime;
            Debug.Log(Mathf.FloorToInt(partyTime));
            int minutes = Mathf.FloorToInt(partyTime / 60F);
            int seconds = Mathf.FloorToInt(partyTime - minutes * 60);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            partyTimer.GetComponent<Text>().text = formattedTime;
        }
        else
        {
            partyTimeStarted = false;
            SceneManager.LoadScene("PartyEndScene");
            
            PlayerPrefs.SetInt("LastPartyScore", partyMeterValue);
            PlayerPrefs.SetString("LastPartyScene", SceneManager.GetActiveScene().name);
        }
    }

    void EmptyPartyMeterCheck()
    {
        if(partyMeterValue <= 0)
        {
            dangerTime -= Time.deltaTime;
            if (dangerTime < 1)
            {
                SceneManager.LoadScene("PartyEndScene");
                PlayerPrefs.SetInt("LastPartyScore", partyMeterValue);
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


    //public void RepeatingActionCoroutine(char type, int value)
    //{
    //    repeatingActionCoroutine = RepeatingAction(type, value);
    //    StartCoroutine(repeatingActionCoroutine);
    //}

    //public void StopRepeatingActionCoroutine()
    //{
    //    StopCoroutine(repeatingActionCoroutine);
    //}

    //IEnumerator RepeatingAction(char type,int value)
    //{
    //    while (continueCoroutine)
    //    {
    //        if (type == '+')
    //        {
    //            partyMeterValue += value;
    //        }
    //        else
    //        { 
    //            partyMeterValue -= value;
    //        }
    //        partymeter.value = partyMeterValue;
    //        Debug.Log(partymeter.value);
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    //public void RandomAction(int value)
    //{

    //    partymeter.value -= value;
    //    Debug.Log(partymeter.value);

    //}
}
