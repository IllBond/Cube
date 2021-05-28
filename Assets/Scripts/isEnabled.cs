using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isEnabled : MonoBehaviour
{
    public int needToUnlock;
    public Material invisibleMaterial;

    void Start()
    {
        if (PlayerPrefs.GetInt("score") < needToUnlock) {
            GetComponent<MeshRenderer>().material = invisibleMaterial;
        }
    }

   
}
