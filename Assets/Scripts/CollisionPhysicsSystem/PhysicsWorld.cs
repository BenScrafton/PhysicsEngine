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

        //if (collision.objectA.isStatic) 
        //{
        //    print("collideA");
        //    collision.objectB.transform.position += collision.nextMove;
        //    collision.objectB.velocity = Vector3.zero;
        //}
        //else if (collision.objectB.isStatic) 
        //{
        //    print("collideB");
        //    collision.objectA.transform.position += collision.nextMove;
        //    collision.objectA.velocity = Vector3.zero;
        //}

        collision.objectA.transform.position += collision.nextMoveA;
        //collision.objectA.velocity = Vector3.zero;

        collision.objectB.transform.position += collision.nextMoveB;
        //collision.objectB.velocity = Vector3.zero;

        ReactionSolver.Solve(collision);
    }

    void ResolveCollisions() 
    {
        foreach (Object a in m_objects)
        {
            if (a.isStatic) 
            {
                continue;
            }

            foreach (Object b in m_objects)
            {
                if (a != b) // Check to make sure they are not the same
                {
                    if (a.TryGetComponent(out Collider a_collider) && b.TryGetComponent(out Collider b_collider)) // Check to see if both have colliders 
                    {
                        Collision collision = a.GetComponent<Collider>().TestCollision(b.GetComponent<Collider>());
                        if (collision.collided)
                        {
                            SolveCollision(collision);
                        }
                    }
                }
            }
        }

        //foreach(Collision collision in m_collisions) 
        //{
        //    UnityEngine.Debug.Log("solve1");

        //    SolveCollision(collision);
        //}
    }

    void UpdateObjectsPositions() 
    {
        foreach (Object obj in m_objects)
        {
            if (!obj.isStatic && obj.velocity != Vector3.zero) 
            {
                // v = u + at so...
                // v = u + (f/m) * fixedTimeStep

                Vector3 resultantForce = obj.force + (m_gravity * obj.mass);

                obj.velocity = obj.velocity + (resultantForce / obj.mass) * Time.fixedDeltaTime;
                obj.transform.position += obj.velocity * Time.fixedDeltaTime;
            }
        }
    }
}
