using UnityEngine;
using System.Collections;

public class EntityController : MonoBehaviour {

	public float health;

	public void TakeDamage(float dmg) {
		health -= dmg;
		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		Debug.Log("Enemy Died!");
	}
}
