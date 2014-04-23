using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private bool facingRight = true;

	private Vector2 jumpForce;

	// Velocity related
	private float curVelocity = 0, normalVelocity = 10, targetVel;
	private string movement;

	// Input related variables
	private Vector2 startPos;
	private Vector2 endPos;
	private float flickVelocity;
	
	//ground
	public bool grounded = false;
	private Transform groundCheck;
	private float groundRadius = 0.2f;

	// Use this for initialization
	void Start ()
	{
		groundCheck = transform.FindChild ("GroundCheck");
		jumpForce = new Vector2 (0, 700f);
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

				// processing input
				flickVelocity = (endPos.x - startPos.x) / (Time.deltaTime * 100);
				if (movement.Equals ("UP") && grounded)
					rigidbody2D.AddForce (jumpForce);
				else
					targetVel = normalVelocity * (flickVelocity / Mathf.Abs (flickVelocity));
			}
		}
		if (Mathf.Abs (curVelocity - targetVel) > 0.1)
			curVelocity = Mathf.Lerp (curVelocity, targetVel, 25 * Time.deltaTime);
		else 
			curVelocity = targetVel;
	}

	void FixedUpdate ()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, 1 << LayerMask.NameToLayer ("Ground"));

		rigidbody2D.velocity = new Vector2 (curVelocity, rigidbody2D.velocity.y);

		// Flip the character
		if (curVelocity > 0 && !facingRight) {
			Flip ();
		} else if (curVelocity < 0 && facingRight) {
			Flip ();
		}
	}

	private void Flip ()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
