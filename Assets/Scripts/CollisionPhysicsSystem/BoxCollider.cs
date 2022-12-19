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

    public override Collision TestCollision(Collider collider) 
    {
        return collider.TestCollision(this);
    }

    public override Collision TestCollision(BoxCollider box)
    {
        return null;
    }

    public override Collision TestCollision(SphereCollider sphereCollider)
    {
        return null;
    }
    public override Collision TestCollision(PlaneCollider planeCollider)
    {
        return null;
    }
}
