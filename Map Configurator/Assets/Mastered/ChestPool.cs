using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPool : MonoBehaviour
{
    public GameObject chestObject;

    public List<GameObject> availableChests = new List<GameObject>();
    public List<GameObject> allChests = new List<GameObject>();

    public Transform chestPoolTransform;
    public Transform allChestsTransform;

    public GameObject RequestChest()
    {
        GameObject chest = null;

        if(availableChests.Count > 0)
        {
            chest = availableChests[0];
            availableChests.RemoveAt(0);
        }
        else
        {
            chest = Instantiate(chestObject);
            allChests.Add(chest);
        }

        chest.SetActive(true);
        chest.transform.parent = allChestsTransform;

        return chest;
    }
        /*
        GameObject newChest;

        if (availableChests.Count == 0)
        {
            newChest = Instantiate(chestObject);
            allChests.Add(newChest);
            newChest.transform.parent = allChestsTransform;
            return newChest;
        }

        newChest = availableChests[0];
        newChest.transform.parent = allChestsTransform;
        availableChests.RemoveAt(0);
        return newChest;

    }
        */

    public void ResetPool()
    {
        availableChests = new(allChests); 

        for(int i = 0; i < availableChests.Count; i++)
        {
            availableChests[i].SetActive(false);
            availableChests[i].transform.parent = chestPoolTransform;
        }
    }
}
        /*
       // Debug.Log("reset" + Time.time);
        for(int i = 0; i < allChests.Count; i++)
        {
         //   Debug.Log(Time.time + " " + i);
            allChests[i].SetActive(false);
            allChests[i].transform.parent = chestPoolTransform;
        }

        availableChests = new(allChests);
    }
}
        */
        /*
        availableChests = new(allChests);
        for(int i = 0; i < availableChests.Count; i++)
        {
            Debug.Log(Time.time);
            availableChests[i].SetActive(false);
        }
    }

}
        */


    /*
    public GameObject RequestChest()
    {
        
        if(availableChests.Count > 0)
        {
            GameObject targetChest = availableChests[0];
            availableChests.RemoveAt(0);
            targetChest.SetActive(true);
            targetChest.transform.parent = allChestsTransform;

            return targetChest; 
        }

        GameObject instance = Instantiate(chestObject);
        allChests.Add(instance);
        instance.SetActive(true);
        instance.transform.parent = allChestsTransform;

        return instance;
    }

    public void ResetPool()
    {
        for(int i = 0; i < allChests.Count; i++)
        {
            allChests[i].SetActive(false);
            allChests[i].transform.parent = chestPoolTransform;
        }

        availableChests = allChests;
    }
}
    */




// Start is called before the first frame update
//    void Start()
//   {

//   }

// Update is called once per frame
//  void Update()
//  {

//  }

