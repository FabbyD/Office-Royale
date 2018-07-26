using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PhysicsObject : NetworkBehaviour
{
    
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public Vector2 Velocity
    {
        get { return velocity; }
    }

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        AdditionalStart();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        velocity = Vector2.zero;
        AdditionalUpdate();
    }

    protected virtual void AdditionalStart()
    {

    }

    protected virtual void AdditionalUpdate()
    {

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Vector2 move = velocity * Time.deltaTime;

        Movement(move);
    }

    void Movement(Vector2 move)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

}
