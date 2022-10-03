using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    List<Object> m_objects;
    Vector3 m_gravity = new Vector3(0, -9.81f, 0);

    // Start is called before the first frame update
    void Start()
    {
        m_objects = new List<Object>(FindObjectsOfType<Object>());
    }

    private void FixedUpdate()
    {
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

    void ResolveCollisions() 
    {

    }

    void UpdateObjectsPositions() 
    {
        foreach (Object obj in m_objects)
        {
            // v = u + at so...
            // v = u + (f/m)*fixedTimeStep
            Vector3 resultantForce = obj.force + (m_gravity * obj.mass);

            obj.velocity = obj.velocity + (resultantForce / obj.mass) * Time.fixedDeltaTime;
            obj.transform.position += obj.velocity * Time.fixedDeltaTime;
        }
    }
}
