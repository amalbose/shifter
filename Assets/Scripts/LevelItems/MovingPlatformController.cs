using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatformController : MonoBehaviour
{

	public List<Transform> locations;
	public float proximity = 0.5f;
	public float speed = 1.0f;
	
	private Transform currentTransform;
	
	void Start ()
	{
		currentTransform = locations [0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 distance = currentTransform.position - transform.position;
		if (distance.magnitude < proximity) {
			currentTransform = getNextPosition ();
		}
		transform.position = Vector3.Slerp (transform.position, currentTransform.position, Time.deltaTime * speed);
	}
	
	private Transform getNextPosition ()
	{
		int index = locations.IndexOf (currentTransform);
		Transform nextPos = locations [0];
		if (index != locations.Count - 1)
			nextPos = locations [index + 1];
		return nextPos;
	}
}
