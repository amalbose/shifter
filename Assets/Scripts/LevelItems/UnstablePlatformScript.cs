using UnityEngine;
using System.Collections;

public class UnstablePlatformScript : MonoBehaviour
{
	private Vector3 initPosition;
	private bool isDisturbed = false;
	private float disturbedDuration;
	private float reSpawnTime = 1f;
	// Use this for initialization
	void Start ()
	{
		initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isDisturbed) {
			disturbedDuration += Time.deltaTime;
			if (disturbedDuration > reSpawnTime) {
				disturbedDuration = 0;
				rigidbody2D.isKinematic = true;
				transform.position = initPosition;
				isDisturbed = false;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		rigidbody2D.isKinematic = false;
		isDisturbed = true;
	}
}
