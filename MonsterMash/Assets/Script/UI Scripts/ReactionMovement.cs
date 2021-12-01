using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionMovement : MonoBehaviour
{

	public GameObject MovingObject;
	public GameObject Destination;
	public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovingObject.transform.position = Vector3.MoveTowards(MovingObject.transform.position, Destination.transform.position, Time.deltaTime * speed);
    }
}
