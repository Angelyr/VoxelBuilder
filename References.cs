using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static GameObject worldContainer;
    public static Player player;

    private void Awake()
    {
        worldContainer = gameObject;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
}
