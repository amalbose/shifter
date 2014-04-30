using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour
{
	public bool rotateAboutCenter = true;
	public Transform rotatePoint;
	public float rotateSpeed = 100;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rotateAboutCenter)
			transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);
		else
			transform.RotateAround (rotatePoint.position, new Vector3 (0, 0, 1), rotateSpeed * Time.deltaTime);
	}
}
