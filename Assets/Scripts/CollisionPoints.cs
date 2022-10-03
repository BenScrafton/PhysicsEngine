using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPoints : MonoBehaviour
{
    public Vector3 colliderA_point; //FurthestPoint of colliderA into colliderB
    public Vector3 colliderB_point; //FurthestPoint of colliderB into colliderA

    public Vector3 direction; //colliderB_point - colliderA_point normalised
    public float depth; //Length of colliderB_point - colliderA_point

    CollisionPoints(Vector3 p_colliderA_point, Vector3 p_colliderB_point, Vector3 p_direction, float p_depth) 
    {
        colliderA_point = p_colliderA_point;
        colliderB_point = p_colliderB_point;
        direction = p_direction;
        depth = p_depth;
    }
}
