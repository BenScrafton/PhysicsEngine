using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider : Collider
{
    Vector3 centerOffset;
    Vector3 size;

    BoxCollider(Vector3 p_centerOffset, Vector3 p_size) 
    {
        centerOffset = p_centerOffset;
        size = p_size;
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
