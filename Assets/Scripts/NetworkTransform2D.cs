using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransform2D : NetworkBehaviour {

    public int updateRate = 9;
    public float maxSpeed = 3;
    public float minDistance = 0.001f;

    [SyncVar]
    private Vector2 targetPosition;

    private Rigidbody2D rb2d;
    private float updateInterval;
    private float timeSinceLastUpdate = 0;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        updateInterval = 1 / (float) updateRate;
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate > updateInterval)
            {
                timeSinceLastUpdate = 0;
                CmdSync(transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            Vector2 delta = targetPosition - rb2d.position;
            if (delta.magnitude > minDistance)
            {
                float maxDelta = maxSpeed * Time.deltaTime;
                rb2d.position = Vector2.MoveTowards(rb2d.position, targetPosition, maxDelta);
            }
        }
    }

    [Command]
    void CmdSync(Vector2 position)
    {
        targetPosition = position;
    }


}
