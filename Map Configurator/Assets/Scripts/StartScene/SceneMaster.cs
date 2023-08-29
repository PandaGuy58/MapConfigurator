using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneMaster : MonoBehaviour
{
    bool menuInitialised = false;
    bool cameraMoveInitialised = false;
    bool cameraRotateInitialised = false;

    Image blackPanel;

    Color initialColour = Color.black;
    Color targetColour = Color.black;

    float blackPanelTime = 0;
    float cameraMoveTime = 0;
    float cameraRotateTime = 0;

    public AnimationCurve panelCurve;
    public AnimationCurve moveCurve;
    public AnimationCurve rotateCurve;


    Vector3 targetRotation = new Vector3(75, 56, 0);
    Vector3 targetPosition = new Vector3(8.65f, 11.64f, 7.63f);

    Vector3 initialRotation = new Vector3(6.6f, 56.4f, 0);
    Vector3 initialPosition = new Vector3(0.14f, 12.56f, 1.89f);

    GameObject cameraObject;
    Title titleScript;

    GameObject playerLight;

    Camera cameraComp;
    private void Start()
    {
        blackPanel = GameObject.Find("Black Panel").GetComponent<Image>();
        targetColour.a = 0;
        blackPanel.color = initialColour;

        cameraObject = GameObject.Find("Main Camera");
        titleScript = GameObject.Find("Title").GetComponent<Title>();
        playerLight = GameObject.Find("Player Light");
        cameraComp = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        UpdateLight();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            menuInitialised = true;
        }


        float evaluation;
        if (menuInitialised && blackPanelTime < 1)
        {
            blackPanelTime += Time.deltaTime * 0.65f;

            evaluation = panelCurve.Evaluate(blackPanelTime);
            Color calculateCol = Color.Lerp(initialColour, targetColour, evaluation);
            blackPanel.color = calculateCol;

            cameraMoveInitialised = true;
            cameraRotateInitialised = true;
        }

        if(cameraMoveInitialised)
        {
            cameraMoveTime += Time.deltaTime * 0.3f;
            evaluation = moveCurve.Evaluate(cameraMoveTime);

            Vector3 calculateMove = Vector3.Lerp(initialPosition, targetPosition, evaluation);
            cameraObject.transform.position = calculateMove;
        }

        if(cameraRotateInitialised)
        {
            cameraRotateTime += Time.deltaTime * 0.3f;
            evaluation = rotateCurve.Evaluate(cameraRotateTime);

            Vector3 calculateRotate = Vector3.Lerp(initialRotation, targetRotation, evaluation);
            cameraObject.transform.localEulerAngles = calculateRotate;

            if(evaluation == 1)
            {
                titleScript.ScriptActivate();
            }

        }
    }

    public void UpdateLight()
    {

        Vector3 mouseCoordinate = Input.mousePosition;

        mouseCoordinate.z = 2.0f;
        Vector3 objectPos = cameraComp.ScreenToWorldPoint(mouseCoordinate);


        playerLight.transform.position = objectPos;

    }

}







    /*
    GraphicRaycaster raycaster;
    PointerEventData eventData;
    EventSystem eventSystem;

    public List<RaycastResult> results = new List<RaycastResult>();

    // Start is called before the first frame update
    void Start()
    {

        raycaster = GetComponent<GraphicRaycaster>();   
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        
    }

    // Update is called once per frame
    void Update()
    {
        eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;


        raycaster.Raycast(eventData, results);
        foreach(RaycastResult result in results)
        {
        ////    if(result.gameObject.CompareTag("UI"))
        //    {
                Debug.Log(result.gameObject.transform);

        }
    }
}


 * //Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GraphicRaycasterRaycasterExample : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
            }
        }
    }
}
*/