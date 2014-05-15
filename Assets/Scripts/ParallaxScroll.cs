using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {
	
	public static Transform target;

	private float yOffset;

	void Start() {
		yOffset = this.gameObject.transform.position.y;
	}
	
	/// <summary>
	/// Increments the x and y positions of the background based on the Player.
	/// </summary>
	void LateUpdate() {
		if (target) {
			renderer.material.mainTextureOffset = new Vector2(target.position.x, target.position.y+yOffset);
		}
	}
}
