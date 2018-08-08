using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

using Random = UnityEngine.Random;

public class SafeZone : NetworkBehaviour {

    // Speed to rescale to next zone
    public float rescaleSpeed = 0.2f;

    // Maximum speed to move to next zone
    public float maxMoveSpeed = 1;

    // How much the scale will be divided by each time
    public float scaleRatio = 2;

    // How long in seconds to stay in one size
    public float stayInterval = 60;

    // Next scale to shrink to
    [SyncVar]
    public float targetScale = 2.3f;

    // Next position to move to
    [SyncVar]
    public Vector2 targetPosition = Vector2.zero;

    // When to start rescaling and moving in seconds (unix timestamp server time)
    [SyncVar]
    public double rescaleStart = 0;

    private double rescaleEnd;

    // A reference to the next zone indication
    public GameObject nextZone;

    // A reference to the zone timer
    public Text wallTimer;

    // A reference to the zone timer logo animator
    public Animator wallTimerAnimator;

    // The current zone's sprite renderer
    private SpriteMask spriteMask;

    // Minimum scale
    private const float minScale = 0.01f;

    // Used to make sure the circle stays inside the game
    public SpriteRenderer office;

    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
    }
    
    void Update () {
        if (isServer && ShouldChoseNextZone())
        {
            ChoseNextZone();
        }

        if (IsRescaling())
        {
            Rescale();
        }
        if (IsMoving())
        {
            Move();
        }
        PlaceNextZone();
        UpdateUIOverlay();
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

    private bool ShouldChoseNextZone()
    {
        float scale = transform.localScale.x;

        if (scale <= 0)
        {
            return false;
        }

        Vector2 position = transform.position;
        return scale == targetScale && position == targetPosition && rescaleStart <= NetworkClock.Time;
    }

    private void ChoseNextZone()
    {
        // We want to make sure the next zone lies in the current zone.
        // To do this, consider the current and next zone z1 and z2 of
        // center ci and radius ri. For the next zone to lie inside the
        // current one, its center needs to be inside the circle of center
        // c1 and radius r1 - r2 where r2 is smaller than r1.

        // Chose next scale first
        float targetScale = this.targetScale / scaleRatio;
        Vector2 targetPosition = this.targetPosition;

        if (targetScale <= minScale)
        {
            // Stop scaling at a certain point
            targetScale = 0;
        } else
        {
            // Get current radius from sprite
            float currentRadius = spriteMask.bounds.size.x / 2;
            float nextRadius = currentRadius / scaleRatio;

            // Absolute just in case nextRadius ends up being bigger than current radius
            float radiusDiff = Mathf.Abs(currentRadius - nextRadius);

            // Get random center position
            Vector2 currentCenter = transform.position;
            float x = (Random.Range(-1.0f, 1.0f) * radiusDiff) + currentCenter.x;
            float y = (Random.Range(-1.0f, 1.0f) * radiusDiff) + currentCenter.y;

            // Clamp the position to not go away from the map
            Vector2 min = office.bounds.min;
            Vector2 max = office.bounds.max;
            x = Mathf.Clamp(x, min.x + nextRadius, max.x - nextRadius);
            y = Mathf.Clamp(y, min.y + nextRadius, max.y - nextRadius);

            targetPosition = new Vector2(x, y);
        }

        // Chose when to start shrinking
        rescaleStart = NetworkClock.Time + stayInterval;

        // Update rescale speed so every circle takes the same amount of time to reach their target
        rescaleSpeed = rescaleSpeed / scaleRatio;

        // Calculate how long it will take for the zone to reach the target (to update timer)
        float scaleDiff = ((Vector2) transform.localScale - new Vector2(targetScale, targetScale)).magnitude;
        float duration = scaleDiff / rescaleSpeed;
        rescaleEnd = rescaleStart + duration;

        // Apply new targets
        this.targetScale = targetScale;
        this.targetPosition = targetPosition;
    }

    private void PlaceNextZone()
    {
        nextZone.transform.position = targetPosition;
        nextZone.transform.localScale = new Vector2(targetScale, targetScale);
    }

    private void UpdateUIOverlay()
    {
        double now = NetworkClock.Time;
        bool isMoving = IsRescaling() || IsMoving();
        double end = isMoving ? rescaleEnd : rescaleStart;
        float timeRemaining = (float)Math.Max(0, end - now);
        int mins = Mathf.FloorToInt(timeRemaining / 60);
        int secs = Mathf.CeilToInt(timeRemaining % 60);
        wallTimer.text = String.Format("{0}:{1:00}", mins, secs);
        wallTimerAnimator.SetBool("Moving", isMoving);
    }
}
