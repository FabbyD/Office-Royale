using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtMouse : MonoBehaviour {

    public float radius = 0.5f;
	
	// Update is called once per frame
	void Update () {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        transform.localPosition = direction * radius;
        transform.LookAt(mousePosition, Vector3.forward);
        var parentPosition = transform.parent.position;
        if (mousePosition.x >= parentPosition.x && transform.localScale.x < 0 ||
            mousePosition.x < parentPosition.x && transform.localScale.x >= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 1);
        }
    }
}
