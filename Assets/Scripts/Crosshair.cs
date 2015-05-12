using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D myCrosshair;
	private Vector2 myPosition;



	// Use this for initialization
	void Start () {
		Cursor.SetCursor (myCrosshair, Vector2.zero, CursorMode.Auto);



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
		float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (myCrosshair.width / 2);
		float yMin = (Screen.height - Input.mousePosition.y) - (myCrosshair.height / 2);
		GUI.DrawTexture(new Rect(xMin, yMin, myCrosshair.width, myCrosshair.height), myCrosshair);


	}


}
