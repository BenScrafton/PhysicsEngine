using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionCalculator
{
    public static float DotProduct(Vector3 a, Vector3 b) 
    {
        float dotProduct = (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        return dotProduct;
    }

    public static float SizeOfVector(Vector3 v) 
    {
        v = mathf.Sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
        return v;
    }

    public static float AngleBetweenVectors(Vector3 a, Vector3 b) 
    {
        float angle = mathf.Acos(DotProduct(a, b) / (SizeOfVector(a) * SizeOfVector(b)));
        return angle;
    }


    public static CollisionPoints CalcSphereToSphereCollisionPoints(SphereCollider s1, SphereCollider s2)
    {
        Vector3 A = s2 - s1; //vector from s1 to s2
        Vector3 V = s1.GetComponent<Object>().velocity * Time.fixedDeltaTime;//Travel vector in next update
        float angleVA = AngleBetweenVectors(V, A);
        float e = mathf.cos(q) * SizeOfVector(A);


        CollisionPoints c = new CollisionPoints();
        return c;
    }

    //public static CollisionPoints CalcSphereToBoxCollisionPoints()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}

    //public static CollisionPoints CalcPlaneToBoxCollisionPoints()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}

    //public static CollisionPoints CalcBoxToBoxCollisionPoints()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}

    public static CollisionPoints CalcSphereToPlaneCollisionPoints(SphereCollider sphereCollider, PlaneCollider planeCollider)
    {
        Vector3 k = planeCollider.transform.position;
        Vector3 P = sphereCollider.tranform.position - k;

        Vector3 sphereTok = k - sphereCollider.transform.position;
        float sphereTokSize = mathf.Sqrt((sphereTok.x * sphereTok.x) + (sphereTok.y * sphereTok.y) + (sphereTok.z * sphereTok.z));
        float d = mathf.Acos()
            //angle = cos(d/V)
            //acos() * V = d
        float q2 = mathf.Asin(d / P);

        CollisionPoints c = new CollisionPoints();
        return c;
    }
}
