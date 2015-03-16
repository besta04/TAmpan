/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
///  A custom handler that implements the ITrackerEventHandler interface.
/// </summary>
public class SmartTerrainEventHandler : MonoBehaviour, ISmartTerrainEventHandler
{

    #region PRIVATE_MEMBERS

    private bool m_propsCloned;

    #endregion //PRIVATE MEMBERS
    #region PUBLIC_MEMBERS

    public PropBehaviour PropTemplate;
    public Iceberg IcePrefab;
    public bool propsCloned
    {
        get
        {
            return m_propsCloned;
        }
    }

    #endregion

    #region UNITY_MONOBEHAVIOUR

    void Start()
    {
        SmartTerrainBehaviour behaviour = GetComponent<SmartTerrainBehaviour>();
        if (behaviour)
        {
            behaviour.RegisterSmartTerrainEventHandler(this);
        }
    }

    #endregion //UNITY_MONOBEHAVIOUR

    #region ISmartTerrainEventHandler_Implementations

    public void OnInitialized(SmartTerrainInitializationInfo initializationInfo)
    {
        Debug.Log("Finished initializing at [" + Time.time + "]");
    }

    public void OnPropCreated(Prop prop)
    {
        Debug.Log("---Created Prop ID: " + prop.ID);

        //shows an example of how you could get a handle on the prop game objects to perform different game logic
        var manager = TrackerManager.Instance.GetStateManager().GetSmartTerrainManager();

        manager.AssociateProp(PropTemplate, prop);
        PropAbstractBehaviour behaviour;
        if (manager.TryGetPropBehaviour(prop, out behaviour))
        {
            behaviour.gameObject.name = "Prop " + prop.ID;
        }
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

    #endregion // ISmartTerrainEventHandler_Implementations

    #region PUBLIC_METHODS

    public void ShowPropClones()
    {
        if (!m_propsCloned)
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

                Iceberg effect = Instantiate(IcePrefab) as Iceberg;
                effect.name = "Ice";
                effect.transform.parent = BoundingBox;
                effect.transform.localPosition = new Vector3(0f, 0.032f, 0f);
                effect.transform.localScale = new Vector3(100, 50, 100);
                effect.transform.localRotation = Quaternion.identity;

            }

            m_propsCloned = true;
        }
    }

    #endregion //PUBLIC_METHODS

}



