using UnityEngine;
using System.Collections;

public class SpringScript : MonoBehaviour
{
	public float force = 30f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name == "Player") {
			col.gameObject.rigidbody2D.velocity = Vector2.zero;
			col.gameObject.rigidbody2D.AddForce (new Vector2 (0, force) / Time.fixedDeltaTime);
		}
	}
}
