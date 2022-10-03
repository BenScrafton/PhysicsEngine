using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    Object objectA;
    Object objectB;
    CollisionPoints collisionPoints;
    Collision(Object p_objectA, Object p_objectB, CollisionPoints p_collisionPoints) 
    {
        objectA = p_objectA;
        objectB = p_objectB;
        collisionPoints = p_collisionPoints;
    }
}
