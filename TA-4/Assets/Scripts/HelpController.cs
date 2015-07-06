using UnityEngine;
using System.Collections;

public class HelpController : MonoBehaviour {

    private GUIStyle aboutStyle;
    private float textureWidth;
    private float textureHeight;
    private int page = 0;

    void Start()
    {
        //Smart Terrain is best experienced in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        aboutStyle = new GUIStyle();
        aboutStyle.normal.background = Resources.Load("GUI/help/1") as Texture2D;

        textureWidth = aboutStyle.normal.background.width;
        textureHeight = aboutStyle.normal.background.height;

    }

    void Update()
    {
        if(page == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("tap");
                aboutStyle.normal.background = Resources.Load("GUI/help/2") as Texture2D;
                page++;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("tap");
                Application.LoadLevelAsync(1);
            }
        }
    }

    void OnGUI()
    {
        float width = Screen.width;
        float height = Screen.height;
        float y = (Screen.height - height) / 2;
        GUI.Box(new Rect(0, y, width, height), "", aboutStyle);

    }
}
