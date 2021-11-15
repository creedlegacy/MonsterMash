using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionMovement : MonoBehaviour
{



	public GameObject Destination;
	public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination.transform.position, Time.deltaTime * speed);
    }
}
