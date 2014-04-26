using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		other.gameObject.GetComponent<PlayerControl> ().ReSpawn ();
	}
}
