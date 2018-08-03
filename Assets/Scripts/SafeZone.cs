using UnityEngine;
using UnityEngine.Networking;
using System;

public class SafeZone : NetworkBehaviour {

    public float rescaleSpeed = 0.5f;

    public float maxMoveSpeed = 1;

    [SyncVar]
    public float targetScale = 2.3f;

    [SyncVar]
    public Vector2 targetPosition = Vector2.zero;

    [SyncVar]
    public double rescaleStart = 0;

    public GameObject NextZone;

    public bool nextZoneIsActive = false;

    void FixedUpdate () {
        if (IsRescaling())
        {
            Rescale();
        }
        if (IsMoving())
        {
            Move();
        }
        PlaceNextZone();
	}

    private bool IsRescaling()
    {
        float scale = transform.localScale.x;
        return scale != targetScale && rescaleStart <= NetworkClock.Time;
    }

    private bool IsMoving()
    {
        return (Vector2)transform.position != targetPosition && rescaleStart <= NetworkClock.Time;
    }

    private void Rescale()
    {
        transform.localScale = Vector2.MoveTowards(
            transform.localScale,
            new Vector2(targetScale, targetScale),
            rescaleSpeed * Time.deltaTime);
    }

    private void Move()
    {
        // The move animation should take as long as the shrink animation unless only the target position changed
        float moveSpeed = maxMoveSpeed;
        float deltaScale = transform.localScale.x - targetScale;
        if (deltaScale > 0)
        {
            float time = deltaScale / rescaleSpeed;
            float distance = (targetPosition - (Vector2)transform.position).magnitude;
            moveSpeed = distance / time;
        }
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);
    }

    private void PlaceNextZone()
    {
        NextZone.SetActive(nextZoneIsActive);
        NextZone.transform.position = targetPosition;
        NextZone.transform.localScale = new Vector2(targetScale, targetScale);
    }
}
