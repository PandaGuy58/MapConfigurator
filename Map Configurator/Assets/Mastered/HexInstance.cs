using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInstance : MonoBehaviour
{
    public Vector2 indexLocation;
    public Vector2 coordinates;

    public bool selected;

    public Renderer meshRend;
    public int gCost;
    public int hCost;

    public bool walkable;
    public HexInstance parent;
    public float pointerTime;

    public float height;


    // forward triangle generation
    /*
    public Transform pointA;
    public Transform pointB;

    public Transform pointC;
    public Transform pointD;
    */

    public Transform spawnLocation;
    public bool alternativeBiome = false;
    public bool waterBiome = false;

    public Transform leftCentre;
    public Transform topLeft;
    public Transform topRight;
    public Transform rightCentre;
    public Transform bottomLeft;
    public Transform bottomRight;

    public GameObject testAsset;

    // public GameObject chest;
    //  Renderer rend;
    /*
    [SerializeField]
    public Transform top;

    [SerializeField]
    public Transform bottom;

    [SerializeField]
    public Transform topRight;

    [SerializeField]
    public Transform topLeft;

    [SerializeField]
    public Transform bottomRight;

    [SerializeField]
    public Transform bottomLeft;

    public bool visited = false;

    public float test;
    */

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float yVal = transform.position.y;
        Color targetCol;        // = Color.Lerp(Color.white, Color.black, yVal);
        /*
        if(test > Time.time - 0.1f)
        {
            targetCol = Color.Lerp(Color.cyan, Color.black, yVal);
        }
        */

        if(waterBiome)
        {
            targetCol = Color.Lerp(Color.black, Color.blue, yVal);
        }
        else if(alternativeBiome)
        {
            targetCol = Color.Lerp(Color.yellow, Color.black, yVal);
        }
        else
        {
            targetCol = Color.Lerp(Color.green, Color.black, yVal);
        }

        meshRend.material.color = targetCol;
    }

    public void InitialiseWater(float yValue)
    {
        waterBiome = true;
        yValue = yValue * 0.1f;
        Vector3 current = transform.position;
        current.y -= yValue;
        transform.position = current;
    }

    public void SpawnChest(GameObject chest)
    {
        Instantiate(chest, spawnLocation.position, Quaternion.identity);
    }

    public void SpawnAssets()
    {
        List<Vector3> allCoordinates = new List<Vector3>();

        float randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(leftCentre.position, topLeft.position, randomise));

        randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(topLeft.position, topRight.position, randomise));

        randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(topRight.position, rightCentre.position, randomise));

        randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(rightCentre.position, bottomRight.position, randomise));

        randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(bottomRight.position, bottomLeft.position, randomise));

        randomise = Random.Range(0.0f, 1.0f);
        allCoordinates.Add(Vector3.Lerp(bottomLeft.position, leftCentre.position, randomise));

        Vector3 finalValue = allCoordinates[0];
        for(int i = 1; i < allCoordinates.Count; i++)
        {
            finalValue += allCoordinates[i];
        }

        finalValue = finalValue / allCoordinates.Count;
        finalValue.y += 10;
        Instantiate(testAsset, finalValue, Quaternion.identity);





        //   Vector2 target = Vector2.Lerp(leftCentre, topLeft, Random.Range();

    }

    public HexInstance GetNeighbour()
    {
        HexInstance toReturn = null;
        /*
        HexInstance toReturn = null;
        List<Transform> locations = new List<Transform> { top, bottom, topLeft, topRight, bottomLeft, bottomRight };
        bool found = false;
        while(!found && locations.Count != 0)
        {
            Transform targetTransform = locations[Random.Range(0, locations.Count)];
            toReturn = SpawnSphereGetHexInstance(targetTransform);



            if(toReturn != null)
            {
                found = true;
            }
            else
            {
                locations.Remove(targetTransform);
            }
        }


        return toReturn;
        */
        return toReturn;
    }

    HexInstance SpawnSphereGetHexInstance(Transform targetLocation)
    {
        HexInstance toReturn = null;
        Collider[] hitColliders = Physics.OverlapSphere(targetLocation.position, 1);
//        Debug.Log(Time.time +" : " + hitColliders.Length);
        foreach(var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.CompareTag("HexCheck"))
            {
             //   Debug.Log(Time.time + " " + "found");
                HexCheck hexCheck = hitCollider.gameObject.GetComponent<HexCheck>();
                toReturn = hexCheck.RequestInstance();
            //    Debug.Log(Time.time + " " + toReturn.gameObject.name);
                break;
            }
        }
        return toReturn;
    }


    //  void Update

    //  public void Initialise(Color targetCol)
    //  {
    //      MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    //      meshRenderer.material.color = targetCol;
    //   meshRend = GetComponent<MeshRenderer>();
    //      Debug.Log(targetCol);
    //    meshRend.material.color = Color.black;
    // Material targetMat = meshRend.material;
    //targetMat.color = Color.green;

    //meshRend.material.color = Color.green;
    // meshRend.material.SetColor("_Color", targetCol); //color = targetCol;
    //  }
    // Update is called once per frame
    //  void Update()
    //  {
    // update colour 


    //     Color targetColor = Color.white;
    //   if(pointerTime > Time.time)
    //    {
    //    targetColor = Color.yellow;
    //    }

    //  if (selected)
    //   {
    //  meshRend.material.color = targetColor;
    // }
    //  else
    //   {
    //  meshRend.material.color = targetColor;
    //

    //   rend.material.SetFloat("_Visibility", curve.Evaluate(visibilityVal) * targetVisibility);
    //   meshRend.material.SetColor("_CurrentColour", targetColor);
    //  currentColour = Color.Lerp(currentColour, targetColour, 2 * Time.deltaTime);

    //   }
}
        /*
        if (selected)
        {
            meshRend.material.color = Color.red;
        }
        else
        {
            meshRend.material.color = Color.white;
        }
    }
}
        */