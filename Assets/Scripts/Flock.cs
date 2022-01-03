using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Boid // this is a struct that should only contain data about boids that is needed by the compute shader
{
    Boid(Vector3 startPos)
    {
        position = startPos;
        velocity = Vector3.zero;
    }
    Vector3 position;
    Vector3 velocity;
}

public class Flock : MonoBehaviour
{
    [SerializeField]
    private ComputeShader m_BoidCompute;
    [SerializeField]
    private int m_NrBoids;
    private ComputeBuffer m_Buffer;
    private Boid[] m_BoidData;
    // Start is called before the first frame update
    void Start()
    {
        int boidSize = sizeof(float) * 3 * 2;
        m_Buffer = new ComputeBuffer(m_NrBoids, boidSize, ComputeBufferType.Default, ComputeBufferMode.Dynamic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
