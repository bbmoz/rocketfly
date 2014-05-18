using UnityEngine;
using System.Collections;

public class MenuGameManager : MonoBehaviour {
	
	public GameObject player;
	
	/// <summary>
	/// Spawns the player.
	/// </summary>
	private void SpawnPlayer() {
		Transform t = (Instantiate(player, Vector3.zero, Quaternion.AngleAxis(90, Vector3.up)) as GameObject).transform;
	}
}
