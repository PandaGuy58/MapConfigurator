using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIRaycast : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    Vector2 recentMouse;
    public List<GrabPanel> targetGrabPanels;
    public List<GrabPanel> currentRaycastGrabPanels;

  //  Vector2 mouseDifference;

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        Vector2 currentMouse = Input.mousePosition;
        Vector2 mouseDifference = currentMouse - recentMouse;

        for(int i = 0; i < targetGrabPanels.Count; i++)
        {
            GrabPanel grabPanel = targetGrabPanels[i];

            if(currentRaycastGrabPanels.Contains(grabPanel))
            {
                grabPanel.RaycastGrabPanel(true);

                if(Input.GetMouseButton(0))
                {
                    grabPanel.ClickGrabPanel(mouseDifference);
                }

                if(Input.GetMouseButtonDown(0))
                {
                    grabPanel.MouseDownPanel();
                }
            }
            else
            {
                grabPanel.RaycastGrabPanel(false);
            }

        //    if(Input.GetMouseButtonDown(0))
         //   {
         //       grabPanel.MouseDownPanel();
        //    }
        //    else if(Input.GetMouseButton(0))
        //    {
        //        grabPanel.ClickGrabPanel(mouseDifference);
       //     }

        }

        /*
        if (grabPanel != null)
        {
            grabPanel.RaycastGrabPanel();

            if(Input.GetMouseButtonDown(0))
            {
                grabPanel.MouseDownPanel();
            }
            else if(Input.GetMouseButton(0))
            {
                grabPanel.ClickGrabPanel(mouseDifference);
            }
        }

        */

        
        if(currentMouse != recentMouse)
        {
            recentMouse = currentMouse;
            currentRaycastGrabPanels = new List<GrabPanel>();

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = currentMouse;
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointerEventData, results);
            foreach(RaycastResult result in results)
            {
                if(result.gameObject.CompareTag("UI"))
                {
                    currentRaycastGrabPanels.Add(result.gameObject.GetComponent<GrabPanel>());
                }
            }
        }
    }
}









//   pointerEventData = new PointerEventData(eventSystem);
//   List<RaycastResult> results = new List<RaycastResult>();

//  foreach (RaycastResult result in results)
//  {
//       Debug.Log(result.gameObject.name);
//   }


/*
public GraphicRaycaster raycaster;
public PointerEventData eventData;
public EventSystem eventSystem;

Vector2 recentMouseCoordinate;

public GrabPanel grabPanel;

void Start()
{
    raycaster = GetComponent<GraphicRaycaster>();
}

// Update is called once per frame
void Update()
{
    Vector2 currentMouseCoordinate = Input.mousePosition;
    if(currentMouseCoordinate != recentMouseCoordinate)
    {
    //    grabPanel = null;
        recentMouseCoordinate = currentMouseCoordinate;

        eventData = new PointerEventData(eventSystem);
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        Debug.Log(Time.time + " " + results.Count);

    //    foreach(RaycastResult result in results)
        //{
          //  Debug.Log(Time.time);
           // if(result.gameObject.CompareTag("UI"))
          //  {
           //     grabPanel = result.gameObject.GetComponent<GrabPanel>();
           // }
      //  }

    }
}
}
*/