using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollider : Collider
{
    public float radius;

    SphereCollider(float p_radius) 
    {
        radius = p_radius;
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
        return CollisionCalculator.CalcSphereToSphereCollision(this, sphereCollider);
    }
    public override Collision TestCollision(PlaneCollider planeCollider)
    {
        print("Sphere: sphere to plane");

        return CollisionCalculator.CalcSphereToPlaneCollision(this, planeCollider);
    }
}
