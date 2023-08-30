using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabPanel : MonoBehaviour
{
    bool currentlyRaycast;
    float timeClickPanel;
    Image img;

    public RectTransform targetRTransform;

    public float timeClicked;

    bool hidden = false;

    public List<GameObject> gameObjects = new List<GameObject>();
    public Image background;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if(currentlyRaycast)
        {
            img.color = Color.white;
        }
        else
        {
            img.color = Color.black;
        }
    }
        /*
        if(timeClickPanel > Time.time)
        {
            img.color = Color.white;
        }
        else if(timeRaycastPanel > Time.time)
        {
            img.color = Color.black;
        }
        else
        {
            img.color = Color.yellow;
        }
    }
        */

    public void RaycastGrabPanel(bool raycastTrue)
    {
        currentlyRaycast = raycastTrue;
    }

    public void ClickGrabPanel(Vector2 mouseDifference)
    {
        timeClickPanel = Time.time;

        Vector2 currentPosition = targetRTransform.position;
        currentPosition += mouseDifference;
        targetRTransform.position = currentPosition;    
    }

    public void MouseDownPanel()
    {
      //  Debug.Log(Time.time);
        if(timeClicked + 0.5f > Time.time)
        {
            hidden = !hidden;
            timeClicked = -1;
            HidePanel();
        }
        else
        {
            timeClicked = Time.time;
        }

        
    }

    void HidePanel()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(hidden);
        }

        Color targetCol = background.color;
        if (hidden)
        {
            targetCol.a = 0.5f;
        }
        else
        {
            targetCol.a = 0;
        }
        background.color = targetCol;
    }
}
