using UnityEngine;
using System.Collections;

public class MenuMouseHandler : MonoBehaviour {

	void OnMouseDown() {

		if (this.name == "PlayBT") {

			Application.LoadLevel ("Scene");
			
		} else if (this.name == "QuitBT") {
			
			Application.Quit();
			
		} else if (this.name == "OptionsBT") {
			
			// options button behavior
			
		}
	}

}