using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static GameObject worldContainer;

    private void Awake()
    {
        worldContainer = gameObject;
    }
}
