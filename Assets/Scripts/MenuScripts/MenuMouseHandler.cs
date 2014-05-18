using UnityEngine;
using System.Collections;

public class MenuMouseHandler : MonoBehaviour {

	private Color originalColor;

	void OnMouseDown() {

		if (this.name == "PlayBT") {

			Application.LoadLevel ("Scene");
			
		} else if (this.name == "QuitBT") {
			
			Application.Quit();
			
		} else if (this.name == "OptionsBT") {
			
			// options button behavior
			
		}
	}
	
	void OnMouseEnter() {
	
		originalColor = renderer.material.color;
		renderer.material.color = new Color(0, 0, 1, 1);
	
	}
	
	void OnMouseExit() {
		
		renderer.material.color = originalColor;
	
	}

}