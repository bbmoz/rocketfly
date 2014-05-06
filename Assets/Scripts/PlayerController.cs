using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	// Player handling
	public float gravity = 20.0f;
	public float speed = 8.0f;
	public float acceleration = 30.0f;
	public float jumpHeight = 12.0f;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	
	private PlayerPhysics playerPhysics;

	/// <summary>
	/// Initializes the playerPhysics by grabbing the PlayerPhysics script attached to Player.
	/// </summary>
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}

	/// <summary>
	/// Takes the user's horizontal input and determines movement. 
	/// Handles jumping based on whether the player is on the ground or not.
	/// </summary>
	void Update () {
		// Stops player if there is horizontal collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0.0f;
			currentSpeed = 0.0f;
		}

		targetSpeed = Input.GetAxisRaw("Horizontal")*speed;
		currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, acceleration);

		// Allow for jumping when Player is on the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0.0f;
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity*Time.deltaTime;
		playerPhysics.Move(amountToMove*Time.deltaTime);
	}
}
