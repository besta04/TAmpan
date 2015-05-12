using UnityEngine;
using System.Collections;

public enum HEADER_MESSAGE
{
    POINT_DEVICE, PULLBACK_SLOWLY, DEFAULT, WAIT
}

public class GUIManager : MonoBehaviour {

    public event System.Action TappedBackButton;
    public event System.Action TappedDoneButton;
    public event System.Action TappedResetButton;

    private GUIStyle imageTargetOverlay;
    private GUIStyle cylinderTargetOverlay;
    private GUIStyle styleHeader;
    private Texture2D[] headerTextures;
    private GUIStyle backButton;
    private GUIStyle doneButton;
    private GUIStyle resetButton;

    private static bool guiInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GUIManager()
    {
        imageTargetOverlay = new GUIStyle();
        cylinderTargetOverlay = new GUIStyle();
        styleHeader = new GUIStyle();
        backButton = new GUIStyle();
        doneButton = new GUIStyle();
        resetButton = new GUIStyle();

        headerTextures = new Texture2D[4];
        headerTextures[0] = Resources.Load("GUI/point_image_target") as Texture2D;
        headerTextures[1] = Resources.Load("GUI/pullback") as Texture2D;
        headerTextures[2] = Resources.Load("GUI/default") as Texture2D;
        headerTextures[3] = Resources.Load("GUI/wait") as Texture2D;

        backButton.normal.background = Resources.Load("GUI/home") as Texture2D;
        //backButton.active.background = Resources.Load("GUI/back") as Texture2D;

        doneButton.normal.background = Resources.Load("GUI/done") as Texture2D;

        resetButton.normal.background = Resources.Load("GUI/reset") as Texture2D;

        imageTargetOverlay.normal.background = Resources.Load("GUI/ImageTargetOutline") as Texture2D;
        cylinderTargetOverlay.normal.background = Resources.Load("GUI/cylinderTargetOutline") as Texture2D;

        UpdateTitle(HEADER_MESSAGE.POINT_DEVICE);
    }

    public void UpdateTitle(HEADER_MESSAGE message)
    {
        int index = (int) message;
        if (index >= 0 && index < headerTextures.Length)
        {
            styleHeader.normal.background = headerTextures[index];

            float texWidth = styleHeader.normal.background.width;
            float texHeight = styleHeader.normal.background.height;

            float width = Screen.width;
            float height = (width / texWidth) * texHeight;

            float y = (Screen.height) - height;
            GUI.Label(new Rect(0, y, width, height), "", styleHeader);
        }
    }

    public void DrawImageTargetOutline()
    {
        float textWidth = imageTargetOverlay.normal.background.width;
        float textHeight = imageTargetOverlay.normal.background.height;

        float width = Screen.width;
        float height = (Screen.width / textWidth) * textHeight;

        float y = (Screen.height * 0.5f) - height * 0.6f;
        GUI.Box(new Rect(0, y, width, height), "", imageTargetOverlay);
    }

    public void DrawCylinderTargetOutline()
    {
        float textWidth = cylinderTargetOverlay.normal.background.width;
        float textHeight = cylinderTargetOverlay.normal.background.height;

        float width = Screen.width;
        float height = (Screen.width / textWidth) * textHeight;

        float y = (Screen.height * 0.5f) - height * 0.6f;
        GUI.Box(new Rect(0, y, width, height), "", cylinderTargetOverlay);
    }

    public void DrawBackButton()
    {
        float texWidth = backButton.normal.background.width;
        float texHeight = backButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        //float x = Screen.width - width;
        if (GUI.Button(new Rect(0, y, width, height), "", backButton))
        {
            if (this.TappedBackButton != null)
            {
                this.TappedBackButton();
            }
            guiInput = true;
        }
    }

    public void DrawDoneButton()
    {
        float texWidth = doneButton.normal.background.width;
        float texHeight = doneButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", doneButton))
        {
            if (this.TappedDoneButton != null)
            {
                this.TappedDoneButton();
            }
            guiInput = true;
        }
    }

    public void DrawResetButton()
    {
        float texWidth = resetButton.normal.background.width;
        float texHeight = resetButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", resetButton))
        {
            if (this.TappedResetButton != null)
            {
                this.TappedResetButton();
            }
            guiInput = true;
        }
    }

}
