using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartyEndManager : MonoBehaviour
{
    private GameObject partyScore;

    // Start is called before the first frame update
    void Start()
    {
        partyScore = GameObject.Find("PartyScore");
        partyScore.GetComponent<Text>().text = PlayerPrefs.GetInt("LastPartyScore").ToString() + " Pts.";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        Debug.Log("test");
        SceneManager.LoadScene(PlayerPrefs.GetString("LastPartyScene"));
    }
}
