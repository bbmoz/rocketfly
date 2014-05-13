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
	private float moveDir; 

	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	public GameObject ragdoll; // dead body animation

	// States
	private bool jumping;
	private bool sliding;
	private bool holding;

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
			// holding
			if (holding) {
				holding = false;
				animator.SetBool("Holding", false);
			}
			// sliding
			if (sliding) {
				if (Mathf.Abs(currentSpeed) < 0.25f) {
					sliding = false;
					animator.SetBool("Sliding", false);
					playerPhysics.ResetCollider();
				}
			}

			// Slide input
			if (Input.GetButtonDown("Slide")) {
				sliding = true;
				animator.SetBool("Sliding", true);
				targetSpeed = 0.0f;

				playerPhysics.SetCollider(new Vector3(10.3f,1.5f,3), new Vector3(0.35f, 0.75f, 0));
			}
		} else { // if jumping or falling
			if (!holding && playerPhysics.canWallHold) {
				holding = true;
				animator.SetBool("Holding", true);
			}
		}

		// Jump input
		if (playerPhysics.grounded || holding) {
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping", true);
				
				if (holding) {
					holding = false;
					animator.SetBool("Holding", false);
				}
			}
		}

		animationSpeed = PublicMethods.IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed), acceleration);
		animator.SetFloat("Speed", Mathf.Abs(animationSpeed));

		// Input
		moveDir = Input.GetAxisRaw("Horizontal");
		if (!sliding) {
			float speed = (Input.GetButton("Run") ? runSpeed : walkSpeed);
			targetSpeed = Input.GetAxisRaw("Horizontal")*speed;
			currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, acceleration);

			// face direction
			if (moveDir != 0 && !holding) { 
				transform.eulerAngles = (moveDir>0 ? Vector3.up*180 : Vector3.zero);
			}
		} else {
			currentSpeed = PublicMethods.IncrementTowards(currentSpeed, targetSpeed, slideDeceleration);
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
		playerPhysics.Move(amountToMove*Time.deltaTime, moveDir);
	}

	void LateUpdate() {
		CheckIfDead();
	}

	private void CheckIfDead() {
		if (transform.position.y <= -10.0f) {
			Ragdoll r = (Instantiate(ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll>();
			r.CopyPose(this.transform);
			Destroy(this.gameObject);
		}
	}
}
