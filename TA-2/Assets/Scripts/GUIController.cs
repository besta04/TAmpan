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
        // Make a background box
        GUI.Box(new Rect(10, 10, 150, 90), "Choose Methods");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(30, 40, 100, 20), "Image Target"))
        {
            Application.LoadLevel(1);
        }

        // Make the second button.
        if (GUI.Button(new Rect(30, 70, 100, 20), "Cylinder Target"))
        {
            Application.LoadLevel(2);
        }
    }
}
