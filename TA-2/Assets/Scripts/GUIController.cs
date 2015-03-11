using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    void OnGUI()
    {
        float width = Screen.width;
        float height = Screen.height;
        float buttonWidth = width / 2;
        float buttonHeight = height / 4;
        float topPosition = 100;

        // Make a background box
        GUI.Box(new Rect(50, 50, width - 100, height - 100), "Choose Methods");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Image Target"))
        {
            Application.LoadLevel(1);
        }
        topPosition = topPosition + buttonHeight;
        // Make the second button.
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Cylinder Target"))
        {
            Application.LoadLevel(2);
        }
        topPosition = topPosition + buttonHeight;
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Multi Target"))
        {
            //Application.LoadLevel(2);
        }
    }
}
