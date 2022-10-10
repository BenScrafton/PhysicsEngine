using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public bool collided;
    public Object objectA;
    public Object objectB;
    public Vector3 nextMove;

    public Collision(bool p_collided, Object p_objectA, Object p_objectB, Vector3 p_nextMove) 
    {
        collided = p_collided;
        objectA = p_objectA;
        objectB = p_objectB;
        nextMove = p_nextMove;
    }
}
