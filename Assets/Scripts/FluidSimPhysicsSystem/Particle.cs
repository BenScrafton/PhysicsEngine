using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 previousPos;
    public List<Particle> particleConnections = new List<Particle>();
    public List<Particle> nearParticleConnections = new List<Particle>();
    public float density;
    public float nearDensity;



    public float slipFactor;
    public float stickDistance;
    public float stickinessConstant;


    Vector3 normal;
    Vector3 relativeNormal;
    Vector3 relativeTangent;
    Vector3 impulse;
    Vector3 contactPoint;
    Vector3 otherColliderVelocity;

    float distanceFromCollisionSurface;
    ContactPoint cp;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {


        print("COLLIDE");

        otherColliderVelocity = Vector3.zero;//collision.collider.GetComponent<Rigidbody>().velocity;
                                             //    Vector3 particleRelativeVelocity = velocity; //- otherColliderVelocity;
                                             //print("Collision " );

        cp = collision.GetContact(0);

        normal = cp.normal;

        relativeNormal = Vector3.Dot(velocity, normal) * normal;
        relativeTangent = velocity - relativeNormal;

        impulse = relativeNormal - (slipFactor * relativeTangent);

        distance = Vector3.Distance(cp.point, transform.position);

        transform.position += (normal * (0.5f - distance)) + (normal * 0.01f);

        //Collision Impulse
        velocity -= impulse;

        distanceFromCollisionSurface = distance;

        velocity += -1.0f * Time.fixedDeltaTime * stickinessConstant * distanceFromCollisionSurface * (1.0f - (distanceFromCollisionSurface / stickDistance)) * normal;

    }
}
