using UnityEngine;
using System.Collections;

public class CylinderLoadingScreen : MonoBehaviour {

    public Texture Spinner;
    public GUIStyle Background;
    private bool changeLevel = true;

    void Awake()
    {
        Background = new GUIStyle();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Spinner = Resources.Load("GUI/splash/support-loading_edited") as Texture;
        Background.normal.background = Resources.Load("GUI/splash/text-loading") as Texture2D;
    }

	// Use this for initialization
	void Start () 
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        changeLevel = true;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(changeLevel)
        {
            LoadUserDefineTargetScene();
            changeLevel = false;
        }
	    
	}

    void OnGUI()
    {
        Matrix4x4 oldMatrix = GUI.matrix;
        float thisAngle = Time.frameCount * 4;

        Rect thisRect = new Rect(Screen.width / 2.0f - Spinner.width / 2f, Screen.height / 2.0f - Spinner.height / 2f,
                                 Spinner.width, Spinner.height);

        GUIUtility.RotateAroundPivot(thisAngle, thisRect.center);
        GUI.DrawTexture(thisRect, Spinner);
        GUI.matrix = oldMatrix;
        DrawBackground();
    }

    void DrawBackground()
    {
        float textWidth = Background.normal.background.width;
        float textHeight = Background.normal.background.height;

        float width = Screen.width;
        float height = (Screen.width / textWidth) * textHeight;

        float y = (Screen.height * 0.5f) - height * 0.7f;
        GUI.Box(new Rect(0, y, width, height), "", Background);
    }

    private void LoadUserDefineTargetScene()
    {
        Application.LoadLevelAsync(4);
    }
}
