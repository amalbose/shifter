using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour
{

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
		if (col.gameObject.name == "Player")
			col.gameObject.GetComponent<PlayerControl> ().SavePointReached ();
	}
}
