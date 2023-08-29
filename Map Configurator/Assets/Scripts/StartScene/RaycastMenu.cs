using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;

public class RaycastMenu : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("UI"))
            {
                int targetScene = -1;
                UIText textScript = result.gameObject.GetComponent<UIText>();
                targetScene = textScript.RaycastText();

                if (Input.GetMouseButtonDown(0))
                {
                    if(targetScene == -1)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        SceneManager.LoadScene(targetScene);
                    }
                  //  GameManager.playerChoice = textScript.action;
                    
                }
            }
        }
    }
}

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneMaster : MonoBehaviour
{
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

/*
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
