using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialHashingGrid : MonoBehaviour
{
    int resolution;
    List<Particle> particles;

    SpatialHashingGrid(int p_resolution, List<Particle> p_particles) 
    {
        resolution = p_resolution;
        particles = p_particles;
    }

    struct Key 
    {
        public int x;
        public int y;
        public int z;
    }

    Dictionary<Key, List<Particle>> grid;

    private void FixedUpdate()
    {
        RemoveParticles();
        
        foreach(Particle p in particles) 
        {
            AddParticle(p);
        }
    }

    void AddParticle(Particle particle) 
    {
        if(grid == null) 
        {
            grid = new Dictionary<Key, List<Particle>>();
        }

        Key key = CalculateKey(particle.transform.position);

        if (grid.ContainsKey(key)) //Add to existing cell 
        {
            List<Particle> particlesRef;
            grid.TryGetValue(key, out particlesRef);
            particlesRef.Add(particle);
        }
        else // Create a new cell and add the particle 
        {
            List<Particle> particlesRef = new List<Particle>();
            particlesRef.Add(particle);
            grid.Add(key, particlesRef);
        }
    }

    void RemoveParticles() 
    {
        grid.Clear();
    }

    Key CalculateKey(Vector3 p_position) 
    {
        int x = (int)(p_position.x / resolution);
        int y = (int)(p_position.y / resolution);
        int z = (int)(p_position.z / resolution);

        Key key;
        key.x = x;
        key.y = y;
        key.z = z;

        return key;
    }

    List<Particle> GetParticlesAtCell(Key key) 
    {
        List<Particle> particlesRef;
        grid.TryGetValue(key, out particlesRef);

        return particlesRef;
    }

    List<Particle> GetNeighbors(Vector3 position, int cellSearchSpan) 
    {
        List<Particle> particlesRef = new List<Particle>();

        Key key = CalculateKey(position);

        for (int z = -cellSearchSpan; z < cellSearchSpan; z++) 
        {
            for (int y = -cellSearchSpan; y < cellSearchSpan; y++)
            {
                for (int x = -cellSearchSpan; x < cellSearchSpan; x++)
                {
                    Key newKey;
                    newKey.x = key.x + x;
                    newKey.y = key.y + y;
                    newKey.z = key.z + z;

                    if (grid.ContainsKey(newKey)) 
                    {
                        particlesRef.AddRange(GetParticlesAtCell(newKey));
                    }
                }
            }
        }

        return particlesRef;
    }
}
