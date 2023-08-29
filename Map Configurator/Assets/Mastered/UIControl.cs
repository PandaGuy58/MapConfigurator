
using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor (typeof (MapGenerator))]
public class UIControl : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        DrawDefaultInspector();
        
        if(GUILayout.Button ("Generate"))
        {
         //   mapGen.GenerateMap();
        }
    }
}





// Start is called before the first frame update
// void Start()
//  {

//  }

// Update is called once per frame

//    private void Update()
//   {
//   RaycastHit hit;
//  Ray ray = camera.ScreenPointToRay(Input.mousePosition);

//  if (Physics.Raycast(ray, out hit))
//   {
//      Transform objectHit = hit.transform;

// Do something with the object that was hit by the raycast.
//  }

//   }

//  public void GenerateMap()
//    {
//     hexMaster.GenerateMap();
//   }
