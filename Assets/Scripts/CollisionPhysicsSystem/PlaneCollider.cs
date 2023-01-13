using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : Collider
{
    public Vector2 size;
    public Vector3 normal = new Vector3(0,1,0);

    PlaneCollider(Vector2 p_size) 
    {
        size = p_size;
        normal = gameObject.transform.up;
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
        print("Plane: sphere to plane");

        return CollisionCalculator.CalcSphereToPlaneCollision(sphereCollider, this);
    }
    public override Collision TestCollision(PlaneCollider planeCollider)
    {
        return null;
    }
}
