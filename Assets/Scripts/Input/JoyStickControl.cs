using UnityEngine;
using System.Collections;

public class JoyStickControl : MonoBehaviour
{
	Touch touchOne, touchTwo;
	string joyStickBtnName;

	public static Movement horizontalMovement = Movement.IDLE;
	public static Movement verticalMovement = Movement.IDLE;

	public enum Movement
	{
		LEFT,
		RIGHT,
		UP,
		DOWN,
		IDLE
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (Touch touch in Input.touches) {
			Collider2D col = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (touch.position), 1 << LayerMask.NameToLayer ("JoyStick"));
			if (col != null) {
				joyStickBtnName = col.gameObject.name;

				//Horizontal Movement
				if (joyStickBtnName.Equals ("Right")) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
						horizontalMovement = Movement.RIGHT;
					else
						horizontalMovement = Movement.IDLE;
				} else if (joyStickBtnName.Equals ("Left")) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
						horizontalMovement = Movement.LEFT;
					else
						horizontalMovement = Movement.IDLE;
				}

				//Vertical Movement
				if (joyStickBtnName.Equals ("Up")) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
						verticalMovement = Movement.UP;
					else
						verticalMovement = Movement.IDLE;
				} else if (joyStickBtnName.Equals ("Down")) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
						verticalMovement = Movement.DOWN;
					else
						verticalMovement = Movement.IDLE;
				}
			} else {
				horizontalMovement = verticalMovement = Movement.IDLE;
			}
		}

		if (Application.platform != RuntimePlatform.Android) {
			// Debug
			if (Input.GetKey (KeyCode.A))
				horizontalMovement = Movement.LEFT;
			else if (Input.GetKey (KeyCode.D))
				horizontalMovement = Movement.RIGHT;
			else
				horizontalMovement = Movement.IDLE;

			if (Input.GetKey (KeyCode.W))
				verticalMovement = Movement.UP;
			else if (Input.GetKey (KeyCode.S))
				verticalMovement = Movement.DOWN;
			else
				verticalMovement = Movement.IDLE;
		}
	}
}
