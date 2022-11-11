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
        if ((sphereCollider.GetComponent<Object>().velocity == Vector3.zero) ||
           (AngleBetweenVectors(sphereCollider.GetComponent<Object>().velocity, planeCollider.normal) == (90 * Mathf.PI / 180)))
        {
            UnityEngine.Debug.Log("new Collision");

            return new Collision(Collision.CollisionType.SPHERE_TO_PLANE, false, sphereCollider.GetComponent<Object>(), planeCollider.GetComponent<Object>(), Vector3.zero, Vector3.zero);
        }

        Vector3 planeCenterOfMass = planeCollider.transform.position;
        Vector3 p = sphereCollider.transform.position - planeCenterOfMass;

        float q1 = AngleBetweenVectors(planeCollider.normal, p); //angle between the normal vector and vector between the center of mass point on the plane
        float q2 = (90 * Mathf.PI / 180) - q1;

        float d = Mathf.Sin(q2) * SizeOfVector(p);  //the distance from the sphere to the plane parallel to the normal of the sphere

        float s = AngleBetweenVectors((planeCollider.normal * -1), sphereCollider.GetComponent<Object>().velocity);

        UnityEngine.Debug.Log("normal: " + (planeCollider.normal * -1));
        UnityEngine.Debug.Log("sphere velocity: " + sphereCollider.GetComponent<Object>().velocity);
        UnityEngine.Debug.Log("s: " + s);

        float nextMoveDistance = (d - sphereCollider.radius) / Mathf.Cos(s);

        Vector3 nextMoveA; // sphere movement
        Vector3 nextMoveB = Vector3.zero; // plane movement
        bool collided = false;

        UnityEngine.Debug.Log("next move dist: " + nextMoveDistance);


        if (nextMoveDistance < SizeOfVector(sphereCollider.GetComponent<Object>().velocity * Time.fixedDeltaTime))
        {
            nextMoveA = NormalizeVector(sphereCollider.GetComponent<Object>().velocity) * nextMoveDistance;
            collided = true;
        }
        else
        {
            nextMoveA = Vector3.zero;
        }

        UnityEngine.Debug.Log("next move A: " + nextMoveA);

        UnityEngine.Debug.Log("new CollisionB");
        return new Collision(Collision.CollisionType.SPHERE_TO_PLANE, collided, sphereCollider.GetComponent<Object>(), planeCollider.GetComponent<Object>(), nextMoveA, nextMoveB);
    }

    public static Collision CalcSphereToSphereCollision(SphereCollider a, SphereCollider b)
    {
        Vector3 nextMoveA = Vector3.zero;
        Vector3 nextMoveB = Vector3.zero;
        bool collided = false;

        Collision.CollisionType type;

        if (!a.GetComponent<Object>().isStatic && !b.GetComponent<Object>().isStatic)
        {
            Collision c = CalculateSphereToMovingSphereCollision(a, b);
            nextMoveA = c.nextMoveA;
            nextMoveB = c.nextMoveB;
            UnityEngine.Debug.Log("nextMoveA: " + nextMoveA);

            type = Collision.CollisionType.SPHERE_TO_SPHERE;
        }
        else if (!a.GetComponent<Object>().isStatic && b.GetComponent<Object>().isStatic)
        {
            nextMoveA = CalcSphereToStationarySphereCollision(a, b);
            type = Collision.CollisionType.SPHERE_TO_SPHERE;
        }
        else if (a.GetComponent<Object>().isStatic && !b.GetComponent<Object>().isStatic)
        {
            nextMoveB = CalcSphereToStationarySphereCollision(b, a);
            type = Collision.CollisionType.SPHERE_TO_SPHERE;
        }
        else //both static
        {
            //no collision
            type = Collision.CollisionType.NONE;
        }

        if (nextMoveA != Vector3.zero || nextMoveB != Vector3.zero)
        {
            collided = true;
        }


        UnityEngine.Debug.Log("new CollisionC");


        return new Collision(type, collided, a.GetComponent<Object>(), b.GetComponent<Object>(), nextMoveA, nextMoveB);
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

        if (Mathf.Abs(nextMoveDistance) < Mathf.Abs(SizeOfVector(a.GetComponent<Object>().velocity * Time.fixedDeltaTime)))
        {
            return nextMove;
        }

        return Vector3.zero;
    }

    static Collision CalculateSphereToMovingSphereCollision(SphereCollider a, SphereCollider b)
    {
        Vector3 aPos = a.transform.position;
        Vector3 aVelocity = a.GetComponent<Object>().velocity;

        Vector3 bPos = b.transform.position;
        Vector3 bVelocity = b.GetComponent<Object>().velocity;

        float diffXpos = aPos.x - bPos.x;
        float diffYpos = aPos.y - bPos.y;
        float diffZpos = aPos.z - bPos.z;

        float diffXvelocity = aVelocity.x - bVelocity.x;
        float diffYvelocity = bVelocity.y - aVelocity.y;
        float diffZvelocity = bVelocity.z - aVelocity.z;

        //Quadratic equation solve
        float A = (diffXvelocity * diffXvelocity) + (diffYvelocity * diffYvelocity) + (diffZvelocity * diffZvelocity);
        float B = (2 * diffXpos * diffXvelocity) + (2 * diffYpos * diffYvelocity) + (2 * diffZpos * diffZvelocity);
        float C = (diffXpos * diffXpos) + (diffYpos * diffYpos) + (diffZpos * diffZpos) - ((a.radius + b.radius) * (a.radius + b.radius));


        if ((B * B) < (4 * A * C))
        {
            //no collision
            UnityEngine.Debug.Log("EXIT_1");
            UnityEngine.Debug.Log("new CollisionD");
            return new Collision(Collision.CollisionType.NONE, false, a.GetComponent<Object>(), b.GetComponent<Object>(), Vector3.zero, Vector3.zero);
        }

        float t1 = (-B + Mathf.Sqrt((B * B) - (4 * A * C))) / (2 * A);
        float t2 = (-B - Mathf.Sqrt((B * B) - (4 * A * C))) / (2 * A);

        float t;


        if ((2 * A) == 0)
        {
            UnityEngine.Debug.Log("new CollisionE");
            return new Collision(Collision.CollisionType.NONE, false, a.GetComponent<Object>(), b.GetComponent<Object>(), Vector3.zero, Vector3.zero);
        }


        if (t1 < t2)
        {
            if ((t1 > 0) && (t1 <= Time.fixedDeltaTime))//t1 is valid
            {
                t = t1;
            }
            else
            {
                t = -1;
            }
        }
        else
        {
            if ((t2 > 0) && (t2 <= Time.fixedDeltaTime))//t2 is valid
            {
                t = t2;
            }
            else
            {
                t = -1;
            }
        }




        if (t == -1)
        {
            //no collision
            UnityEngine.Debug.Log("EXIT_2");
            UnityEngine.Debug.Log("new CollisionF");
            return new Collision(Collision.CollisionType.NONE, false, a.GetComponent<Object>(), b.GetComponent<Object>(), Vector3.zero, Vector3.zero);
        }


        UnityEngine.Debug.Log("T1: " + t1);
        UnityEngine.Debug.Log("T2: " + t2);

        Vector3 nextMoveA = (t * a.GetComponent<Object>().velocity);
        Vector3 nextMoveB = (t * b.GetComponent<Object>().velocity);

        UnityEngine.Debug.Log("new CollisionG");
        return new Collision(Collision.CollisionType.SPHERE_TO_SPHERE, true, a.GetComponent<Object>(), b.GetComponent<Object>(), nextMoveA, nextMoveB);
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
