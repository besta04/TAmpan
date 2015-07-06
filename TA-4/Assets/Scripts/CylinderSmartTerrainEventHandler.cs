using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Vuforia;

public class CylinderSmartTerrainEventHandler : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private ReconstructionBehaviour mReconstructionBehaviour;
    private DebugLogController debugLog;
    private bool propsCloned;
    private int keyCount = 0;
    //private GameObject text = GameObject.FindGameObjectWithTag("DebugText");

    #endregion // PRIVATE_MEMBERS


    #region PUBLIC_MEMBERS

    public PropBehaviour PropTemplate;
    public PropBehaviour PropTemplate2;
    public SurfaceBehaviour SurfaceTemplate;
    public GameObject propWillGenerated;
    public Hills HillsPrefab;
    public GameObject crate;
    public GameObject tanamanJalar;
    public GameObject stoneFormation;
    public GameObject kastil;
    public GameObject weapon;
    public GameObject key;
    public GameObject explode;
    public GameObject kotak;
    public GameObject kunci;
    public GameObject boom;

    public bool propsReallyCloned
    {
        get
        {
            return propsCloned;
        }
    }
    #endregion // PUBLIC_MEMBERS



    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        debugLog = GameObject.FindObjectOfType(typeof(DebugLogController)) as DebugLogController;
        
        mReconstructionBehaviour = GetComponent<ReconstructionBehaviour>();
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.RegisterInitializedCallback(OnInitialized);
            mReconstructionBehaviour.RegisterPropCreatedCallback(OnPropCreated);
            mReconstructionBehaviour.RegisterSurfaceCreatedCallback(OnSurfaceCreated);
        }
        //treeCan.SetActive(false);
        kastil.SetActive(false);
        debugLog.InsertLog("Smart Terrain started");
    }

    void OnDestroy()
    {
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.UnregisterInitializedCallback(OnInitialized);
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
        Debug.Log("---Created Prop ID: " + prop.ID);
        debugLog.InsertLog("---Created Prop ID: " + prop.ID);

        if (mReconstructionBehaviour)
        {
            // berdasarkan ketinggian, sumbu z
            //if (prop.BoundingBox.Center.y <= 100)
            //{
            //    //Debug.Log("min");
            //    mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
            //}
            //else if (prop.BoundingBox.Center.y > 100)
            //{
            //    //Debug.Log("pos");
            //    mReconstructionBehaviour.AssociateProp(PropTemplate2, prop);
            //}
            mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
            PropAbstractBehaviour behaviour;
            if (mReconstructionBehaviour.TryGetPropBehaviour(prop, out behaviour))
            {
                behaviour.gameObject.name = "Prop " + prop.ID;
            }
            //debugTextX.text = "Size X : " + prop.BoundingBox.Center.x.ToString();
            //debugTextY.text = "Size Y : " + prop.BoundingBox.Center.y.ToString();
            //debugTextZ.text = "Size Z : " + prop.BoundingBox.Center.z.ToString();

        }
        //mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
    }

    /// <summary>
    /// Called when a surface has been created
    /// </summary>
    public void OnSurfaceCreated(Surface surface)
    {
        Debug.Log("---Created Surface ID" + surface.ID);
        debugLog.InsertLog("---Created Surface ID" + surface.ID);

        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.AssociateSurface(SurfaceTemplate, surface);
            SurfaceAbstractBehaviour behaviour;
            if (mReconstructionBehaviour.TryGetSurfaceBehaviour(surface, out behaviour))
            {
                behaviour.gameObject.name = "Surface " + surface.ID;
            }
        }
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

                //Hills spawn = Instantiate(HillsPrefab) as Hills;
                Hills spawn = Instantiate(HillsPrefab) as Hills;
                spawn.name = "Hills";
                //spawn.transform.parent = BoundingBox;
                spawn.transform.localPosition = new Vector3(prop.transform.localPosition.x, prop.transform.localPosition.y + 260, prop.transform.localPosition.z);
                //spawn.transform.localScale = new Vector3(300, 300, 300);
                spawn.transform.localScale = prop.Prop.BoundingBox.HalfExtents; //new Vector3((prop.Prop.BoundingBox.Center.x), (prop.Prop.BoundingBox.Center.y), (prop.Prop.BoundingBox.Center.z));
                spawn.transform.localRotation = Quaternion.identity;
                //Instantiate(spawn);

                //HillsPrefab.gameObject.SetActive(true);
                //propWillGenerated.gameObject.SetActive(true);

                //GameObject tree = Instantiate(treeCan) as GameObject;
                //tree.name = "Trunks";
                //tree.transform.localPosition = prop.transform.localPosition;
                //tree.transform.localScale = new Vector3((prop.Prop.BoundingBox.Center.x), (prop.Prop.BoundingBox.Center.y), (prop.Prop.BoundingBox.Center.z));
                //tree.transform.localRotation = Quaternion.identity;

                GameObject stones = Instantiate(stoneFormation) as GameObject;
                stones.name = "Stones";
                stones.transform.localPosition = prop.transform.localPosition;
                stones.transform.localRotation = Quaternion.identity;

                GameObject jalar = Instantiate(tanamanJalar) as GameObject;
                jalar.name = "Tanaman Jalar";
                jalar.transform.localPosition = new Vector3(prop.transform.localPosition.x, prop.transform.localPosition.y, prop.transform.localPosition.z + 300);
                jalar.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                if(keyCount == 0)
                {
                    kotak = Instantiate(crate) as GameObject;
                    kotak.name = "Kotak";
                    kotak.transform.localPosition = new Vector3(prop.transform.localPosition.x, prop.transform.localPosition.y, prop.transform.localPosition.z - 300);

                    boom = Instantiate(explode) as GameObject;
                    boom.name = "Explosion";
                    boom.transform.localPosition = kotak.transform.localPosition;

                    kunci = Instantiate(key) as GameObject;
                    kunci.name = "Kunci";
                    kunci.transform.localPosition = kotak.transform.localPosition;

                    //keys.tag = "Key";
                    //keys.SetActive(false);
                    keyCount++;
                }
            }

            //key.SetActive(false);
            kastil.SetActive(true);
            weapon.SetActive(true);
            //treeCan.SetActive(true);
            propsCloned = true;
            debugLog.InsertLog(props.Length + " props cloned");
        }
    }

    public void OnInitialized(SmartTerrainInitializationInfo initializationInfo)
    {
        Debug.Log("Finished initializing at [" + Time.time + "]");
        debugLog.InsertLog("Finished initializing at [" + Time.time + "]");
    }

    public void OnPropDeleted(Prop prop)
    {
        Debug.Log("---Deleted Prop ID: " + prop.ID);
        debugLog.InsertLog("---Deleted Prop ID: " + prop.ID);
    }

    public void OnPropUpdated(Prop prop)
    {
        Debug.Log("---Updated Prop ID: " + prop.ID);
        debugLog.InsertLog("---Updated Prop ID: " + prop.ID);
    }

    public void OnSurfaceUpdated(SurfaceAbstractBehaviour surfaceBehaviour)
    {
        Debug.Log("Surface updated at [" + Time.time + "]");
        debugLog.InsertLog("Surface updated at [" + Time.time + "]");
    }
}



