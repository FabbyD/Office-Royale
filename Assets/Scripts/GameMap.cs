using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMap : MonoBehaviour {

    public GameObject gameMap;

    void Start()
    {
        gameMap.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Map"))
        {
            // Trigger the map
            gameMap.SetActive(!gameMap.activeSelf);
        }
    }
}
