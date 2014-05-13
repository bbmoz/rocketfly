using UnityEngine;
using System.Collections;

public class CubeController : EntityController {

	public float speed = 300;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Player") {
			gameObject.GetComponent<EntityController>().TakeDamage(2);
		}
	}
}
