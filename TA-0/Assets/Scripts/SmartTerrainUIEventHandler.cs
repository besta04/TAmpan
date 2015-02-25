/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// UI Event Handler class that handles events generated by user-tap actions
/// over the UI Options Menu
/// </summary>
public class SmartTerrainUIEventHandler : ISampleAppUIEventHandler { 
    
    #region PUBLIC_MEMBER_VARIABLES
    public override event System.Action CloseView;
    public override event System.Action GoToAboutPage;
    #endregion PUBLIC_MEMBER_VARIABLES
    
    #region PRIVATE_MEMBER_VARIABLES
    private static bool sExtendedTrackingIsEnabled;
    private SmartTerrainUIView mView;
	private SmartTerrainTracker mTracker;
    #endregion PRIVATE_MEMBER_VARIABLES
   
	#region PUBLIC_MEMBER_PROPERTIES
    public SmartTerrainUIView View
    {
        get {
            if(mView == null){
                mView = new SmartTerrainUIView();
                mView.LoadView();
            }
            return mView;
        }
    }

    #endregion PUBLIC_MEMBER_PROPERTIES
    
    #region PUBLIC_METHODS
    public override void UpdateView (bool tf)
    {
        this.View.UpdateUI(tf);
    }
    
    public override  void Bind()
    {
        this.View.mCloseButton.TappedOn         += OnTappedToClose;
        this.View.mAboutLabel.TappedOn          += OnTappedOnAboutButton;
        this.View.mCameraFlashSettings.TappedOn += OnTappedToTurnOnFlash;
        this.View.mAutoFocusSetting.TappedOn    += OnTappedToTurnOnAutoFocus;
		this.View.mStartStopScanning.TappedOn += OnTappedOnScanning;
		this.View.mReset.TappedOn += OnTappedOnReset;

        EnableContinuousAutoFocus();

        mTracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
    }
    
    public override  void UnBind()
    { 
        this.View.mCloseButton.TappedOn         -= OnTappedToClose;
        this.View.mAboutLabel.TappedOn          -= OnTappedOnAboutButton;
        this.View.mCameraFlashSettings.TappedOn -= OnTappedToTurnOnFlash;
        this.View.mAutoFocusSetting.TappedOn    -= OnTappedToTurnOnAutoFocus;
		this.View.mStartStopScanning.TappedOn -= OnTappedOnScanning;
		this.View.mReset.TappedOn -= OnTappedOnReset;
        this.View.UnLoadView();
        mView = null;
    }
    
     public override  void TriggerAutoFocus()
    {
        StartCoroutine(TriggerAutoFocusAndEnableContinuousFocusIfSet());
    }
    
    public override void SetToDefault(bool tf)
    {
        this.View.mCameraFlashSettings.Enable(tf);
    }
   
    #endregion PUBLIC_METHODS
    
    #region PRIVATE_METHODS
  
	 /// <summary>
    /// Activating trigger autofocus mode unsets continuous focus mode (if was previously enabled from the UI Options Menu)
    /// So, we wait for a second and turn continuous focus back on (if options menu shows as enabled)
    /// </returns>
    private IEnumerator TriggerAutoFocusAndEnableContinuousFocusIfSet()
    {
        //triggers a single autofocus operation 
        if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO)) {
              this.View.FocusMode = CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO;
        }
        
        yield return new WaitForSeconds(1.0f);
         
        //continuous focus mode is turned back on if it was previously enabled from the options menu
        if(this.View.mAutoFocusSetting.IsEnabled)
        {
            if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO)) {
              this.View.FocusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;
            }
        }
        
        Debug.Log (this.View.FocusMode);
        
    }
	
    private void OnTappedToTurnOnFlash(bool tf)
    {
        if(tf)
        {
            if(!CameraDevice.Instance.SetFlashTorchMode(true))
            {
                this.View.mCameraFlashSettings.Enable(false);
            }
        }
        else 
        {
            CameraDevice.Instance.SetFlashTorchMode(false);
        }
        
        OnTappedToClose();
    }

    //We want autofocus to be enabled when the app starts
    private void EnableContinuousAutoFocus()
    {
        if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
        {
            this.View.FocusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;
            this.View.mAutoFocusSetting.Enable(true);
        }
    }
	
    private void OnTappedToTurnOnAutoFocus(bool tf)
    {
        if(tf)
        {
            if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
            {
                this.View.FocusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;
            }
            else 
            {
                this.View.mAutoFocusSetting.Enable(false);
            }
        }
        else 
        {
            if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL))
            {
                this.View.FocusMode = CameraDevice.FocusMode.FOCUS_MODE_NORMAL;
            }
        }
        
        OnTappedToClose();
    }
	
	  
    private void OnTappedToClose()
    {
        if(this.CloseView != null)
        {
            this.CloseView();
        }
    }
    
    
    private void OnTappedOnAboutButton(bool tf)
    {
        if(this.GoToAboutPage != null)
        {
            this.GoToAboutPage();
        }
    }
	
	private void OnTappedOnScanning(bool tf)
	{
		if(this.View.mStartStopScanning.Title == "Start")
		{
			Debug.Log ("Start Scanning [" + Time.time + "]");
            mTracker.StartMeshUpdates();
			this.View.mStartStopScanning.Title = "Stop";
		}
		else
		{
			Debug.Log ("Stop Scanning [" + Time.time + "]");
			mTracker.StopMeshUpdates();
			this.View.mStartStopScanning.Title = "Start";
		}
		
		OnTappedToClose();
	}
		
	private void OnTappedOnReset(bool tf)
	{
		Debug.Log ("Reset Smart Terrain [" + Time.time + "]");

	    bool trackerWasActive = mTracker.IsActive;
        // first stop the tracker
        if (trackerWasActive)
            mTracker.Stop();
        // now you can reset...
		mTracker.Reset();
        // ... and restart the tracker
        if (trackerWasActive)
        {
            mTracker.Start();
            mTracker.StartMeshUpdates();
        }

        this.View.mStartStopScanning.Title = "Stop";
		OnTappedToClose();
	}
	
   
    #endregion PRIVATE_METHODS
}

