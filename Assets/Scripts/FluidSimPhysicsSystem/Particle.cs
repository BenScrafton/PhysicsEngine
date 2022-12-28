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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        Vector3 otherColliderVelocity = Vector3.zero;//collision.collider.GetComponent<Rigidbody>().velocity;
        Vector3 particleRelativeVelocity = velocity; //- otherColliderVelocity;

        Vector3 normal = collision.GetContact(0).normal;

        Vector3 relativeNormal = Vector3.Dot(particleRelativeVelocity, normal) * normal;
        Vector3 relativeTangent = particleRelativeVelocity - relativeNormal;

        Vector3 impulse = relativeNormal - (slipFactor * relativeTangent);


        Vector3 contactPoint = collision.GetContact(0).point;
        transform.position += (normal * (0.5f - Vector3.Distance(contactPoint, transform.position))) + (normal * 0.01f);

        //Collision Impulse
        velocity -= impulse;

        float distanceFromCollisionSurface = Vector3.Distance(contactPoint, transform.position);
        Vector3 stickinessImpulse = -1 * Time.fixedDeltaTime * stickinessConstant * distanceFromCollisionSurface * (1 - (distanceFromCollisionSurface / stickDistance)) * normal;

        velocity += stickinessImpulse;
    }
}
