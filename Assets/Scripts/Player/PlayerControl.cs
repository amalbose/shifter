using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private bool facingRight = true;

	private Vector2 jumpForce = new Vector2 (0, 700f);
	private float horizontalVel;

	private JoyStickControl.Movement horizontalMovement;
	private JoyStickControl.Movement verticalMovement;

	// Velocity related
	private float norSpeed = 7;

	private Vector2 spawnPoint;

	//ground
	private bool grounded = false;
	private Transform groundCheck;
	private Collider2D groundCheckCollider;
	private bool onPlatform = false;

	// Shift
	private bool shifted = false;

	private float maxTimer = 0.8f;
	private float jumpTimer, flipTimer;

	// Use this for initialization
	void Start ()
	{
		groundCheck = transform.FindChild ("GroundCheck");
		spawnPoint = transform.FindChild ("SpawnPoint").transform.position;
		transform.position = spawnPoint;
	}
	
	// Update is called once per frame
	void Update ()
	{
		groundCheckCollider = Physics2D.OverlapPoint (groundCheck.transform.position);
		if (groundCheckCollider != null && groundCheckCollider.transform != null && groundCheckCollider.transform.gameObject != null) {
			if (groundCheckCollider.gameObject.layer == LayerMask.NameToLayer ("Ground"))
				grounded = true;
			else if (groundCheckCollider.gameObject.layer == LayerMask.NameToLayer ("MovingPlatform")) {
				if (!onPlatform) {
					onPlatform = true;
					transform.parent = groundCheckCollider.transform;
				} else {
					//if already on platform
				}
			} else {
				grounded = false;
				onPlatform = false;
				if (transform.parent != null)
					transform.parent = null;
			}
		} else {
			// all checks are false;
			grounded = false;
			onPlatform = false;
			if (transform.parent != null)
				transform.parent = null;
		}
	}

	void FixedUpdate ()
	{
		// Move player
		horizontalMovement = JoyStickControl.horizontalMovement;
		if (horizontalMovement != JoyStickControl.Movement.IDLE) {
			if (horizontalMovement == JoyStickControl.Movement.RIGHT) {
				horizontalVel = norSpeed;
				if (!facingRight)
					Flip ();
			} else {
				horizontalVel = -norSpeed;
				if (facingRight)
					Flip ();
			}
		} else {
			horizontalVel = 0;
		}
		rigidbody2D.velocity = new Vector2 (horizontalVel, rigidbody2D.velocity.y);
		
		verticalMovement = JoyStickControl.verticalMovement;
		
		// processing input
		if (verticalMovement == JoyStickControl.Movement.UP && (grounded || onPlatform) && jumpTimer < 0f) {
			if (shifted)
				rigidbody2D.AddForce (jumpForce * -1);
			else
				rigidbody2D.AddForce (jumpForce);
			jumpTimer = maxTimer;
		} else if (verticalMovement == JoyStickControl.Movement.DOWN) {
			// Shift movement
			if (groundCheckCollider != null && groundCheckCollider.transform.gameObject.tag == "ShiftPlatform" && flipTimer < 0f) {
				ShiftPlayer ();
				flipTimer = maxTimer;
			}
		}

		// Decrease timers;
		jumpTimer -= Time.deltaTime;
		flipTimer -= Time.deltaTime;
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
		rigidbody2D.velocity = Vector2.zero;
		Vector3 scale = transform.localScale;
		scale.x = Mathf.Sign (transform.localScale.x) * transform.localScale.x;
		facingRight = true;
		if (shifted) {
			shifted = false;
			rigidbody2D.gravityScale *= -1;
			scale.y = Mathf.Sign (transform.localScale.y) * transform.localScale.y;
		}
		transform.localScale = scale;
	}

	private void ShiftPlayer ()
	{
		rigidbody2D.gravityScale *= -1;

		Vector2 newPos = transform.position;
		if (shifted)
			newPos.y += 2;
		else
			newPos.y -= 2;
		transform.position = newPos;

		Vector3 scale = transform.localScale;
		scale.y *= -1;
		transform.localScale = scale;

		// Setting shifted variable
		shifted = !shifted;
	}

	public void SavePointReached ()
	{
		spawnPoint = transform.position;
	}

	public void HitObstacle (Obstacle obstacleType)
	{
		switch (obstacleType) {
		case Obstacle.FIRE:
		case Obstacle.SPIKE:
		case Obstacle.CIRCULAR_SAW:
		default:
			Die ();
			break;
		}
	}

	private void Die ()
	{
		ReSpawn ();
	}
}
