using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class SmartTerrainTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {

        public bool trackablesFound;
        //public string tes = "defgh";

        #region PRIVATE_MEMBER_VARIABLES

        private CylinderTargetAbstractBehaviour cylinderTarget;
        private TrackableBehaviour trackableBehaviour;
        private CylinderGUIController GUI;
        private DebugLogController debugLog;

        private bool trackableDetectedFirstTime = true;
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            debugLog = FindObjectOfType(typeof(DebugLogController)) as DebugLogController;
            cylinderTarget = FindObjectOfType(typeof(CylinderTargetAbstractBehaviour)) as CylinderTargetAbstractBehaviour;
            GUI = FindObjectOfType(typeof(CylinderGUIController)) as CylinderGUIController;

            trackableBehaviour = GetComponent<TrackableBehaviour>();
            if (trackableBehaviour)
            {
                trackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                GUI.primarySurfaceStagged = true;
                OnTrackingFound();
            }
            else
            {
                GUI.primarySurfaceStagged = false;
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            // Enable wireframe rendering:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + trackableBehaviour.TrackableName + " found");
            debugLog.InsertLog("Trackable " + trackableBehaviour.TrackableName + " found");

            if (cylinderTarget != null)
            {
                Renderer[] rendererComponentsOfCylinder = cylinderTarget.gameObject.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer component in rendererComponentsOfCylinder)
                {
                    component.enabled = true;
                }
            }

            trackablesFound = true;
            //tes = "opqrs";
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            // Disable wireframe rendering:
            foreach (WireframeBehaviour component in wireframeComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + trackableBehaviour.TrackableName + " lost");
            debugLog.InsertLog("Trackable " + trackableBehaviour.TrackableName + " lost");

            //hide the soda can and iceberg only when smart terrain tracking is lost.

            if (cylinderTarget != null)
            {
                Renderer[] rendererComponentsOfCylinder = cylinderTarget.gameObject.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer component in rendererComponentsOfCylinder)
                {
                    component.enabled = false;
                }
            }

            trackablesFound = false;
        }

        #endregion // PRIVATE_METHODS
    }
}
