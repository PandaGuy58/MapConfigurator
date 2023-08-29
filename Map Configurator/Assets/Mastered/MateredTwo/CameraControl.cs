using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform lookTarget;

    [SerializeField]
    private Transform cameraLook;

    int tilesNumber;

    int defaultHeight = 0;
    float distancePerTile = 0.5f;

    [SerializeField]
    private float startScrollValue = 1;

    [SerializeField]
    private float positionScrollSpeed = 0.5f;

    [SerializeField]
    private float positionJumpScale = 0.25f;

    [SerializeField]
    private float startRotationValue = 1;

    [SerializeField]
    private float rotationScrollSpeed = 1;

    [SerializeField]
    private float rotationJumpScale = 1;

    Vector3 targetPosition;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        cameraLook.LookAt(lookTarget);                                              // rotation

        Vector3 targetRotation = cameraLook.eulerAngles;
        float x = Mathf.PerlinNoise(startRotationValue + Time.time * rotationScrollSpeed, 1 + Time.time * rotationScrollSpeed) - 0.5f;
        float y = Mathf.PerlinNoise(startRotationValue + Time.time * rotationScrollSpeed, 1 + Time.time * rotationScrollSpeed) - 0.5f;
        float z = Mathf.PerlinNoise(startRotationValue + 4 + Time.time * rotationScrollSpeed, 5 + Time.time * rotationScrollSpeed) - 0.5f;

        targetRotation.x += x * rotationJumpScale;
        targetRotation.y += y * rotationJumpScale;
     //   targetRotation.z += z * rotationJumpScale;

        Vector3 currentRotation = transform.eulerAngles;
        Vector3 calculate = new Vector3();

        calculate.x = Mathf.LerpAngle(currentRotation.x, targetRotation.x, Time.deltaTime);
        calculate.y = Mathf.LerpAngle(currentRotation.y, targetRotation.y, Time.deltaTime);
        calculate.z = Mathf.LerpAngle(currentRotation.z, targetRotation.z, Time.deltaTime);

        transform.eulerAngles = calculate;


        // ----------------         ----------------------------      ------------------------
                                                                        // position
        Vector3 currentPosition = transform.position;
   //     Vector3 targetPosition = currentPosition;
  //      targetPosition.y = defaultHeight + (tilesNumber * distancePerTile);

        x = Mathf.PerlinNoise(startScrollValue + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        y = Mathf.PerlinNoise(startScrollValue + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        z = Mathf.PerlinNoise(startScrollValue + 4 + Time.time * positionScrollSpeed, 5 + Time.time * positionScrollSpeed) - 0.5f;

        Vector3 calculateTargetPosition = targetPosition;
        calculateTargetPosition.x += x * positionJumpScale;
        calculateTargetPosition.y += y * positionJumpScale;
        calculateTargetPosition.z += z * positionJumpScale;

        currentPosition = Vector3.Lerp(currentPosition, calculateTargetPosition, Time.deltaTime);
        transform.position = currentPosition;

        /*
         * *
         *         float x = Mathf.PerlinNoise(startScrollValue + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        float y = Mathf.PerlinNoise(startScrollValue + Time.time * positionScrollSpeed, 1 + Time.time * positionScrollSpeed) - 0.5f;
        float z = Mathf.PerlinNoise(startScrollValue + 4 + Time.time * positionScrollSpeed, 5 + Time.time * positionScrollSpeed) - 0.5f;

        Vector3 calculatePostion = initialPositionValue;
        calculatePostion.x += x * positionJumpScale;
        calculatePostion.y += y * positionJumpScale;
        calculatePostion.z += z * positionJumpScale;


        lightObject.transform.localPosition = calculatePostion;
        */
    }


    public void CalculateCameraTarget(List<GameObject> allTilesList)
    {
   //     Debug.Log(Time.time +" " + allTilesList.Count);
        Vector3 targetLocation = Vector3.zero;
        for (int i = 0; i < allTilesList.Count; i++)
        {
            targetLocation = targetLocation + allTilesList[i].transform.position;
        }

        targetLocation = targetLocation / allTilesList.Count;
        targetLocation.y += 5;

        lookTarget.position = targetLocation;

        targetPosition = startPosition;
        targetPosition.y = allTilesList.Count * distancePerTile;

     //   Debug.Log(targetLocation + " " + Time.time);
     //   camControl.tilesNumber = allTilesList.Count;

        //   return allTilesList.Count;
    }


}
