using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    List<Object> m_objects;
    List<Collision> m_collisions = new List<Collision>();
    Vector3 m_gravity = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        m_objects = new List<Object>(FindObjectsOfType<Object>());

    }

    private void FixedUpdate()
    {
        m_collisions.Clear();

        UpdateObjectsPositions();
        ResolveCollisions();

        foreach (Object obj in m_objects)
        {
            obj.force = Vector3.zero;
        }
    }

    void AddObject() 
    {

    }

    void RemoveObject(Object obj)
    {

    }

    void SolveCollision(Collision collision) 
    {
        if (!collision.collided) 
        {
            return;
        }

        if (collision.objectA.isStatic) 
        {
            print("collideA");
            collision.objectB.transform.position += collision.nextMove;
            collision.objectB.velocity = Vector3.zero;
        }
        else if (collision.objectB.isStatic) 
        {
            print("collideB");
            collision.objectA.transform.position += collision.nextMove;
            collision.objectA.velocity = Vector3.zero;
        }
    }

    void ResolveCollisions() 
    {
        print("num objects: " + m_objects.Count);

        foreach (Object a in m_objects)
        {
            foreach (Object b in m_objects) 
            {
                if (a != b) // Check to make sure they are not the same
                {
                    if (a.TryGetComponent(out Collider a_collider) && b.TryGetComponent(out Collider b_collider)) // Check to see if both have colliders 
                    {
                        Collision collision = a.GetComponent<Collider>().TestCollision(b.GetComponent<Collider>());
                        if (collision.collided) 
                        {
                            m_collisions.Add(collision);
                        }

                    }
                }
            }
        }

        foreach(Collision collision in m_collisions) 
        {
            SolveCollision(collision);
        }
    }

    void UpdateObjectsPositions() 
    {
        foreach (Object obj in m_objects)
        {
            if (!obj.isStatic) 
            {
                // v = u + at so...
                // v = u + (f/m)*fixedTimeStep

                Vector3 resultantForce = obj.force + (m_gravity * obj.mass);

                obj.velocity = obj.velocity + (resultantForce / obj.mass) * Time.fixedDeltaTime;
                obj.transform.position += obj.velocity * Time.fixedDeltaTime;
            }
        }
    }
}
