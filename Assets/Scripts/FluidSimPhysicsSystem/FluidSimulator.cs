using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    [SerializeField] GameObject debugLine;
    List<LineRenderer> debugLineRenderers = new List<LineRenderer>();

    List<Particle> particles = new List<Particle>(); //Change to spatial hashing later

    Vector3 gravity = new Vector3(0, -9.0f, 0);

    SpatialHashingGrid spatialHashingGrid;

    [Header("Generic Values")]
    [SerializeField] float interactionRadius;
    [SerializeField] int maxConnections;
    
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

    [Space]

    [Header("Viscosity Values")]
    [SerializeField] float linearImpulseControl;
    [SerializeField] float quadraticImpulseControl;

    [Space]

    [Header("Stickyness")]
    [SerializeField] float slipFactor;
    [SerializeField] float stickDistance;
    [SerializeField] float stickinessConstant;

    List<Spring> springs = new List<Spring>();


    // Start is called before the first frame update
    void Start()
    {
        spatialHashingGrid = new SpatialHashingGrid(1, particles);

        foreach (Particle particle in FindObjectsOfType<Particle>()) 
        {
            particles.Add(particle);
            particle.slipFactor = slipFactor;
            particle.stickDistance = stickDistance;
            particle.stickinessConstant = stickinessConstant;
        }
    }

    private void FixedUpdate()
    {
        
        spatialHashingGrid.Update();
        
        //SIMULATION STEPS:

        ApplyGravity();
        
        ApplyViscosity();
        SetStartPositions();
        //AdvanceToPredictedPos();
        AdjustSprings();
        ApplySpringDisplacements();
        DoubleDenistyRelaxation();
        //ResolveCollisions();
        CalculateNextVelocities();
    }

    void SetStartPositions() 
    {
        foreach(Particle p in particles) 
        {
            p.previousPos = new Vector3(p.transform.position.x, p.transform.position.y, p.transform.position.z);
            p.transform.position += p.velocity * Time.deltaTime;
        }
    }

    void ApplyGravity() 
    {
        foreach(Particle p in particles) 
        {
           p.velocity += Time.fixedDeltaTime * gravity;
        }
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
        foreach (Particle a in particles)
        {
            if (a.particleConnections.Count >= maxConnections)
            {
                continue;
            }

            List<Particle> neighbors = spatialHashingGrid.GetNeighbors(a.transform.position, 1, interactionRadius);

       //     print("Neighbors " + neighbors.Count);

            foreach (Particle b in neighbors)
            {
                if (a == b) 
                {
                    continue;
                }

                if (a.particleConnections.Contains(b))
                {
                    continue;
                }

     //           float dist = Vector3.Distance(a.transform.position, b.transform.position);

      //          if (dist <= interactionRadius)
                {
                    //Add a spring between particles
                    //LineRenderer lr = Instantiate(debugLine).GetComponent<LineRenderer>();

                    Spring spring = new Spring(a, b, 
                                               springConstant, springRestLength, interactionRadius, 
                                               plasticityConstant, yieldRatio
                                             //  lr
                                               );
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

    void ApplyViscosity() 
    {
        foreach(Spring spring in springs) 
        {
            float scaleFactor = Vector3.Distance(spring.particleA.transform.position, 
                                       spring.particleB.transform.position) / interactionRadius;
            
            if(scaleFactor < 1) 
            {
                Vector3 dir = spring.particleB.transform.position - spring.particleA.transform.position;
                Vector3.Normalize(dir);
                float inwardRadialVelocity = Vector3.Dot((spring.particleA.velocity - spring.particleB.velocity), dir);

                if(inwardRadialVelocity > 0) 
                {
                    Vector3 impulse = Time.fixedDeltaTime * (1 - scaleFactor) * 
                                      (linearImpulseControl * inwardRadialVelocity + quadraticImpulseControl * inwardRadialVelocity * inwardRadialVelocity) *
                                       dir;

                    //Apply impuse in equal and opposite directions
                    spring.particleA.velocity -= impulse / 2;
                    spring.particleB.velocity += impulse / 2;
                }
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
        foreach(Particle p in particles) 
        {
            p.velocity = ((p.transform.position - p.previousPos) / Time.fixedDeltaTime); //+ p.GetComponent<Rigidbody>().velocity;
        }
    }
}
