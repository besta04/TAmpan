using UnityEngine;
using System.Collections;

public class CustomTrackerEventHandler : MonoBehaviour, ITrackerEventHandler {

	 #region UNTIY_MONOBEHAVIOUR_METHODS


    public void Start()
    {

        QCARAbstractBehaviour qcarBehaviour = (QCARAbstractBehaviour)FindObjectOfType(typeof(QCARAbstractBehaviour));
        if (qcarBehaviour)
        {
            qcarBehaviour.RegisterTrackerEventHandler(this);
        }

    }


    // Deinitialize the tracker when the Behaviour is destroyed.
    void OnDestroy()
    {
        // unregister from the QCARBehaviour
        QCARAbstractBehaviour qcarBehaviour = (QCARAbstractBehaviour)FindObjectOfType(typeof(QCARAbstractBehaviour));
        if (qcarBehaviour)
        {
            qcarBehaviour.UnregisterTrackerEventHandler(this);
        }
    }
	#endregion UNTIY_MONOBEHAVIOUR_METHODS

	#region ITrackerEventHandler implementation
	public void OnInitialized ()
	{
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	public void OnTrackablesUpdated ()
	{
		 
	}
	#endregion
}
