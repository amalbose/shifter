using UnityEngine;
using System.Collections;

public class SlowDownPlayer : MonoBehaviour
{
	bool used = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (!used) {
			used = true;
			col.gameObject.GetComponent<PlayerControl> ().SlowDown ();
		}
	}
}
