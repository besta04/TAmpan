using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

[RequireComponent(typeof( CharacterController ) )]
public class PersonController : MonoBehaviour 
{
    public GameObject weaponOnHand;
    //public GameObject key;
    public float movementSpeed = 3.0f;
    //public GameObject explosion;
    Animator animator;

    public bool appeared
    {
        get
        {
            return personAppear;
        }
    }

    private bool personAppear;
    private bool interactive;
    private CharacterController characterController;
    private Vector3 movementTarget;
    private const float MIN_DISTANCE = 50.05f;
    private bool characterMoving = false;
    private Vector3 lookRotationPoint;
    private Quaternion rotation;
    private bool moveBegin;
    private Vector3 lookRotationDirection;
    private DebugLogController debugLog;
    private CylinderGUIController gui;
    private Rigidbody playerRigidBody;
    private GameObject weapon;
    private GameObject crate;
    private GameObject house;
    private GameObject frontDoor;
    //private GameObject key;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        characterController = gameObject.GetComponent<CharacterController>();
        personAppear = true;
    }

	// Use this for initialization
	void Start () 
    {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        //weaponOnHand = GameObject.FindGameObjectWithTag("WeaponOnHand");
        crate = GameObject.FindGameObjectWithTag("Crate");
        //key = GameObject.Find("Kunci");
        house = GameObject.FindGameObjectWithTag("House");
        frontDoor = GameObject.FindGameObjectWithTag("FrontDoor");
        //explosion = GameObject.FindGameObjectWithTag("Explosion");
        debugLog = FindObjectOfType(typeof(DebugLogController)) as DebugLogController;
        gui = FindObjectOfType(typeof(CylinderGUIController)) as CylinderGUIController;
        this.transform.parent = null;
        animator = GetComponent<Animator>();
        //weaponOnHand.SetActive(false);
        //key.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(GUIManager.SingleTappedOnScreen)
        {
            Debug.Log("tapped!");
            debugLog.InsertLog("Tapped on screen");
            HandleSingleTap();
        }
        if(personAppear && interactive)
        {
            //Debug.Log("moved!");
            //animator.SetBool("run", false);
            Move();
        }
	}

    private void HandleSingleTap()
    {
        GameObject gameobject = QCARManager.Instance.ARCameraTransform.gameObject;
        Camera[] camera = gameobject.GetComponentsInChildren<Camera>();
        Ray ray = camera[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
                // tap front door, no key
                if (hitInfo.collider.CompareTag("House") && !gui.gotKey)
                {
                    debugLog.InsertLog("house tapped!");
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 2;
                }

                // front door, have key
                else if (hitInfo.collider.CompareTag("House") && gui.gotKey)
                {
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 9;
                }

                // in shoot radius, no weapon
                else if (hitInfo.collider.CompareTag("Crate") && !weaponOnHand.activeSelf)
                {
                    debugLog.InsertLog("in crate radius");
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 3;
                }

                // in box radius, can do shoot
                else if (hitInfo.collider.CompareTag("Crate") && weaponOnHand.activeSelf)
                {
                    debugLog.InsertLog("in shoot radius");
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 6;
                }
            
            // in weapon radius
                else if (hitInfo.collider.CompareTag("Weapon"))
                {
                    debugLog.InsertLog("in weapon radius");
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 4;
                }

            // in key radius
                else if (hitInfo.collider.CompareTag("Key") && !gui.gotKey)
                {
                    debugLog.InsertLog("in key radius");
                    lookRotationDirection = hitInfo.point - transform.position;
                    RotateUpdate();
                    gui.gameState = 8;
                }

                else
                {
                    movementTarget = hitInfo.point;
                    lookRotationDirection = hitInfo.point - transform.position;
                }
                RotateUpdate();
            Debug.Log("hit!");
            debugLog.InsertLog("Raycast Hit");
        }
        
        if(personAppear)
        {
            interactive = true;
        }
    }

    private void Move()
    {
        Vector2 currentVectorToTarget = 
            (new Vector2(movementTarget.x, movementTarget.z) -
            new Vector2(transform.position.x, transform.position.z));

        debugLog.InsertLog("jarak " + currentVectorToTarget.magnitude);
        debugLog.InsertLog("" + characterMoving);
        if(!characterMoving && currentVectorToTarget.magnitude >= MIN_DISTANCE)
        {
            //debugLog.InsertLog("move");
            characterMoving = true;
            animator.SetBool("run", true);
            animator.SetBool("aim", false);
        }

        if(characterMoving && currentVectorToTarget.magnitude < MIN_DISTANCE)
        {
            //debugLog.InsertLog("stop");
            characterMoving = false;
            if (weaponOnHand.activeSelf)
            {
                animator.SetBool("run", false);
                animator.SetBool("aim", true);
            }
            else
            {
                animator.SetBool("run", false);
                animator.SetBool("aim", false);
            }
        }

        if(characterMoving)
        {
            Vector2 movementVector = currentVectorToTarget.normalized * movementSpeed * Time.deltaTime;
            Vector3 direction = new Vector3(movementVector.x, 0, movementVector.y);
            Debug.Log(direction);
            //playerRigidBody.MovePosition(direction);
            characterController.Move(direction);
            //characterController.SimpleMove(direction);
            //float journeyLength = Vector3.Distance(transform.position, movementTarget);
            //float distCovered = Time.deltaTime * movementSpeed;
            //float fracJourney = distCovered / journeyLength;
            //transform.localPosition = Vector3.Lerp(transform.position, direction, fracJourney);
            RotateUpdate();
        }
        //Debug.Log(movementTarget);
        //debugLog.InsertLog("Character move to " + movementTarget);
        //transform.localPosition = new Vector3(transform.localPosition.x, 0.25f, transform.localPosition.z);
        //transform.localPosition.y = 0.0f;
    }

    private void RotateUpdate()
    {
        rotation = Quaternion.LookRotation(lookRotationDirection.normalized);
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8);
        //debugLog.InsertLog("Character rotating " + rotation.eulerAngles.y + " degrees");
    }
}
