using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : Collider
{
    public Vector2 size;

    PlaneCollider(Vector2 p_size) 
    {
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
