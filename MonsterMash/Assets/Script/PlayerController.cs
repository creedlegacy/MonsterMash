using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Controls horizontal and vertical movement of the player
        Vector2 axisMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        axisMovement.Normalize();
        transform.Translate(axisMovement * speed * Time.deltaTime);
    }


   
}
