using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static Collision CalcSphereToPlaneCollision(SphereCollider sphereCollider, PlaneCollider planeCollider)
    {

        if((sphereCollider.GetComponent<Object>().velocity == Vector3.zero) || 
           (AngleBetweenVectors(sphereCollider.GetComponent<Object>().velocity, planeCollider.normal) == (90 * Mathf.PI/180))) 
        {
            return new Collision(false, sphereCollider.GetComponent<Object>(), planeCollider.GetComponent<Object>(), Vector3.zero);
        }

        Vector3 planeCenterOfMass = planeCollider.transform.position;
        Vector3 p = sphereCollider.transform.position - planeCenterOfMass;

        float q1 = AngleBetweenVectors(planeCollider.normal, p); //angle between the normal vector and vector between the center of mass point on the plane
        float q2 = (90 * Mathf.PI/180) - q1;

        float d = Mathf.Sin(q2) * SizeOfVector(p);  //the distance from the sphere to the plane parallel to the normal of the sphere

        float s = AngleBetweenVectors((planeCollider.normal * -1), sphereCollider.GetComponent<Object>().velocity);

        UnityEngine.Debug.Log("normal: " + (planeCollider.normal * -1));
        UnityEngine.Debug.Log("sphere velocity: " + sphereCollider.GetComponent<Object>().velocity);
        UnityEngine.Debug.Log("s: " + s);

        float nextMoveDistance = (d - sphereCollider.radius) / Mathf.Cos(s);




        //if (nextMoveDistance < SizeOfVector(sphereCollider.GetComponent<Object>().velocity * Time.fixedDeltaTime))
        //{
        //    Vector3 nextMove = NormalizeVector(sphereCollider.GetComponent<Object>().velocity) * nextMoveDistance;
        //    return nextMove;
        //}

        //return Vector3.zero;

        Vector3 nextMove;
        bool collided = false;

        if (nextMoveDistance < SizeOfVector(sphereCollider.GetComponent<Object>().velocity * Time.fixedDeltaTime))
        {
            nextMove = NormalizeVector(sphereCollider.GetComponent<Object>().velocity) * nextMoveDistance;
            collided = true;
        }
        else 
        {
            nextMove = Vector3.zero;
        }

        return new Collision(collided, sphereCollider.GetComponent<Object>(), planeCollider.GetComponent<Object>(), nextMove);
    }

    public static Collision CalcSphereToSphereCollision(SphereCollider a, SphereCollider b)
    {
        Vector3 nextMove = new Vector3();
        bool collided = false;

        if (!a.GetComponent<Object>().isStatic && !b.GetComponent<Object>().isStatic) 
        {
            //calc moving sphere
            nextMove = Vector3.zero;
        }
        else if (!a.GetComponent<Object>().isStatic && b.GetComponent<Object>().isStatic) 
        {
            nextMove = CalcSphereToStationarySphereCollision(a, b);
        }
        else if (a.GetComponent<Object>().isStatic && !b.GetComponent<Object>().isStatic) 
        {
            nextMove = CalcSphereToStationarySphereCollision(b, a);
        }
        else //both static
        {
            //no collision
            nextMove = Vector3.zero;
        }


        if (nextMove != Vector3.zero)
        {
            collided = true;
        }

        return new Collision(collided, a.GetComponent<Object>(), b.GetComponent<Object>(), nextMove);
    }

    static Vector3 CalcSphereToStationarySphereCollision(SphereCollider a, SphereCollider b) 
    {
        Vector3 A = b.transform.position - a.transform.position; // Vector between both centers
        Vector3 V = a.GetComponent<Object>().velocity; // Velocity of a
        float angleQ = AngleBetweenVectors(A, V);// angle between A and V
        float d = SizeOfVector(A) * Mathf.Sin(angleQ); // Distance between the centers of a and b 
        float e = Mathf.Sqrt(((a.radius + b.radius) * (a.radius + b.radius)) - (d * d)); //Amount to displace V back from the center of b
        float nextMoveDistance = Mathf.Cos(angleQ) * SizeOfVector(A) - e;
        Vector3 nextMove = (V / SizeOfVector(V)) * nextMoveDistance;

        if(nextMoveDistance < SizeOfVector(a.GetComponent<Object>().velocity * Time.fixedDeltaTime)) 
        {
            return nextMove;
        }

        return Vector3.zero;
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


}
