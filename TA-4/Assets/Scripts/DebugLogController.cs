using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugLogController : MonoBehaviour 
{
    public Text debugText1;
    public Text debugText2;
    public Text debugText3;
    public Text debugText4;
    public Text debugText5;

	// Use this for initialization
	void Start () 
    {
        debugText1.text = "";
        debugText2.text = "";
        debugText3.text = "";
        debugText4.text = "";
        debugText5.text = "";
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void InsertLog(string log)
    {
        if(debugText1.text == "" )
        {
            debugText1.text = log;
        }
        else if (debugText2.text == "" )
        {
            debugText2.text = log;
        }
        else if (debugText3.text == "" )
        {
            debugText3.text = log;
        }
        else if (debugText4.text == "" )
        {
            debugText4.text = log;
        }
        else if (debugText5.text == "" )
        {
            debugText5.text = log;
        }
        else if (debugText4.text != log && debugText5.text != log)
        {
            if(debugText1 != null && debugText5 != null)
            {
                debugText1.text = debugText2.text;
                debugText2.text = debugText3.text;
                debugText3.text = debugText4.text;
                debugText4.text = debugText5.text;
                debugText5.text = log;
            }
        }
    }
}
