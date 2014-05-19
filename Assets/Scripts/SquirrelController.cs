using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class SquirrelController : MonoBehaviour {
	
	// Player handling
	public float gravity = 20.0f;
	public float runSpeed = 6.0f;
	public float acceleration = 30.0f;
	public float jumpHeight = 12.0f;
	
	// System
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float moveDir; 
	private float animationSpeed;
	
	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	
	// States
	private bool jumping;
	private bool falling;
	private bool holding;
	
	/// <summary>
	/// Initializes the playerPhysics by grabbing the PlayerPhysics script attached to Player.
	/// </summary>
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
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
		
		// Allow for jumping when Player is on the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0.0f;
			
			// landing
			if (jumping) {
				jumping = false;
				animator.SetBool("Jumping", false);
			}
			// holding
			/*
			if (holding) {
				holding = false;
				//animator.SetBool("Holding", false);
			}*/
			// falling
			if (falling) {
				falling = false;
				animator.SetBool("Falling", false);
				playerPhysics.ResetCollider();
			}

		} else { // if jumping or falling
			/*if (!holding && playerPhysics.canWallHold) {
				holding = true;
			}*/
		}
		
		// Jump input
		if (playerPhysics.grounded || holding) {
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping", true);
				if (holding) {
					holding = false;
					//animator.SetBool("Holding", false);
				}
			}
		}

		animationSpeed = PublicMethods.IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed), acceleration);
		animator.SetFloat("Speed", Mathf.Abs(animationSpeed));
		
		// Input
		moveDir = Input.GetAxisRaw("Horizontal");
		float speed = runSpeed; //(Input.GetButton("Run") ? runSpeed : walkSpeed);
		targetSpeed = Input.GetAxisRaw("Horizontal")*speed;
		currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, acceleration);
			
		// face direction
		if (moveDir != 0 && !holding) { 
			transform.eulerAngles = (moveDir>0 ? Vector3.up*90 : Vector3.up*270);
		}
		
		// Input wall holding
		if (holding) {
			amountToMove.x = 0;
			if (Input.GetAxisRaw("Vertical") != -1) {
				amountToMove.y = 0;
			}
		} else {
			amountToMove.x = currentSpeed; // default horizontal moving
		}
		
		// set amount to move
		amountToMove.y -= gravity*Time.deltaTime;
		if (jumping && amountToMove.y <= 0.0f) {
			falling = true;
			animator.SetBool("Falling", true);
			playerPhysics.SetCollider(new Vector3(3.82f,17.2f, 19.2f), new Vector3(0.11f, -6.32f, -1.12f));
		}

	}
	
	void LateUpdate() {
		CheckIfDead();
		playerPhysics.Move(amountToMove*Time.deltaTime, moveDir);
	}
	
	private void CheckIfDead() {
		if (transform.position.y <= -10.0f) {
			transform.position = new Vector3(0.15f, 6.0f, 0.0f);
		}
	}
}
