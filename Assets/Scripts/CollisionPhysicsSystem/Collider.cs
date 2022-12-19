using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    public virtual Collision TestCollision(Collider collider) 
    {
        return null;
    }

    public virtual Collision TestCollision(BoxCollider box)
    {
        return null;
    }

    public virtual Collision TestCollision(SphereCollider sphereCollider)
    {
        return null;
    }
    public virtual Collision TestCollision(PlaneCollider planeCollider)
    {
        return null;
    }
}
