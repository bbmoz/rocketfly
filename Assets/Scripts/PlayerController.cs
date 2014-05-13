using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	// Player handling
	public float gravity = 20.0f;
	public float walkSpeed = 8.0f;
	public float runSpeed = 12.0f;
	public float acceleration = 30.0f;
	public float jumpHeight = 12.0f;
	public float slideDeceleration = 10.0f;

	// System
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float animationSpeed;

	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;

	// States
	private bool jumping;
	private bool sliding;

	/// <summary>
	/// Initializes the playerPhysics by grabbing the PlayerPhysics script attached to Player.
	/// </summary>
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();

		animator.SetLayerWeight(1,1); // start Headband layer with base layer
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
			if (sliding) {
				if (Mathf.Abs(currentSpeed) < 0.25f) {
					sliding = false;
					animator.SetBool("Sliding", false);
					playerPhysics.ResetCollider();
				}
			}
			// Jump input
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping", true);
			}
			// Slide input
			if (Input.GetButtonDown("Slide")) {
				sliding = true;
				animator.SetBool("Sliding", true);
				targetSpeed = 0.0f;

				playerPhysics.SetCollider(new Vector3(10.3f,1.5f,3), new Vector3(0.35f, 0.75f, 0));
			}
		}

		animationSpeed = PublicMethods.IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed), acceleration);
		animator.SetFloat("Speed", Mathf.Abs(animationSpeed));

		// Input
		if (!sliding) {
			float speed = (Input.GetButton("Run") ? runSpeed : walkSpeed);
			targetSpeed = Input.GetAxisRaw("Horizontal")*speed;
			currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, acceleration);

			// face direction
			float moveDir = Input.GetAxisRaw("Horizontal");
			if (moveDir != 0) {
				transform.eulerAngles = (moveDir>0 ? Vector3.up*180 : Vector3.zero);
			}
		} else {
			currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, slideDeceleration);
		}
		// set amount to move
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity*Time.deltaTime;
		playerPhysics.Move(amountToMove*Time.deltaTime);
	}
}
