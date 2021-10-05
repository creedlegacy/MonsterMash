using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    public Slider partymeter;
    public int partyMeterValue = 0;
    public bool continueCoroutine = true;
    private IEnumerator repeatingActionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //partybar = GetComponent<Slider>();
        partymeter.value = 0;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RepeatingActionCoroutine(char type, int value)
    {
        repeatingActionCoroutine = RepeatingAction(type, value);
        StartCoroutine(repeatingActionCoroutine);
    }

    public void StopRepeatingActionCoroutine()
    {
        StopCoroutine(repeatingActionCoroutine);
    }

    IEnumerator RepeatingAction(char type,int value)
    {
        while (continueCoroutine)
        {
            if (type == '+')
            {
                partyMeterValue += value;
            }
            else
            { 
                partyMeterValue -= value;
            }
            partymeter.value = partyMeterValue;
            Debug.Log(partymeter.value);
            yield return new WaitForSeconds(1f);
        }
    }

    public void RandomAction(int value)
    {
 
        partymeter.value -= value;
        Debug.Log(partymeter.value);

    }
}
