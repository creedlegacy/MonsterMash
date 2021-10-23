using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
    	targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
    	Vector3 newPosition = transform.position;

    	newPosition.x = (targetTransform.position.x - 0)*0.1f;
    	newPosition.y = (targetTransform.position.y - 0)*0.1f;

    	transform.position = newPosition;
    }
}
