using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private InputManager inputManager;

    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    public int interactableLayerIndexMix;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private PhysicsObject physicsObject;
    private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField]
    private Transform pickupParent;
    public GameObject currentlyPickUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField]
    private float minSpeed = 0;
    [SerializeField]
    private float maxSpeed = 300f;
    [SerializeField]
    private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }

    
    void Update()
    {
        //check if we are looking at an interactable object
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if(Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {
            lookObject = hit.collider.transform.root.gameObject;
            
        }
        

        //if pickup button is pressed
        if (inputManager.PlayerPressedPickup())
        {
            //and we are not holding anything
            if(currentlyPickUpObject == null)
            {
                //and we are looking at an interactable object
                if(lookObject != null)
                {
                    PickupObject();
                }
            }
            //if we press the pickup button and have something we drop it
            else
            {
                BreakConnection();
            }
        }

        if (currentlyPickUpObject != null && currentDist > maxDistance) BreakConnection();
    }

    //physics
    private void FixedUpdate()
    {
        if(currentlyPickUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;

            //rotation
            lookRot = Quaternion.LookRotation(mainCamera.transform.position - currentlyPickUpObject.transform.position);
            lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
            pickupRB.MoveRotation(lookRot);
        }
    }

    public void BreakConnection()
    {
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickUpObject = null;
        physicsObject.pickedUp = false;
        currentDist = 0;
    }

    public void PickupObject()
    {
        physicsObject = lookObject.GetComponentInChildren<PhysicsObject>();
        currentlyPickUpObject = lookObject;
        pickupRB = currentlyPickUpObject.GetComponent<Rigidbody>();
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());
    }
}
