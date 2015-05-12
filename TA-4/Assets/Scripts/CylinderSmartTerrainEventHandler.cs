/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/


using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A default event handler that handles reconstruction events for a ReconstructionFromTarget
    /// It uses a single Prop template that is used for every newly created prop, 
    /// and a surface template that is used for the primary surface
    /// </summary>
    public class CylinderSmartTerrainEventHandler : MonoBehaviour
    {
        #region PRIVATE_MEMBERS

        private ReconstructionBehaviour mReconstructionBehaviour;
        private bool propsCloned;

        #endregion // PRIVATE_MEMBERS


        #region PUBLIC_MEMBERS

        public PropBehaviour PropTemplate;
        public PropBehaviour PropTemplate2;
        public SurfaceBehaviour SurfaceTemplate;

        #endregion // PUBLIC_MEMBERS



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mReconstructionBehaviour = GetComponent<ReconstructionBehaviour>();
            if (mReconstructionBehaviour)
            {
                mReconstructionBehaviour.RegisterPropCreatedCallback(OnPropCreated);
                mReconstructionBehaviour.RegisterSurfaceCreatedCallback(OnSurfaceCreated);
            }
        }

        void OnDestroy()
        {
            if (mReconstructionBehaviour)
            {
                mReconstructionBehaviour.UnregisterPropCreatedCallback(OnPropCreated);
                mReconstructionBehaviour.UnregisterSurfaceCreatedCallback(OnSurfaceCreated);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region RECONSTRUCTION_CALLBACKS

        /// <summary>
        /// Called when a prop has been created
        /// </summary>
        public void OnPropCreated(Prop prop)
        {
            Debug.Log("created prop");
            if (mReconstructionBehaviour)
            {
                // berdasarkan ketinggian, sumbu y 
                if (prop.BoundingBox.Center.y <= 100)
                {
                    //Debug.Log("min");
                    mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
                }
                else if (prop.BoundingBox.Center.y > 100)
                {
                    //Debug.Log("pos");
                    mReconstructionBehaviour.AssociateProp(PropTemplate2, prop);
                }
            }
            //mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
        }

        /// <summary>
        /// Called when a surface has been created
        /// </summary>
        public void OnSurfaceCreated(Surface surface)
        {
            if (mReconstructionBehaviour)
                mReconstructionBehaviour.AssociateSurface(SurfaceTemplate, surface);
        }

        #endregion // RECONSTRUCTION_CALLBACKS

        public void ShowPropClones()
        {
            if (!propsCloned)
            {
                PropAbstractBehaviour[] props = GameObject.FindObjectsOfType(typeof(PropAbstractBehaviour)) as PropAbstractBehaviour[];

                foreach (PropAbstractBehaviour prop in props)
                {
                    Transform BoundingBox = prop.transform.FindChild("BoundingBoxCollider");
                    BoxCollider collider = BoundingBox.GetComponent<BoxCollider>();
                    collider.isTrigger = false;

                    prop.SetAutomaticUpdatesDisabled(true);
                    Renderer propRenderer = prop.GetComponent<MeshRenderer>();
                    if (propRenderer != null)
                    {
                        Destroy(propRenderer);
                    }
                }

                propsCloned = true;
            }
        }
    }
}



