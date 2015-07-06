using UnityEngine;
using System.Collections;

public class AboutController : MonoBehaviour {

	private GUIStyle aboutStyle;
    private float textureWidth;
    private float textureHeight;

    void Start()
    {
        //Smart Terrain is best experienced in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        aboutStyle = new GUIStyle();
        aboutStyle.normal.background = Resources.Load("GUI/about/about_me") as Texture2D;

        textureWidth = aboutStyle.normal.background.width;
        textureHeight = aboutStyle.normal.background.height;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("tap");
            Application.LoadLevelAsync(1);
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
