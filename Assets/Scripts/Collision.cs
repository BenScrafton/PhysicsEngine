﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public bool collided;
    public Object objectA;
    public Object objectB;
    public Vector3 nextMoveA;
    public Vector3 nextMoveB;

    public enum CollisionType
    {
        SPHERE_TO_PLANE,
        SPHERE_TO_SPHERE,
        NONE
    }

    public CollisionType collisionType;

    public Collision(CollisionType p_collisionType, bool p_collided, Object p_objectA, Object p_objectB, Vector3 p_nextMoveA, Vector3 p_nextMoveB) 
    {
        collisionType = p_collisionType;
        collided = p_collided;
        objectA = p_objectA;
        objectB = p_objectB;
        nextMoveA = p_nextMoveA;
        nextMoveB = p_nextMoveB;
    }
}
