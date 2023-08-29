using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CharacterController player;
    Rigidbody rb;

    bool combat = false;

    public BattleAreas battleAreas;
    public bool questPanel = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        combat = battleAreas.battleTriggered;

        if (!combat)
        {
            FreeRoamCamera();
        }
        else
        {
            BattleCamera();
        }
        
    }

    void BattleCamera()
    {
        Vector3 target = battleAreas.battleTargetCameraPosition;
        target.y += 7.5f;
        target.z -= 7.65f;

        Vector3 targetPosition = target - transform.position;
        rb.AddForce(targetPosition.normalized * 900 * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target);
        rb.drag = 10 / distance * 1.6f;
    }

    void FreeRoamCamera()
    {

            Vector3 target = player.transform.position;
            target.y += 7.5f;
            target.z -= 7.65f;

        if(questPanel)
        {
            target.x -= 4;
        }

            Vector3 targetPosition = target - transform.position;
            rb.AddForce(targetPosition.normalized * 900 * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, target);
            rb.drag = 10 / distance * 1.6f;

    }
}










/*
 * public class CameraControl : MonoBehaviour
{                                                                       // script dedicated to controlling the player camera
    public GameObject player;
    Rigidbody rb;
    public GameObject targetAheadPlayer;
    Movement playerMoveScript;
    bool playerMoving;

    float defaultYheight = 30;                                          // camera position
    float defaultZdistance = -27.5f;

    float returnDelay = 1f;                                             // moving camera times
    float moveDelay = 0.5f;

    float returnTargetTime;                                             // calculating the time when to move the camera
    float moveTargetTime;

    bool returnInitiated = false;
    bool moveInitiated = false;

    public bool moveToRight = false;

    float distanceToRightOfPlayer = 7.5f;


    Vector3 zoomOutPosition = new Vector3(0, 0, 0);                     // target positions
    Vector3 zoomInPosition = new Vector3(0, -18.5f, 3f);
    Vector3 currentZoom = new Vector3(0, 0, 0);

    public GameObject objectToLookAt;
    ObjectToLookAtPhysics lookAtScript;

    public bool interacting = false;
    public bool panelOpen = false;

    void Start()
    {
        // initialise the start location of camera
        Vector3 playerCoordinate = player.transform.position;
        playerCoordinate.y += defaultYheight;
        playerCoordinate.z += defaultZdistance;
        playerCoordinate += currentZoom;

        transform.position = playerCoordinate;

        //initialise the component references
        rb = gameObject.GetComponent<Rigidbody>();
        playerMoveScript = player.GetComponent<Movement>();

        lookAtScript = objectToLookAt.GetComponent<ObjectToLookAtPhysics>();

    }

    private void FixedUpdate()
    {
    //    CameraZoom();                                            // rotation under update is very jittery for some reason  >  fixed updated sorts it out
   //     transform.eulerAngles = currentRotation;
    }

    void Update()
    {

        lookAtScript.ApplyNewPosition(transform.position);          // camera rotations
        transform.LookAt(objectToLookAt.transform);


        if(interacting || panelOpen)
        {
            currentZoom = zoomInPosition;
        }
        else
        {
            currentZoom = zoomOutPosition;
        }

        playerMoving = playerMoveScript.moving;                 // check player movement status

        if(interacting)
        {
            InteractionPhysics();
        }
        else if(moveToRight)
        {
            MoveCameraToRightOfPlayer();
        }
        else if (playerMoving)                                    // when moving... 
        {
            if (!moveInitiated)                                  // if move camera ahead not initialised  >>  initialise (calculate the target time for transition)
            {
                moveInitiated = true;
                moveTargetTime = Time.time + moveDelay;
            }
            else if (Time.time > moveTargetTime)
            {
                MoveCameraAheadOfPlayer();
                returnInitiated = false;
            }
        }
        else                                                     // when not moving...
        {
            if (!returnInitiated)                                // if camera return not initialised  >>  initialise (calculate the target time for transition)
            {
                returnInitiated = true;
                returnTargetTime = Time.time + returnDelay;
            }
            else if (Time.time >= returnTargetTime)              // once target time has been met  >>  reset moveInitiated + execute return camera
            {
                moveInitiated = false;
                ReturnCameraToDefault();
            }
        }
    }

    void ReturnCameraToDefault()
    {
        // calculating the target location to move to (location of player)  +  distance
        Vector3 target = player.transform.position;
        target.y += defaultYheight;
        target.z += defaultZdistance;
        target += currentZoom;

        Vector3 targetPosition = target - transform.position;

        float distance = Vector3.Distance(transform.position, target);

        // apply the force
        rb.AddForce(targetPosition.normalized * 650 * Time.deltaTime);                 // add force to the camera towards the target location

        rb.drag = 10 / distance * 1.25f;                              // increase drag (resistance) to camera to slow it down
    }

    void MoveCameraAheadOfPlayer()
    {
        // calculating the target location to move to (location ahead of player)  +  distance
        Vector3 target = targetAheadPlayer.transform.position;
        target.y += defaultYheight;
        target.z += defaultZdistance;
        target += currentZoom;

        Vector3 targetPosition = target - transform.position;

        float distance = Vector3.Distance(transform.position, target);      // calculate the distance to move

        rb.AddForce(targetPosition.normalized * 2200 * Time.deltaTime);                        // add force to the camera towards the target location

        rb.drag = 35 / distance * 1.25f;                                     // increase drag (resistance) to camera to slow it down
    }

    void MoveCameraToRightOfPlayer()
    {
        // calculating the target location to move to (location of player)  +  distance
        Vector3 target = player.transform.position;
        target.y += defaultYheight;
        target.z += defaultZdistance;
        target.x += distanceToRightOfPlayer;
        target += currentZoom;

        Vector3 targetPosition = target - transform.position;

        float distance = Vector3.Distance(transform.position, target);

        // apply the force
        rb.AddForce(targetPosition.normalized * 1300 * Time.deltaTime);                 // add force to the camera towards the target location

        rb.drag = 35 / distance * 1.25f;
    }

    void InteractionPhysics()
    {
        // calculating the target location to move to (location of player)  +  distance
        Vector3 target = player.transform.position;
        target.y += defaultYheight;
        target.z += defaultZdistance;
        target += currentZoom;

        Vector3 targetPosition = target - transform.position;

        float distance = Vector3.Distance(transform.position, target);

        // apply the force
        rb.AddForce(targetPosition.normalized * 2200 * Time.deltaTime);                 // add force to the camera towards the target location

        rb.drag = 27.5f / distance * 0.75f;                                      // increase drag (resistance) to camera to slow it down
    }
}





*/