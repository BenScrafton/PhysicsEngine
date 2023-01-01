using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{
    public Particle particleA;
    public Particle particleB;

    //Elasticity
    public float springConstant;
    public float restLength;
    public float maxNeighborDistance; // interactionRadius

    //Plasticity
    //note: make plasticity and yield independant for compression and stretching
    public float plasticityConstant;
    public float yieldRatio;

    //Debug
    LineRenderer lineRenderer;

    public Spring(Particle p_particleA, Particle p_particleB, 
                  float p_springConstant, float p_restLength, float p_maxNeighborDistance,
                  float p_plasticityConstant, float p_yieldRatio
        //          LineRenderer p_lineRenderer
        )
    {
        //Initialize

        //Elasticity
        particleA = p_particleA;
        particleB = p_particleB;
        springConstant = p_springConstant;
        restLength = p_restLength;
        maxNeighborDistance = p_maxNeighborDistance;

        //Plasticity
        plasticityConstant = p_plasticityConstant;
        yieldRatio = p_yieldRatio;

        //AddConnections
        particleA.particleConnections.Add(particleB);
        particleB.particleConnections.Add(particleA);

        //Debug-----------------------------------------------------------------------------
     //   lineRenderer = p_lineRenderer;

        // set the color of the line
   //     lineRenderer.startColor = Color.blue;
   //     lineRenderer.endColor = Color.blue;

        // set width of the renderer
    //    lineRenderer.startWidth = 0.1f;
   //     lineRenderer.endWidth = 0.1f;
    }

    public void ModifyRestLength() 
    {
        //ModifyRestLength
        float tolerableDeformation = yieldRatio * restLength;
        float distBetweenParticles = Vector3.Distance(particleA.transform.position, particleB.transform.position);

        if (distBetweenParticles > restLength + tolerableDeformation) //plasticly stretch 
        {
            restLength += (Time.fixedDeltaTime * plasticityConstant * (distBetweenParticles - (restLength + tolerableDeformation))); //increase the rest length
        }
        else if (distBetweenParticles < restLength - tolerableDeformation) //plasticly compress
        {
            restLength -= (Time.fixedDeltaTime * plasticityConstant * ((restLength - tolerableDeformation) - distBetweenParticles)); //decrease the rest length
        }
    }

    public bool CheckSpringIsValid() 
    {
        float dist = Vector3.Distance(particleA.transform.position, particleB.transform.position);

        //return true;
        if (dist > maxNeighborDistance + 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ApplyDisplacements() 
    {
        //Calculate Displacement
        Vector3 unitVecAB = particleB.transform.position - particleA.transform.position;
        Vector3.Normalize(unitVecAB);
        float compression = restLength - Vector3.Distance(particleA.transform.position, particleB.transform.position);
        float timeSquared = (Time.fixedDeltaTime * Time.fixedDeltaTime);
        float dampener = 1 - (restLength / maxNeighborDistance);

        Vector3 dispalcement = timeSquared * springConstant * compression * unitVecAB;

        //Apply displacment
        particleA.transform.position -= dispalcement / 2;
        particleB.transform.position += dispalcement / 2;

        //Debug
    //    UpdateDebugLineRender();
    }

    void UpdateDebugLineRender() 
    {
        // set the position
        //lineRenderer.SetPosition(0, particleA.transform.position);
        //lineRenderer.SetPosition(1, particleB.transform.position);
    }

    public void CleanUp() 
    {
    //    GameObject.Destroy(lineRenderer.gameObject);
    }
}
