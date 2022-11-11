using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ReactionSolver
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
        UnityEngine.Debug.Log("REACT");

        if (collision.collisionType == Collision.CollisionType.SPHERE_TO_SPHERE)
        {
            UnityEngine.Debug.Log("SPHERE REACT SOLVE");

            SphereToSphere(collision);
        }
        else
        {
            //collision.objectA.velocity = Vector3.zero;
            //collision.objectB.velocity = Vector3.zero;
        }
    }

    public static void SphereToSphere(Collision collision)
    {

        UnityEngine.Debug.Log("SPHERE REACT SOLVE2");
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

        UnityEngine.Debug.Log("momA1: " + momA1 + " : momA2: " + momA2);
        UnityEngine.Debug.Log("momB1: " + momB1 + " : momB2: " + momB2);

        //objA.velocity = (momA1) / objA.mass;
        //objB.velocity = (momB1) / objB.mass;

        //objA.velocity = momA1 / objA.mass;
        //objB.velocity = momB1 / objB.mass;

    }
}
