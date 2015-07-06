using UnityEngine;
using System.Collections;

public enum HEADER_MESSAGE
{
    POINT_DEVICE, PULLBACK_SLOWLY, DEFAULT, WAIT
}

public enum GAME_MESSAGE
{
    FIND_WAY, TAP_FRONTDOOR, TAP_BOX, WEAPON_RADIUS, WEAPON_PICKED, BOX_RADIUS, BOX_DESTROYED, KEY_RADIUS, TAP_FRONTDOOR_KEY
}

public class GUIManager : MonoBehaviour {

    public event System.Action TappedBackButton;
    public event System.Action TappedDoneButton;
    public event System.Action TappedResetButton;
    public event System.Action TappedFlashOnButton;
    public event System.Action TappedFlashOffButton;
    public event System.Action TappedPickWeaponButton;
    public event System.Action TappedPickKeyButton;
    public event System.Action TappedShootButton;
    public event System.Action TappedEnterHouseButton;

    private GUIStyle imageTargetOverlay;
    private GUIStyle cylinderTargetOverlay;
    private GUIStyle styleHeader;
    private Texture2D[] headerTextures;
    private Texture2D[] gameTextures;
    private GUIStyle backButton;
    private GUIStyle doneButton;
    private GUIStyle resetButton;
    private GUIStyle flashOnButton;
    private GUIStyle flashOffButton;

    private static bool guiInput;

    public static bool SingleTappedOnScreen
    {
        get
        {
            bool tappedNoGUI = false;
            if(!guiInput && Input.GetMouseButtonUp(0))
            {
                tappedNoGUI = true;
            }
            else
            {
                guiInput = false;
            }
            return tappedNoGUI;
        }
    }

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
        flashOffButton = new GUIStyle();
        flashOnButton = new GUIStyle();

        headerTextures = new Texture2D[4];
        headerTextures[0] = Resources.Load("GUI/point_image_target") as Texture2D;
        headerTextures[1] = Resources.Load("GUI/pullback") as Texture2D;
        headerTextures[2] = Resources.Load("GUI/default") as Texture2D;
        headerTextures[3] = Resources.Load("GUI/wait") as Texture2D;

        gameTextures = new Texture2D[9];
        gameTextures[0] = Resources.Load("GUI/new/1-find_way_house") as Texture2D;
        gameTextures[1] = Resources.Load("GUI/new/2-tap_frontdoor") as Texture2D;
        gameTextures[2] = Resources.Load("GUI/new/3-tap_box") as Texture2D;
        gameTextures[3] = Resources.Load("GUI/new/4-gun_radius") as Texture2D;
        gameTextures[4] = Resources.Load("GUI/new/5-gun_picked") as Texture2D;
        gameTextures[5] = Resources.Load("GUI/new/6-box_radius") as Texture2D;
        gameTextures[6] = Resources.Load("GUI/new/7-box_destroyed") as Texture2D;
        gameTextures[7] = Resources.Load("GUI/new/8-key_radius") as Texture2D;
        gameTextures[8] = Resources.Load("GUI/new/9-tap_frontdoor_have_key") as Texture2D;

        backButton.normal.background = Resources.Load("GUI/home") as Texture2D;
        //backButton.active.background = Resources.Load("GUI/back") as Texture2D;

        doneButton.normal.background = Resources.Load("GUI/done") as Texture2D;

        resetButton.normal.background = Resources.Load("GUI/reset") as Texture2D;

        flashOnButton.normal.background = Resources.Load("GUI/flash/flash_on") as Texture2D;
        flashOffButton.normal.background = Resources.Load("GUI/flash/flash_off") as Texture2D;

        imageTargetOverlay.normal.background = Resources.Load("GUI/ImageTargetOutline") as Texture2D;
        cylinderTargetOverlay.normal.background = Resources.Load("GUI/cylinderTargetOutline") as Texture2D;

        //UpdateTitle(HEADER_MESSAGE.POINT_DEVICE);
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

    public void UpdateGameTitle(GAME_MESSAGE message)
    {
        int index = (int)message;
        if (index >= 0 && index < gameTextures.Length)
        {
            styleHeader.normal.background = gameTextures[index];

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

    public void DrawPickButton(string whatToPick)
    {
        float texWidth = resetButton.normal.background.width;
        float texHeight = resetButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", resetButton))
        {
            if (this.TappedPickWeaponButton != null && whatToPick == "weapon")
            {
                this.TappedPickWeaponButton();
            }
            else if (this.TappedPickKeyButton != null && whatToPick == "key")
            {
                this.TappedPickKeyButton();
            }
            guiInput = true;
        }
    }

    public void DrawShootButton()
    {
        float texWidth = resetButton.normal.background.width;
        float texHeight = resetButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", resetButton))
        {
            if (this.TappedShootButton != null)
            {
                this.TappedShootButton();
            }
            guiInput = true;
        }
    }

    public void DrawEnterHouseButton()
    {
        float texWidth = resetButton.normal.background.width;
        float texHeight = resetButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", resetButton))
        {
            if (this.TappedEnterHouseButton != null)
            {
                this.TappedEnterHouseButton();
            }
            guiInput = true;
        }
    }

    public void DrawFlashButton(bool flash)
    {
        float texWidth = flashOnButton.normal.background.width;
        float texHeight = flashOnButton.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if(flash)
        {
            if (GUI.Button(new Rect(x, 0, width, height), "", flashOnButton))
            {
                if (this.TappedFlashOnButton != null)
                {
                    this.TappedFlashOnButton();
                }
                guiInput = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(x, 0, width, height), "", flashOffButton))
            {
                if (this.TappedFlashOffButton != null)
                {
                    this.TappedFlashOffButton();
                }
                guiInput = true;
            }
        }
    }

}
