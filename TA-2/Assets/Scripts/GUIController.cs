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

        GUIStyle style = new GUIStyle();
        style.fontSize = 72;
        
        style.alignment = TextAnchor.MiddleCenter;
        RectOffset margin = new RectOffset();
        margin.bottom = 10;
        margin.top = 10;
        style.margin = margin;
        style.normal.background = new Texture2D(1, 1);

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Image Target", style))
        {
            Application.LoadLevel(1);
        }
        topPosition = topPosition + buttonHeight;
        topPosition += 10;
        // Make the second button.
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Cylinder Target", style))
        {
            Application.LoadLevel(2);
        }
        topPosition = topPosition + buttonHeight;
        topPosition += 10;
        if (GUI.Button(new Rect(width / 4, topPosition, buttonWidth, buttonHeight), "Multi Target", style))
        {
            //Application.LoadLevel(2);
        }
    }
}
