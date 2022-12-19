using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 previousPos;
    public List<Particle> particleConnections = new List<Particle>();
    public List<Particle> nearParticleConnections = new List<Particle>();
    public float density;
    public float nearDensity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
