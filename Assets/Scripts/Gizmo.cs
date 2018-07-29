﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour {

    public float radius;
    public Color color = Color.yellow;
    
	void OnDrawGizmos ()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
	}
}
