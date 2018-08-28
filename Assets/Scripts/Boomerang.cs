using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Boomerang : Weapon {

    public float angularSpeed = 720;

    public override Sprite GetSprite()
    {
        return transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
}
