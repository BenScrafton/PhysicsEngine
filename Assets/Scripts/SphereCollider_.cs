using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollider_ : Collider
{
    public float radius;

    SphereCollider_(float p_radius) 
    {
        radius = p_radius;
    }

    //public override CollisionPoints BoxCollision()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}
    //public override CollisionPoints SphereCollision()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}
    //public override CollisionPoints PlaneCollision()
    //{
    //    CollisionPoints c = new CollisionPoints();
    //    return c;
    //}
}
