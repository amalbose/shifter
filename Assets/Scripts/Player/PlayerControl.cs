using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

	private Vector2 startPos;
	private Vector2 endPos;

	private float curVelocity = 0, targetVel;
	private float normalVelocity = 10;
	public float flickVelocity;
	public string movement;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			startPos = Input.mousePosition;
		} else if (Input.GetMouseButtonUp (0)) {
			endPos = Input.mousePosition;
			if (Mathf.Abs (endPos.x - startPos.x) < 0.1) {
				targetVel = 0;
			} else {
				Vector2 difVector = endPos - startPos;
				float angle = Mathf.Rad2Deg * Mathf.Atan2 (difVector.y, difVector.x);

				if (angle <= 40 && angle >= -40)
					movement = "RIGHT";
				else if (angle > 40 && angle < 140)
					movement = "UP";
				else if ((angle >= 140 && angle < 180) || angle < -140)
					movement = "LEFT";
				else if (angle < -40 && angle > -140)
					movement = "DOWN";
				else
					movement = "IDLE";
				Debug.Log (movement);
				//
				flickVelocity = (endPos.x - startPos.x) / (Time.deltaTime * 100);
				targetVel = normalVelocity * (flickVelocity / Mathf.Abs (flickVelocity));
			}
		}
		if (Mathf.Abs (curVelocity - targetVel) > 0.1)
			curVelocity = Mathf.Lerp (curVelocity, targetVel, 25 * Time.deltaTime);
		else 
			curVelocity = targetVel;
		rigidbody2D.velocity = new Vector2 (curVelocity, Physics2D.gravity.y);
	}



}
