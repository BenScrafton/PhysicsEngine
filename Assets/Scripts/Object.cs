using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float mass;
    public Vector3 velocity;
    public Vector3 force;

    //Collider

    private void Start()
    {
        AddForce(new Vector3(0, 1000, 0));
    }

    void AddForce(Vector3 f) 
    {
        force = f;
    }
}
