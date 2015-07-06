using UnityEngine;
using System.Collections;
using Vuforia;

public enum UIStates
{
    OVERLAY_OUTLINE, INIT_ANIMATION, SCANNING, WAITING, RENDERING, RESET_ALL, BACK, NONE
}

public class OtherGUIController : MonoBehaviour {

    public bool primarySurfaceStagged;
    public bool cylinderTarget;

    private UIStates state;
    private GUIManager uiInput;
    private SurfaceBehaviour smartSurface;
    private SmartTerrainEventHandler smartTerrainEventHandler;
    private CylinderSmartTerrainEventHandler cylinderSmartTerrainEventHandler;
    private SmartTerrainTrackableEventHandler smartTerrainTrackableHandler;
    private SmartTerrainTracker smartTerrainTracker;
    //private WireframeTrackableEventHandler wireframeTrackableHandler;

    void Awake()
    {
        uiInput = new GUIManager();
    }

	// Use this for initialization
	void Start () {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        //uiInput = new GUIManager();

        smartTerrainTracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
        smartSurface = GameObject.FindObjectOfType(typeof(SurfaceBehaviour)) as SurfaceBehaviour;
        smartTerrainEventHandler = GameObject.FindObjectOfType(typeof(SmartTerrainEventHandler)) as SmartTerrainEventHandler;
        cylinderSmartTerrainEventHandler = GameObject.FindObjectOfType(typeof(CylinderSmartTerrainEventHandler)) as CylinderSmartTerrainEventHandler;
        smartTerrainTrackableHandler = GameObject.FindObjectOfType(typeof(SmartTerrainTrackableEventHandler)) as SmartTerrainTrackableEventHandler;
        //wireframeTrackableHandler = GameObject.FindObjectOfType(typeof(WireframeTrackableEventHandler)) as WireframeTrackableEventHandler;
        //wireframeTrackableHandler.testing = "fuuu";
        uiInput.TappedBackButton += uiInput_TappedBackButton;
        uiInput.TappedDoneButton += uiInput_TappedDoneButton;
        uiInput.TappedResetButton += uiInput_TappedResetButton;

        //state = UIStates.RENDERING;
	}

    void uiInput_TappedResetButton()
    {
        state = UIStates.RESET_ALL;
    }

    void uiInput_TappedDoneButton()
    {
        state = UIStates.RENDERING;
    }

    void uiInput_TappedBackButton()
    {
        state = UIStates.BACK;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(wireframeTrackableHandler.primarySurfaceStagged);
        //Debug.Log(primarySurfaceStagged);
        //Debug.Log(smartTerrainTrackableHandler.tes);
	}

    void OnGUI()
    {
        //if(GUI.Button(new Rect(25,25,200,50),"Back"))
        //{
        //    Application.LoadLevel(0);
        //}
        switch (state)
        {
            case UIStates.OVERLAY_OUTLINE:
                uiInput.UpdateTitle(HEADER_MESSAGE.POINT_DEVICE);
                smartSurface.GetComponent<Renderer>().enabled = false;
                if (cylinderTarget)
                {
                    uiInput.DrawCylinderTargetOutline();
                }
                else
                {
                    uiInput.DrawImageTargetOutline();
                }
                uiInput.DrawBackButton();
                if(smartTerrainTrackableHandler.trackablesFound)
                {
                    state = UIStates.WAITING;
                }
                break;

            case UIStates.WAITING:
                uiInput.UpdateTitle(HEADER_MESSAGE.WAIT);
                uiInput.DrawBackButton();
                smartSurface.GetComponent<Renderer>().enabled = false;
                //Debug.Log(wireframeTrackableHandler.primarySurfaceStagged);
                if (primarySurfaceStagged)
                {
                    state = UIStates.SCANNING;
                }
                else if (smartTerrainTrackableHandler.trackablesFound == false)
                {
                    state = UIStates.OVERLAY_OUTLINE;
                }
                //else
                //{
                    //state = UIStates.WAITING;
                //}
                break;

            case UIStates.SCANNING:
                uiInput.UpdateTitle(HEADER_MESSAGE.PULLBACK_SLOWLY);
                
                smartSurface.GetComponent<Renderer>().enabled = smartTerrainTrackableHandler.trackablesFound;
                uiInput.DrawBackButton();
                uiInput.DrawDoneButton();
                break;

            case UIStates.RENDERING:
                uiInput.UpdateTitle(HEADER_MESSAGE.DEFAULT);
                if (cylinderTarget)
                {
                    cylinderSmartTerrainEventHandler.ShowPropClones();
                }
                else
                {
                    smartTerrainEventHandler.ShowPropClones();
                }
                //smartTerrainTracker.StopMeshUpdates();
                //smartTerrainTracker.Stop();
                uiInput.DrawBackButton();
                uiInput.DrawResetButton();
                break;

            case UIStates.RESET_ALL:
                if (cylinderTarget)
                {
                    Application.LoadLevelAsync(2);
                }
                else
                {
                    Application.LoadLevelAsync(1);
                }
                state = UIStates.NONE;
                break;

            case UIStates.BACK:
                Application.LoadLevelAsync(0);
                break;

            case UIStates.NONE:
                break;
        }
    }
}
