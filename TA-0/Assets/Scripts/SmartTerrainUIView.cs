/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;

public class SmartTerrainUIView : ISampleAppUIView {
    #region PUBLIC_PROPERTIES
    public CameraDevice.FocusMode FocusMode
    {
        get {
            return mFocusMode;
        }
        set {
            mFocusMode = value;
        }
    }
    #endregion PUBLIC_PROPERTIES
    
    #region PUBLIC_MEMBER_VARIABLES
    public event System.Action TappedToClose;
    public SampleAppUIBox mBox;
    public SampleAppUICheckButton mAboutLabel;
    public SampleAppUILabel mSmartTerrainLabel;
    public SampleAppUICheckButton mCameraFlashSettings;
    public SampleAppUICheckButton mAutoFocusSetting;
	public SampleAppUILabel mScanningLabel;
	public SampleAppUICheckButton mStartStopScanning;
	public SampleAppUICheckButton mReset;
    //public SampleAppUICheckButton mTracker;
    //public SampleAppUICheckButton mObjectMeshes;
    public SampleAppUIButton mCloseButton;
    //public SampleAppUICheckButton mResetButton;
	
    #endregion PUBLIC_MEMBER_VARIABLES
    
    #region PRIVATE_MEMBER_VARIABLES
    private CameraDevice.FocusMode mFocusMode;
	private SampleAppsUILayout mLayout;
    #endregion PRIVATE_MEMBER_VARIABLES
    
    #region PUBLIC_METHODS
    
    public void LoadView()
    {	
		mLayout = new SampleAppsUILayout();
		mSmartTerrainLabel = mLayout.AddLabel("Smart Terrain");
		mAboutLabel = mLayout.AddSimpleButton("About");
		mLayout.AddGap(2);
		mCameraFlashSettings = mLayout.AddSlider("Flash", false);
		mLayout.AddGap(2);
		mAutoFocusSetting = mLayout.AddSlider ("Autofocus", true);
		mLayout.AddGap(16);
		mScanningLabel = mLayout.AddGroupLabel("Scanning");
		mStartStopScanning = mLayout.AddSimpleButton("Stop");
		mLayout.AddGap(2);
		mReset = mLayout.AddSimpleButton("Reset");
		
		Rect CloseButtonRect = new Rect(0, Screen.height - (100 * Screen.width) / 800.0f, Screen.width, (70.0f * Screen.width) / 800.0f);
		mCloseButton = mLayout.AddButton("Close",CloseButtonRect);

    }
    
    public void UnLoadView()
    {
        mAboutLabel = null;
        mSmartTerrainLabel = null;
        mCameraFlashSettings = null;
        mAutoFocusSetting = null;
        mScanningLabel = null;
        mStartStopScanning = null;
        mReset = null;
        mCloseButton = null;
    }
    
    public void UpdateUI(bool tf)
    {
        if(!tf)
        {
            return;
        }
        
        mLayout.Draw();
    }

    public void OnTappedToClose ()
    {
        if(this.TappedToClose != null)
        {
            this.TappedToClose();
        }
    }
    #endregion PUBLIC_METHODS
}

