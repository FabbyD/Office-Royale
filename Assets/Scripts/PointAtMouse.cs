using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtMouse : MonoBehaviour {

    public float radius = 0.5f;
	
	// Use LateUpdate to make adjustements after parent
	void LateUpdate () {
        var parentPosition = transform.parent.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)(mousePosition - parentPosition)).normalized;
        transform.position = (Vector2)parentPosition + direction * radius;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (mousePosition.x >= parentPosition.x && transform.lossyScale.y < 0 ||
            mousePosition.x < parentPosition.x && transform.lossyScale.y > 0)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1, -1, 1));
        }

        // Make sure global x scale is always positive
        if (transform.lossyScale.x < 0)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }
}
