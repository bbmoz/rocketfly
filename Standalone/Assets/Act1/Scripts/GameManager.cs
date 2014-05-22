using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;

	/// <summary>
	/// Initializes camera by grabbing the GameCamera script attached to Camera.
	/// Spawns the Player and sets the target of the camera to the Player to follow it.
	/// </summary>
	void Start() {
		SpawnPlayer();
	}

	/// <summary>
	/// Spawns the player.
	/// </summary>
	private void SpawnPlayer() {
		Transform t = (Instantiate(player, Vector3.zero, Quaternion.AngleAxis(90, Vector3.up)) as GameObject).transform;
	}
}
