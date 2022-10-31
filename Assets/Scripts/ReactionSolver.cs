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

        if (collision.collisionType == Collision.CollisionType.SPHERE_TO_STATIONARY_SPHERE)
        {
            UnityEngine.Debug.Log("SPHERE REACT SOLVE");

            SphereToStationarySphere(collision);
        }
        else
        {
            collision.objectA.velocity = Vector3.zero;
            collision.objectB.velocity = Vector3.zero;
        }
    }

    public static void SphereToStationarySphere(Collision collision)
    {
        Object a;
        Object b;

        if (collision.objectA.isStatic)
        {
            a = collision.objectB;
            b = collision.objectA;

            b.isStatic = false;
        }
        else if (collision.objectB.isStatic)
        {
            a = collision.objectA;
            b = collision.objectB;

            b.isStatic = false;
        }
        else 
        {
            return;
        }

        Vector3 v2 = NormalizeVector(b.transform.position - a.transform.position);
        Vector3 v1 = NormalizeVector(v2 * -1);

        float totalMomentumMagnitude = a.mass * SizeOfVector(a.velocity);
        Vector3 momentumA;
        Vector3 momentumB;

        momentumB = totalMomentumMagnitude * Mathf.Cos(AngleBetweenVectors(a.transform.position, v2)) * v2;
        momentumA = (totalMomentumMagnitude - SizeOfVector(momentumB)) * v1;

        UnityEngine.Debug.Log("MOMB: " + momentumB + " MOMA:" + momentumA);

        a.velocity = momentumA / a.mass;
        b.velocity = momentumB / b.mass;

        UnityEngine.Debug.Log("A: " + a.velocity + " B:" + b.velocity);
    }
}
