using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField]
    public bool multipleVariants;

    public GameObject objectPrefab;             // if multiple variants == false
    public List<GameObject> objectPrefabVariants;

    public List<GameObject> availableObjects;
    public List<GameObject> allObjects;

    public Transform targetParent;

    public GameObject RequestObject()
    {
        GameObject instance;     
        
        if (availableObjects.Count == 0)
        {
            if(multipleVariants)
            {
                instance = Instantiate(objectPrefabVariants[Random.Range(0, objectPrefabVariants.Count)]);
            }
            else
            {
                instance = Instantiate(objectPrefab);
            }

            instance.transform.parent = targetParent;

            allObjects.Add(instance);
        }
        else
        {
            instance = availableObjects[0];
            availableObjects.RemoveAt(0);
        }

        instance.SetActive(true);   
        return instance;    
    }

    public void ResetPool()
    {
        availableObjects = new(allObjects);

        for(int i = 0; i < availableObjects.Count; i++)
        {
            availableObjects[i].SetActive(false);
        }
    }

}
