using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsFall : MonoBehaviour
{
    public Transform sphereSpawnLocation;

    void Start()
    {
        bool taskComplete = false;
        while (!taskComplete)
        {

            Vector3 current = transform.position;
            current.y -= 0.05f;
            transform.position = current;

            Collider[] hitColliders;

            hitColliders = Physics.OverlapSphere(sphereSpawnLocation.position, 0.05f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Ground"))
                {
                    taskComplete = true;
                }
            }
        }
    }
}

    /*
    public void EnableDuringGame()
    {
        bool taskComplete = false;
        while (!taskComplete)
        {

            Vector3 current = transform.position;
            current.y -= 0.05f;
            transform.position = current;

            Collider[] hitColliders;

            hitColliders = Physics.OverlapSphere(secondSphereSpawn.position, 0.05f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Ground"))
                {
                    taskComplete = true;
                }
            }
        }
    }
}
    */

                /*
                if (assignToHex && hitCollider.gameObject.CompareTag("Tile"))
                {
                    assignToHex = false;
                    HexTile tile = hitCollider.gameObject.GetComponent<HexTile>();
                    tile.objectAtLocation = gameObject;
                }
            }
        }
    }
}
                */
        /*
        if(rotateObject)
        {
            Vector3 calculateRotate = Vector3.zero;
            float randomRotation = Random.Range(0, 360);
            calculateRotate.y = randomRotation;
            transform.localEulerAngles = calculateRotate;
        }
    }
}

        */

 //   public void ChangeSphereSpawnLocation(float yChange)
 //   {
 //       Vector3 targetPosition = sphereSpawnLocation.position;
 //       targetPosition.y += yChange;
  //      sphereSpawnLocation.transform.position = targetPosition;
 //   }

 //   public void MultipleSizeY(float sizeMultiplier)
   // {
 //      Vector3 currentSize = transform.localScale;
 //       currentSize.y = currentSize.y * sizeMultiplier;
//    }

    //  private void Update()
    //   {

    //  }

  //  public void UpdateObject()
 //   {
   //     Debug.Log(gameObject.name);
   //     if (active)
   //     {
       //     Debug.Log(gameObject.name);
      //      rb.AddForce(new Vector3(0, -1000000, 0));
       //     if (Time.time > 1)
       //     {
            //    Debug.Log(gameObject.name);
    //            active = false;
             //   Debug.Log("yolo");
        //    }
    //   }

 //   }

 //   private void OnCollisionEnter(Collision collision)
  //  {
  //      if(collision.gameObject.CompareTag("Ground"))
   //     {
    //        loopEnd = true;
   //     }
     //   Debug.Log()
  //  }

    /*

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Tile"))
        {
            Debug.Log("tile");
            HexTile tile = other.gameObject.GetComponent<HexTile>();
        }
    }
}
    */
    /*

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("yolo");
            active = false;
        }
    }
}
    */
    // Update is called once per frame
    /*
    private void On(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Ground"))
        {
            active = false;
            Debug.Log("yolo");
        }
    }

}
    */