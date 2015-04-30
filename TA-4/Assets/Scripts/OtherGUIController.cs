using UnityEngine;
using System.Collections;

public class OtherGUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GUI.Button(new Rect(25,25,200,50),"Back"))
        {
            Application.LoadLevel(0);
        }
    }
}
