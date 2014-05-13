using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;
	private float trackSpeed = 10;

	/// <summary>
	/// Sets the target of the camera to follow the Player.
	/// </summary>
	/// <param name="t">Target transform</param>
	public void SetTarget(Transform t) {
		target = t;
	}

	/// <summary>
	/// Increments the x and y positions of the camera based on the Player.
	/// </summary>
	void LateUpdate() {
		if (target) {
			float x = PublicMethods.IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = PublicMethods.IncrementTowards(transform.position.y, target.position.y+3, trackSpeed);
			transform.position = new Vector3(x,y,transform.position.z);
		}
	}
}
