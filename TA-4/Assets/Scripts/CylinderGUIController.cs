using UnityEngine;
using System.Collections;
using Vuforia;

public enum GUIStates
{
    OVERLAY_OUTLINE, SCANNING, WAITING, RENDERING, PLAY, RESET, HOME, NONE
}

public class CylinderGUIController : MonoBehaviour
{

    public bool primarySurfaceStagged;
    //public GameObject explosion;
    public GameObject flamethrower;
    //public GameObject key;
    public PersonController person;
    public int gameState = 1;
    public bool gotKey;
    public GameObject weaponOnHand;

    private bool cameraState = false;
    private bool isTap = false;
    private bool spawn = true;
    private float time1;
    private float time2;
    private GUIStates state;
    private GUIManager uiInput;
    private SurfaceBehaviour smartSurface;
    private SmartTerrainTrackableEventHandler smartTerrainTrackableEventHandler;
    private CylinderSmartTerrainEventHandler cylinderSmartTerrainEventHandler;
    private CylinderTrackableEventHandler cylinderTrackableEventHandler;
    private ReconstructionBehaviour reconstructionBehaviour;
    private DebugLogController debugLog;
    private GameObject weapon;
    private GameObject crate;
    private GameObject explosion;
    //private GameObject key;

    void Awake()
    {
        uiInput = new GUIManager();
    }

    // Use this for initialization
    void Start()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        //CameraDevice.Instance.SetFlashTorchMode(true);
        //uiInput = new GUIManager();
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        //weaponOnHand = GameObject.FindGameObjectWithTag("WeaponOnHand");
        //key = GameObject.FindGameObjectWithTag("Key");
        //crate = GameObject.Find("Kotak");
        //explosion = GameObject.FindGameObjectWithTag("Explosion");
        //flamethrower = GameObject.FindGameObjectWithTag("Flamethrower");

        weapon.SetActive(false);
        //explosion.SetActive(false);
        //weaponOnHand.SetActive(false);
        //person.gameObject.SetActive(false);
        //key.SetActive(false);

        smartSurface = GameObject.FindObjectOfType(typeof(SurfaceBehaviour)) as SurfaceBehaviour;
        cylinderSmartTerrainEventHandler = GameObject.FindObjectOfType(typeof(CylinderSmartTerrainEventHandler)) as CylinderSmartTerrainEventHandler;
        smartTerrainTrackableEventHandler = GameObject.FindObjectOfType(typeof(SmartTerrainTrackableEventHandler)) as SmartTerrainTrackableEventHandler;
        cylinderTrackableEventHandler = GameObject.FindObjectOfType(typeof(CylinderTrackableEventHandler)) as CylinderTrackableEventHandler;
        reconstructionBehaviour = FindObjectOfType(typeof(ReconstructionBehaviour)) as ReconstructionBehaviour;
        debugLog = FindObjectOfType(typeof(DebugLogController)) as DebugLogController;

        uiInput.TappedBackButton += uiInput_TappedBackButton;
        uiInput.TappedDoneButton += uiInput_TappedDoneButton;
        uiInput.TappedResetButton += uiInput_TappedResetButton;
        uiInput.TappedFlashOffButton += uiInput_TappedFlashOffButton;
        uiInput.TappedFlashOnButton += uiInput_TappedFlashOnButton;
        uiInput.TappedShootButton += uiInput_TappedShootButton;
        uiInput.TappedPickWeaponButton += uiInput_TappedPickWeaponButton;
        uiInput.TappedPickKeyButton += uiInput_TappedPickKeyButton;
        uiInput.TappedEnterHouseButton += uiInput_TappedEnterHouseButton;

        //state = GUIStates.PLAY;
        gameState = 1;
        gotKey = false;
        //uiInput.DrawFlashButton(false);
        
    }

    void uiInput_TappedEnterHouseButton()
    {
        Destroy(person.gameObject);
        spawn = false;
        gameState = 0;
    }

    void uiInput_TappedPickKeyButton()
    {
        Destroy(cylinderSmartTerrainEventHandler.kunci.gameObject);
        gotKey = true;
        gameState = 9;
    }

    void uiInput_TappedPickWeaponButton()
    {
        debugLog.InsertLog("pick button");
        Destroy(weapon);
        weaponOnHand.SetActive(true);
        gameState = 5;
        //state = GUIStates.PLAY;
    }

    void uiInput_TappedShootButton()
    {
        flamethrower.SetActive(true);
        //explosion = GameObject.Find("ShockFlame");
        //explosion.SetActive(true);
        //person.explosion.SetActive(true);
        cylinderSmartTerrainEventHandler.boom.gameObject.SetActive(true);
        StartCoroutine(Wait(2.5f));
        Destroy(cylinderSmartTerrainEventHandler.kotak.gameObject);
        //crate.SetActive(false);
        cylinderSmartTerrainEventHandler.kunci.gameObject.SetActive(true);
        gameState = 8;
    }

    void uiInput_TappedFlashOnButton()
    {
        CameraDevice.Instance.SetFlashTorchMode(false);
        //uiInput.DrawFlashButton(false);
        cameraState = false;
        debugLog.InsertLog("Flash turned off");
    }

    void uiInput_TappedFlashOffButton()
    {
        CameraDevice.Instance.SetFlashTorchMode(true);
        //uiInput.DrawFlashButton(true);
        cameraState = true;
        debugLog.InsertLog("Flash turned on");
    }

    void uiInput_TappedResetButton()
    {
        state = GUIStates.RESET;
    }

    void uiInput_TappedDoneButton()
    {
        state = GUIStates.RENDERING;
    }

    void uiInput_TappedBackButton()
    {
        state = GUIStates.HOME;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(1);
        }
    }

    void OnGUI()
    {
        switch (state)
        {
            // detection phase - when you search for target
            case GUIStates.OVERLAY_OUTLINE:
                debugLog.InsertLog("Detection phase");
                uiInput.UpdateTitle(HEADER_MESSAGE.POINT_DEVICE);
                smartSurface.GetComponent<Renderer>().enabled = false;
                uiInput.DrawCylinderTargetOutline();
                uiInput.DrawBackButton();
                uiInput.DrawFlashButton(cameraState);
                if (cylinderTrackableEventHandler.cylinderDetected)
                {
                    state = GUIStates.WAITING;
                }
                break;

            // waiting phase - when target found but smart terrain no active yet
            case GUIStates.WAITING:
                debugLog.InsertLog("Waiting phase");
                uiInput.UpdateTitle(HEADER_MESSAGE.WAIT);
                uiInput.DrawBackButton();
                uiInput.DrawFlashButton(cameraState);
                smartSurface.GetComponent<Renderer>().enabled = false;
                //Debug.Log(wireframeTrackableHandler.primarySurfaceStagged);
                if (primarySurfaceStagged)
                {
                    state = GUIStates.SCANNING;
                }
                else if (cylinderTrackableEventHandler.cylinderDetected == false)
                {
                    state = GUIStates.OVERLAY_OUTLINE;
                }
                break;

            // scanning phase - when smart terrain on and surface tracking
            case GUIStates.SCANNING:
                debugLog.InsertLog("Scanning phase");
                uiInput.UpdateTitle(HEADER_MESSAGE.PULLBACK_SLOWLY);

                smartSurface.GetComponent<Renderer>().enabled = primarySurfaceStagged;
                uiInput.DrawBackButton();
                uiInput.DrawDoneButton();
                uiInput.DrawFlashButton(cameraState);
                break;

            // rendering phase - user tap done button and props will rendered
            case GUIStates.RENDERING:
                debugLog.InsertLog("Rendering phase");
                if ((reconstructionBehaviour != null) && (reconstructionBehaviour.Reconstruction != null))
                {
                    cylinderSmartTerrainEventHandler.ShowPropClones();
                    cylinderSmartTerrainEventHandler.kunci.gameObject.SetActive(false);
                    reconstructionBehaviour.Reconstruction.Stop();
                    state = GUIStates.PLAY;
                }
                //uiInput.DrawFlashButton(cameraState);
                break;
                
            // user interaction phase - character will appear
            case GUIStates.PLAY:
                //debugLog.InsertLog("User interaction phase");
                //if(cylinderSmartTerrainEventHandler.propsReallyCloned && person.appeared)
                //{
                if (spawn)
                {
                    person.gameObject.SetActive(true);
                }
                    //debugLog.InsertLog("Character spawn");
                //}
                debugLog.InsertLog("gamestate " + gameState);
                if(gameState == 0)
                {
                    uiInput.UpdateTitle(HEADER_MESSAGE.DEFAULT);
                }
                else if(gameState == 1)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.FIND_WAY);
                }
                else if(gameState == 2)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.TAP_FRONTDOOR);
                }
                else if(gameState == 3)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.TAP_BOX);
                }
                else if (gameState == 4)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.WEAPON_RADIUS);
                    uiInput.DrawPickButton("weapon");
                }
                else if (gameState == 5)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.WEAPON_PICKED);
                }
                else if (gameState == 6)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.BOX_RADIUS);
                    uiInput.DrawShootButton();
                }
                else if (gameState == 7)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.BOX_DESTROYED);
                }
                else if (gameState == 8)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.KEY_RADIUS);
                    uiInput.DrawPickButton("key");
                }
                else if (gameState == 9)
                {
                    uiInput.UpdateGameTitle(GAME_MESSAGE.TAP_FRONTDOOR_KEY);
                    uiInput.DrawEnterHouseButton();
                }
                
                uiInput.DrawBackButton();
                uiInput.DrawResetButton();
                uiInput.DrawFlashButton(cameraState);
                break;

            // reset - user press reset button
            case GUIStates.RESET:
                Application.LoadLevelAsync(3);
                state = GUIStates.NONE;
                break;

            // home - user press home button whenever the phase
            case GUIStates.HOME:
                Application.LoadLevel(0);
                break;

            case GUIStates.NONE:
                break;
        }
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        //yield return new WaitForEndOfFrame();
    }
}
