using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	private Vector2 myPosition;
	private Sprite myCrosshair;

	// Use this for initialization
	void Start () {


		myCrosshair = GameObject.Find ("SettingsContainer").GetComponent<SettingsBehaviour> ().crossHair;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.SetCursor (myCrosshair.texture , Vector2.zero, CursorMode.Auto);

	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKey (KeyCode.Escape)) {
//			Screen.lockCursor = false;
//		} else {
//			Screen.lockCursor = true;
//		}
//
//		if (Screen.lockCursor) {
//			//float myX = Screen.width / 2;
//			//float myY = Screen.height / 2;
//			//Input.mousePosition.x = myX;
//			//Input.mousePosition.y = myY;
//		}
//	
	}
	void OnGUI() {
		float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (myCrosshair.rect.width / 4);
		float yMin = (Screen.height - Input.mousePosition.y) - (myCrosshair.rect.height / 4);
		GUI.DrawTexture(new Rect(xMin, yMin, myCrosshair.rect.width/2, myCrosshair.rect.height/2), myCrosshair.texture );


	}


}
