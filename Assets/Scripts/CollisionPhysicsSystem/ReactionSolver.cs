using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionSolver : MonoBehaviour
{
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        float dotProduct = (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        return dotProduct;
    }

    public static float SizeOfVector(Vector3 v)
    {
        float size = Mathf.Sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
        return size;
    }

    public static float AngleBetweenVectors(Vector3 a, Vector3 b)
    {
        float angle = Mathf.Acos(DotProduct(a, b) / (SizeOfVector(a) * SizeOfVector(b)));
        return angle;
    }

    public static Vector3 NormalizeVector(Vector3 vector)
    {
        Vector3 normalizedVector = vector / SizeOfVector(vector);
        return normalizedVector;
    }

    public static void Solve(Collision collision)
    {
        if (collision.collisionType == CollisionType.SPHERE_TO_SPHERE)
        {
            SphereToSphere(collision);
        }
        else if(collision.collisionType == CollisionType.SPHERE_TO_PLANE)
        {
            //collision.objectA.velocity = Vector3.zero;
            //collision.objectB.velocity = Vector3.zero;
            SphereToPlane(collision);
        }
    }

    public static void SphereToSphere(Collision collision)
    {
        Object objA = collision.objectA;
        Object objB = collision.objectB;

        objA.isStatic = false;
        objB.isStatic = false;

        // A on B
        Vector3 momA1 = Vector3.zero;
        Vector3 momB1 = Vector3.zero;

        if (objA.velocity != Vector3.zero)
        {
            Vector3 totalMom1 = objA.velocity * objA.mass;
            Vector3 collisionNormal = NormalizeVector(objB.transform.position - objA.transform.position);
            float angleQ = AngleBetweenVectors(collisionNormal, objA.velocity);

            momB1 = Mathf.Cos(angleQ) * SizeOfVector(totalMom1) * collisionNormal;
            momA1 = totalMom1 - momB1;
        }

        // B on A
        Vector3 momA2 = Vector3.zero;
        Vector3 momB2 = Vector3.zero;

        if (objB.velocity != Vector3.zero)
        {
            Vector3 totalMom2 = objB.velocity * objB.mass;
            Vector3 collisionNormal2 = NormalizeVector(objA.transform.position - objB.transform.position);
            float angleR = AngleBetweenVectors(collisionNormal2, objB.velocity);

            momA2 = Mathf.Cos(angleR) * SizeOfVector(totalMom2) * collisionNormal2;
            momB2 = totalMom2 - momA2;
        }

        objA.velocity = (momA1 + momA2) / objA.mass;
        objB.velocity = (momB1 + momB2) / objB.mass;

        //objA.velocity = momA1 / objA.mass;
        //objB.velocity = momB1 / objB.mass;
    }

    public static void SphereToPlane(Collision collision) 
    {
        Object sphere = collision.objectA;
        Object plane = collision.objectB;

        Vector3 normal = plane.GetComponent<PlaneCollider>().normal;
        float vMagnitude = SizeOfVector(sphere.velocity);

        Vector3 vBeforeUnit = sphere.velocity / vMagnitude;
        Vector3 vAfterUnit = (2 * normal * DotProduct(normal, vBeforeUnit * -1)) + vBeforeUnit;
        Vector3 vAfter = vAfterUnit * vMagnitude;

        sphere.velocity = vAfter;
        sphere.transform.position += vAfterUnit * 0.01f;

        Debug.Log("vAfter: " + vAfter);
    }
}

