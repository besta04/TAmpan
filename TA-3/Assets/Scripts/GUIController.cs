﻿using UnityEngine;
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
        float buttonWidth = width - 150;
        float buttonHeight = height / 4;
        float topPosition = 100;

        // Make a background box
        GUI.Box(new Rect(50, 50, width - 100, height - 100), "Select Methods");

        GUIStyle style = new GUIStyle();
        style.fontSize = 68;
        
        style.alignment = TextAnchor.MiddleCenter;
        RectOffset margin = new RectOffset();
        margin.bottom = 10;
        margin.top = 10;
        style.margin = margin;
        int textureWidth = (int)buttonWidth;
        int textureHeight = (int)buttonHeight;
        Texture2D texture = new Texture2D(1, 1);
        //texture.SetPixel(128, 128, Color.blue);
        //Color[] colors = new Color[1];
        //colors[0] = Color.white;
        //texture.SetPixels(colors);
        //texture.alphaIsTransparency = true;
        
        //Debug.Log("tess");
        texture.Apply();
        style.normal.background = texture;
        
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(75, topPosition, buttonWidth, buttonHeight), "Image Target", style))
        {
            Application.LoadLevel(1);
        }
        topPosition = topPosition + buttonHeight;
        topPosition += 10;
        // Make the second button.
        if (GUI.Button(new Rect(75, topPosition, buttonWidth, buttonHeight), "Cylinder Target", style))
        {
            Application.LoadLevel(2);
        }
    }
}
