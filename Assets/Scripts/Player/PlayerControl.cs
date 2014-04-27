using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private bool facingRight = true;

	private Vector2 jumpForce;

	// Velocity related
	private float curVelocity = 0, normalVelocity = 10, targetVel;
	private Movement movement;

	// Input related variables
	private Vector3 startPos;
	private Vector3 endPos;
	private float flickVelocity;
	private float flickStartTime;
	private Vector2 spawnPoint;

	// Drag
	private float dragVelReductionFactor = 3f;
	private float flickTimeLimit = 0.2f;
	
	//ground
	private bool grounded = false;
	private Transform groundCheck;
	private Collider2D groundCheckCollider;
	private bool onPlatform = false, onPlatformStart = false;

	// Garbage stuff in update
	Vector2 difVector;
	float angle;

	enum Movement
	{
		LEFT,
		RIGHT,
		UP,
		DOWN,
		IDLE
	}

	enum InputMode
	{
		BEGIN,
		DRAG,
		FLICK,
		NONE
	}

	// Use this for initialization
	void Start ()
	{
		groundCheck = transform.FindChild ("GroundCheck");
		spawnPoint = transform.FindChild ("SpawnPoint").transform.position;
		jumpForce = new Vector2 (0, 700f);
		transform.position = spawnPoint;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			startPos = Input.mousePosition;
			flickStartTime = Time.time;
		} else if (Input.GetMouseButton (0)) {
			// Indentifies the input mode
			if (Time.time - flickStartTime > flickTimeLimit) {
				endPos = Input.mousePosition;
				// move the player
				targetVel = Mathf.Sign (targetVel) * normalVelocity / dragVelReductionFactor;
			}
		} else if (Input.GetMouseButtonUp (0)) {
				
			endPos = Input.mousePosition;
			if (Mathf.Abs (endPos.x - startPos.x) < 0.1) {
				targetVel = 0;
			} else {
				difVector = endPos - startPos;
				angle = Mathf.Rad2Deg * Mathf.Atan2 (difVector.y, difVector.x);
					
				if (angle <= 40 && angle >= -40)
					movement = Movement.RIGHT;
				else if (angle > 40 && angle < 140)
					movement = Movement.UP;
				else if ((angle >= 140 && angle < 180) || angle < -140) { 
					movement = Movement.LEFT;
				} else if (angle < -40 && angle > -140)
					movement = Movement.DOWN;
				else
					movement = Movement.IDLE;
					
				// processing input
				flickVelocity = (endPos.x - startPos.x) / (Time.deltaTime * 100);
				if (movement == Movement.UP && (grounded || onPlatform))
					rigidbody2D.AddForce (jumpForce);
				else
					targetVel = normalVelocity * (flickVelocity / Mathf.Abs (flickVelocity));
			}
		}

		if (onPlatformStart) {
			curVelocity = targetVel = 0;
		}

		if (Mathf.Abs (curVelocity - targetVel) > 0.1)
			curVelocity = Mathf.Lerp (curVelocity, targetVel, 25 * Time.deltaTime);
		else 
			curVelocity = targetVel;
	}

	void FixedUpdate ()
	{
		groundCheckCollider = Physics2D.OverlapPoint (groundCheck.transform.position);
		if (groundCheckCollider != null && groundCheckCollider.transform != null && groundCheckCollider.transform.gameObject != null) {
			if (groundCheckCollider.gameObject.layer == LayerMask.NameToLayer ("Ground"))
				grounded = true;
			else if (groundCheckCollider.gameObject.layer == LayerMask.NameToLayer ("MovingPlatform")) {
				if (!onPlatform) {
					onPlatformStart = true;
					onPlatform = true;
					transform.parent = groundCheckCollider.transform;
				} else {
					//if already on platform
					onPlatformStart = false;
				}
			} else {
				grounded = false;
				onPlatformStart = false;
				onPlatform = false;
				if (transform.parent != null)
					transform.parent = null;
			}
		} else {
			// all checks are false;
			grounded = false;
			onPlatform = false;
			onPlatformStart = false;
			if (transform.parent != null)
				transform.parent = null;
		}

		if (grounded && transform.parent != null) {
			transform.parent = null;
			onPlatform = false;
		}

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

	public void ReSpawn ()
	{
		transform.position = spawnPoint;
		targetVel = Mathf.Abs (rigidbody2D.velocity.x);
		
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Sign (transform.localScale.x) * transform.localScale.x;
		transform.localScale = scale;
		facingRight = true;
	}
}
