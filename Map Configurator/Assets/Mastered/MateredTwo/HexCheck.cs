using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCheck : MonoBehaviour
{
    public HexInstance targetInstance;
    // Start is called before the first frame update


    public HexInstance RequestInstance()
    {
        return targetInstance;
    }
}
