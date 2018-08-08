using UnityEngine;
using UnityEngine.Networking;

public class SafeZone : NetworkBehaviour {

    // Speed to rescale to next zone
    public float rescaleSpeed = 0.5f;

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

    // A reference to the next zone indication
    public GameObject nextZone;

    // Whether or not the safe zone is active
    public bool isActive = false;

    // The current zone's sprite renderer
    private SpriteMask spriteMask;

    // The next zone's sprite renderer
    private SpriteRenderer nextSpriteRenderer;

    // Minimum scale
    private const float minScale = 0.01f;

    // Position constraints
    public SpriteRenderer office;

    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        nextSpriteRenderer = nextZone.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate () {
        if (!isActive)
        {
            return;
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
	}

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (isServer && ShouldChoseNextZone())
        {
            ChoseNextZone();
        }
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
        targetScale = targetScale / scaleRatio;

        if (targetScale <= minScale)
        {
            // Stop scaling at a certain point
            targetScale = 0;
        } else
        {
            // Get current radius from sprite
            float currentRadius = spriteMask.bounds.size.x / 2;

            // The next zone is not rescaled yet, so we need to scale it ourselves
            float nextRadius = (nextSpriteRenderer.bounds.size.x / 2) / scaleRatio;

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
    }

    private void PlaceNextZone()
    {
        nextZone.SetActive(isActive);
        nextZone.transform.position = targetPosition;
        nextZone.transform.localScale = new Vector2(targetScale, targetScale);
    }
}
