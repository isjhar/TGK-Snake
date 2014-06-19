using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {

	void OnGUI () {
		// Make a background box
		float midX = (Screen.width / 2) - 50;
		float midY = (Screen.height / 2) - 50;
		GUI.Box(new Rect(midX,midY,100,120), "Main Menu");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(midX+10,midY+30,80,20), "Easy")) {
			Application.LoadLevel("Easy");
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(midX+10,midY+60,80,20), "Medium")) {
			Application.LoadLevel("Medium");
		}

		// Make the second button.
		if(GUI.Button(new Rect(midX+10,midY+90,80,20), "Hard")) {
			Application.LoadLevel("Hard");
		}
	}
}
