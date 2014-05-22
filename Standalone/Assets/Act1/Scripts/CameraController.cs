using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Quaternion OrgRotation;
	private Vector3 OrgPosition;

	void Start() {
		OrgRotation = transform.rotation;
		OrgPosition = transform.parent.transform.position - transform.position;
	}

	void LateUpdate() {
		transform.rotation = OrgRotation;
		transform.position = transform.parent.position - OrgPosition;
	}

	void Update() {
		if (Input.GetKeyDown("escape")) {
			Application.LoadLevel ("Menu");
		}
	}
}
