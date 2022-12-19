using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    [SerializeField] GameObject debugLine;
    List<LineRenderer> debugLineRenderers = new List<LineRenderer>();

    List<Particle> particles = new List<Particle>(); //Change to spatial hashing later


    [Header("Generic Values")]
    [SerializeField] float interactionRadius;

    [Space]

    [Header("Elasticity Values")]
    [SerializeField] float springConstant;
    [SerializeField] float springRestLength;


    [Space]

    [Header("Plasticity Values")]
    [SerializeField] float plasticityConstant;
    [SerializeField] float yieldRatio;

    [Space]

    [Header("Density & Pressure Values")]
    [SerializeField] float restDensity;
    [SerializeField] float pressureConstant;
    [SerializeField] float nearInteractionRadius;
    [SerializeField] float nearPressureConstant;


    List<Spring> springs = new List<Spring>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Particle particle in FindObjectsOfType<Particle>()) 
        {
            particles.Add(particle);
        }
    }

    private void FixedUpdate()
    {
        //SIMULATION STEPS:

        //ApplyGravity();
        //ApplyViscosity();
        //AdvanceToPredictedPos();
        AdjustSprings();
        ApplySpringDisplacements();
        DoubleDenistyRelaxation();
        //ResolveCollisions();
        CalculateNextVelocities();
    }

    void AdjustSprings()
    {
        AddSprings();
        
        if (springs.Count == 0) 
        {
            return;
        }

        PlasticallyDeformSprings();
        RemoveSprings();
    }

    void PlasticallyDeformSprings() 
    {
        foreach (Spring spring in springs) 
        {
            spring.ModifyRestLength();
        }
    }

    void AddSprings() 
    {
        //Replace with Spatial Hashing later
        foreach (Particle a in particles)
        {
            foreach (Particle b in particles)
            {
                if (a == b || a.particleConnections.Contains(b))
                {
                    continue;
                }

                float dist = Vector3.Distance(a.transform.position, b.transform.position);
                if (dist <= interactionRadius)
                {
                    //Add a spring between particles
                    LineRenderer lr = Instantiate(debugLine).GetComponent<LineRenderer>();

                    Spring spring = new Spring(a, b, 
                                               springConstant, springRestLength, interactionRadius, 
                                               plasticityConstant, yieldRatio, 
                                               lr);
                    springs.Add(spring);
                }
            }
        }
    }

    void RemoveSprings() 
    {
        for (int i = springs.Count - 1; i >= 0; i--)
        {
            Spring spring = springs[i];

            if (!spring.CheckSpringIsValid()) 
            {
                Particle a = spring.particleA;
                Particle b = spring.particleB;

                a.particleConnections.Remove(b);
                b.particleConnections.Remove(a);

                springs.Remove(spring);

                spring.CleanUp();
                spring = null;
            }
        }
    }

    void ApplySpringDisplacements()
    {
        foreach (Spring spring in springs)
        {
            spring.ApplyDisplacements();
        }
    }

    void DoubleDenistyRelaxation() 
    {
        UpdateNearNeighbors();
        CalculateDensities();
        DensityRexlaxation();
    }

    void UpdateNearNeighbors()
    {
        RemoveNearNeighbors();
        AddNearNeighbors();
    }

    void AddNearNeighbors() 
    {
        foreach(Particle a in particles) 
        {
            foreach (Particle b in a.particleConnections) 
            {
                if (a == b)
                {
                    continue;
                }

                float distBetweenParticles = Vector3.Distance(a.transform.position, b.transform.position);
                if ((distBetweenParticles <= nearInteractionRadius) && 
                    (!a.nearParticleConnections.Contains(b))) 
                {
                    a.nearParticleConnections.Add(b);
                }
            }
        }
    }

    void RemoveNearNeighbors() 
    {
        foreach(Particle a in particles) 
        {
            for (int i = a.nearParticleConnections.Count - 1; i >= 0; i--)
            {
                Particle b = a.nearParticleConnections[i];

                float distBetweenParticles = Vector3.Distance(a.transform.position, b.transform.position);
                if (distBetweenParticles > nearInteractionRadius) 
                {
                    a.nearParticleConnections.Remove(b);
                }
            }
        } 
    }

    void CalculateDensities() 
    {
        foreach (Particle a in particles)
        {
            //Calculate density value per particle
            float density = 0;

            foreach (Particle b in a.particleConnections) //Loop through particle a's neighbors 
            {
                float distBetweenParticles = Vector3.Distance(a.transform.position, b.transform.position);
                float baseValue = (1 - (distBetweenParticles / interactionRadius));

                density += baseValue * baseValue;
            }

            //Calculate near density value per particle
            float nearDensity = 0;

            foreach(Particle b in a.nearParticleConnections) 
            {
                float distBetweenParticles = Vector3.Distance(a.transform.position, b.transform.position);
                float baseValue = (1 - (distBetweenParticles / interactionRadius));

                nearDensity += baseValue * baseValue * baseValue;
            }

            //Apply density and near density values
            a.density = density;
            a.nearDensity = nearDensity;
        }
    }

    void DensityRexlaxation() 
    {
        float deltaTimeSquared = Time.fixedDeltaTime * Time.fixedDeltaTime;

        foreach (Particle a in particles) 
        {
            float pressure = pressureConstant * (a.density - restDensity);
            float nearPressure = nearPressureConstant * a.nearDensity;

            foreach (Particle b in a.particleConnections) 
            {
                Vector3 dir = b.transform.position - a.transform.position;
                Vector3.Normalize(dir);
                float distBetweenParticles = Vector3.Distance(a.transform.position, b.transform.position);
                float scaleFactor = (1 - distBetweenParticles / interactionRadius);

                Vector3 displacement = deltaTimeSquared * (((pressure * scaleFactor)) + (nearPressure * scaleFactor * scaleFactor)) * dir;

                //Apply equal and opposite displacement
                a.transform.position -= displacement / 2;
                b.transform.position += displacement / 2;
            }
        }
    }

    void CalculateNextVelocities() 
    {

    }
}
