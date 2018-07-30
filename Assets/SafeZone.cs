using UnityEngine;
using UnityEngine.Networking;

public class SafeZone : NetworkBehaviour {

    public float shrinkSpeed = 0.2f;

    public float moveSpeed = 1;

    public float targetSize = 3;

    public Vector2 targetPosition;

    private SpriteMask spriteMask;

	// Use this for initialization
	void Start () {
        spriteMask = GetComponent<SpriteMask>();
        targetPosition = new Vector2(-12,-18);
	}
	
	void FixedUpdate () {
        if (!isServer)
        {
            return;
        }

        shrink();
        move();
	}

    private void shrink()
    {
        transform.localScale = Vector2.MoveTowards(
            transform.localScale,
            new Vector2(targetSize, targetSize),
            shrinkSpeed * Time.deltaTime);
    }

    private void move()
    {

        // The move animation should take as long as the shrink animation unless only the target position changed
        float moveSpeed = this.moveSpeed;
        float deltaSize = transform.localScale.x - targetSize;
        if (deltaSize > 0)
        {
            float time = deltaSize / shrinkSpeed;
            float distance = (targetPosition - (Vector2)transform.position).magnitude;
            moveSpeed = distance / time;
        }
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);
    }
}
