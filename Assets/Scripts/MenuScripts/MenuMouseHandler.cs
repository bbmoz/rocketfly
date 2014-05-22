using UnityEngine;
using System.Collections;

public class MenuMouseHandler : MonoBehaviour {

	private Color originalColor;
	private RaycastHit hit;

	void Update () {
		if (Input.GetMouseButton(0))
		{
			Vector3 pos = Input.mousePosition;
			Debug.Log("Mouse pressed " + pos);
			
			Ray ray = Camera.mainCamera.ScreenPointToRay(pos);
			if(Physics.Raycast(ray))
			{
				Debug.Log("Something hit");
			}
		}
	}

	void OnMouseUp() {
		Vector3 pos = Input.mousePosition;
		Ray ray = Camera.mainCamera.ScreenPointToRay(pos);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if (hit.collider.gameObject.name == "PlayBT") {
				
				Application.LoadLevel("Scene");
				
			} else if (hit.collider.gameObject.name == "QuitBT") {
				
				Application.Quit();
				
			} else if (hit.collider.gameObject.name == "OptionsBT") {
				
				// options button behavior
				
			}
		}
	}
	
	void OnMouseEnter() {
		originalColor = renderer.material.color;
		if (gameObject.name == "QuitBT") {
			renderer.material.color = new Color(1, 0, 0, 1);
		} else if (gameObject.name == "OptionsBT") {
			renderer.material.color = new Color(0, 1, 0, 1);
		} else if (gameObject.name == "PlayBT") {
			renderer.material.color = new Color(0, 0, 1, 1);
		}
	}
	
	void OnMouseExit() {
		renderer.material.color = originalColor;
	}

}