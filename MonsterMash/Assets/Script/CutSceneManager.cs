using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    public GameObject mycamera;
    private GameObject flag;
    private int presses;
    // Start is called before the first frame update
    void Start()
    {
        flag = GameObject.Find("Flag");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if(flag.active == false)
            {
                SceneManager.LoadScene("MainMenuScene");
            }
            else
            {
                if (presses == 0)
                {
                    mycamera.GetComponent<Animator>().SetBool("finish", true);
                    presses++;
                }
                else
                {
                    SceneManager.LoadScene("MainMenuScene");
                }
            }
            


        }
    }
}
