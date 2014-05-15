using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask; // represents the Ground's layer to detect collisions for

	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;

	private Vector3 originalSize;
	private Vector3 originalCenter;
	private float colliderScale;

	private int collisionDivisionX = 10;
	private int collisionDivisionY = 10; // TODO: when falling, rays don't spread over space!!

	private float skin = .01f; // small space between player and ground to avoid buggy collision rays

	[HideInInspector]
	public bool grounded; // to keep track whether we are on the ground to be able to jump
	[HideInInspector]
	public bool movementStopped; // to avoid "sticking" for horizontal collision
	[HideInInspector]
	public bool canWallHold;

	Ray ray;
	RaycastHit hit;

	void Start() {
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;
		originalSize = collider.size;
		originalCenter = collider.center;
		SetCollider(originalSize, originalCenter);
	}

	public void Move(Vector2 moveAmount, float moveDir) {
		float deltaX = moveAmount.x;
		float deltaY = moveAmount.y;
		Vector2 position = transform.position;

		// vertical collision
		for(int i=0; i<collisionDivisionY; i++) {
			float dir = Mathf.Sign(deltaY);
			float x = position.x + center.x - size.x/(collisionDivisionY-1) * i; // left, center, and right of collider
			float y = position.y + center.y + size.y/2 * dir; // top or bottom of collider

			ray = new Ray(new Vector2(x,y), new Vector2(0, dir));
			Debug.DrawRay(ray.origin, ray.direction);
			if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY)+skin, collisionMask)) {
				float dist = Vector3.Distance(ray.origin, hit.point); // distance between player and ground

				// stop player's downward movement after coming within skin width of collider
				if (dist > skin) {
					deltaY = (dist-skin)*dir;
				} else {
					deltaY = 0;
				}
				grounded = true;
				break;
			} else {
				grounded = false;
			}
		}

		// horizontal collision
		movementStopped = false;
		canWallHold = false;

		// if there's horizontal movement, check for horizontal collision
		if (deltaX != 0) {
			for(int i=0; i<collisionDivisionX; i++) {
				float dir = Mathf.Sign(deltaX);
				float x = position.x + center.x + size.x/2 * dir; // left or right of collider
				float y = position.y + center.y - size.y/2 + size.y/(collisionDivisionX-1) * i; // bottom, center, and top of collider
				
				ray = new Ray(new Vector2(x,y), new Vector2(dir, 0));
				Debug.DrawRay(ray.origin, ray.direction);
				if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX)+skin, collisionMask)) {

					if (hit.collider.tag == "Wall") {
						if (moveDir!=0 && Mathf.Sign(deltaX) == Mathf.Sign(moveDir)) {
							canWallHold = true;
						}
					}

					float dist = Vector3.Distance(ray.origin, hit.point); // distance between player and ground
					
					// stop player's downward movement after coming within skin width of collider
					if (dist > skin) {
						deltaX = (dist-skin)*dir;
					} else {
						deltaX = 0;
					}
					movementStopped = true;
					break;
				} else {
					movementStopped = false;
				} 
			}
		}

		// diagonal collision
		if (!grounded && !movementStopped) {
			Vector3 playerDir = new Vector3(deltaX, deltaY);
			float xDiag = position.x + center.x + size.x/2 * Mathf.Sign(deltaX); 
			float yDiag = position.y + center.y + size.y/2 * Mathf.Sign(deltaY);
			Vector2 origin = new Vector2(xDiag, yDiag);
			ray = new Ray(origin, playerDir.normalized);
			Debug.DrawRay(origin, playerDir.normalized);
			if (Physics.Raycast(ray, Mathf.Sqrt(deltaX*deltaX + deltaY*deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX, deltaY);

		// movement
		transform.Translate(finalTransform, Space.World); 
	}

	public void SetCollider(Vector3 s, Vector3 c) {
		collider.size = s;
		collider.center = c;
		size = s*colliderScale;
		center = c*colliderScale;
	}

	public void ResetCollider() {
		SetCollider(originalSize, originalCenter);
	}
}
