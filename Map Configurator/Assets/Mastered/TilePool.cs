using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    public GameObject hexInstance;
    public List<HexInstance> availableHex = new List<HexInstance>();
    public List<HexInstance> allHex = new List<HexInstance>();

    public Transform poolTransform;

    public HexInstance requestHex()
    {
        if(availableHex.Count > 0)
        {
            HexInstance hex = availableHex[0];
            availableHex.RemoveAt(0);
            hex.gameObject.SetActive(true);
            return hex;
        }

        GameObject hexObject = Instantiate(hexInstance);
        allHex.Add(hexObject.GetComponent<HexInstance>());
        return allHex[allHex.Count - 1];
    }

    public void ResetPool()
    {
        availableHex = new(allHex);

        for(int i = 0; i < availableHex.Count; i++)
        {
            availableHex[i].transform.parent = poolTransform;
            availableHex[i].gameObject.SetActive(false);
        }
    }


}
