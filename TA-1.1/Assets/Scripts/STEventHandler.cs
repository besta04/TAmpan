/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/


using UnityEngine;

/// <summary>
///  A custom handler that implements the ITrackerEventHandler interface.
/// </summary>
public class STEventHandler : MonoBehaviour, ISmartTerrainEventHandler
{
    #region PUBLIC_MEMBERS

    public PropBehaviour PropTemplate;

    #endregion // PUBLIC_MEMBERS


    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        SmartTerrainBehaviour behaviour = GetComponent<SmartTerrainBehaviour>();
        if (behaviour)
        {
            behaviour.RegisterSmartTerrainEventHandler(this);
        }
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    public void OnInitialized(SmartTerrainInitializationInfo initializationInfo)
    {
        Debug.Log("Finished initializing");
    }

    public void OnPropCreated(Prop prop)
    {
        Debug.Log("---Created Prop");
        var manager = TrackerManager.Instance.GetStateManager().GetSmartTerrainManager();

        manager.AssociateProp(PropTemplate, prop);
    }

    public void OnPropUpdated(Prop prop)
    {
        Debug.Log("---Updated Prop");
    }

    public void OnPropDeleted(Prop prop)
    {
        Debug.Log("---Deleted Prop");
    }

    public void OnSurfaceUpdated(SurfaceAbstractBehaviour surfaceBehaviour)
    {
        Debug.Log("--- Primary surface has been updated");
    }

    #endregion // PUBLIC_METHODS
}



