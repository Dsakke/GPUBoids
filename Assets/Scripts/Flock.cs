using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Boid // this is a struct that should only contain data about boids that is needed by the compute shader
{
    public Boid(Vector3 startPos)
    {
        position = startPos;
        velocity = Vector3.zero;
    }
    public Vector3 position;
    public Vector3 velocity;
}

public class Flock : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField]
    private ComputeShader m_BoidCompute = null;
    [SerializeField]
    private GameObject m_BoidObject = null;
    [Header("Flock Settings")]
    [SerializeField]
    private int m_NrBoids = 100;
    private ComputeBuffer m_Buffer;
    private Boid[] m_BoidData;
    [SerializeField]
    private float m_WorldSize = 1000.0f;
    [SerializeField]
    private float m_NeighborhoodRadius = 20.0f;
    [Header("Boid Settings")]
    [SerializeField]
    private float m_Acceleration = 10.0f;
    [SerializeField]
    private float m_MaxSpeed = 50.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_Cohesion = 0.5f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_Allignment = 0.5f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_Seperation = 0.5f;
    private List<Transform> m_BoidTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        m_BoidData = new Boid[m_NrBoids];
        for (int i = 0; i < m_NrBoids; ++i)
        {
            m_BoidData[i] = new Boid(new Vector3(Random.Range(-m_WorldSize, m_WorldSize), Random.Range(-m_WorldSize, m_WorldSize), Random.Range(-m_WorldSize, m_WorldSize)));
            GameObject newBoid = Instantiate(m_BoidObject);
            m_BoidTransforms.Add(newBoid.transform);
        }
        UpdateBoids();
        int posVecSize = sizeof(float) * 3;
        int velocityVecSize = sizeof(float) * 3;
        int boidSize = posVecSize + velocityVecSize;
        m_Buffer = new ComputeBuffer(m_NrBoids, boidSize);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int posVecSize = sizeof(float) * 3;
        int velocityVecSize = sizeof(float) * 3;
        int boidSize = posVecSize + velocityVecSize;

        SetComputeShaderInput();
        int kernelId = m_BoidCompute.FindKernel("CSMain");
        m_BoidCompute.Dispatch(kernelId, m_NrBoids / 100, 1, 1);
        m_Buffer.GetData(m_BoidData);
        UpdateBoids();
    }

    void UpdateBoids()
    {
        for(int i = 0; i < m_NrBoids; i++)
        {
            m_BoidTransforms[i].position = m_BoidData[i].position;
            m_BoidTransforms[i].forward = m_BoidData[i].velocity;
        }
    }

    void SetComputeShaderInput()
    {
        m_Buffer.SetData(m_BoidData);
        int kernel = m_BoidCompute.FindKernel("CSMain");
        m_BoidCompute.SetBuffer(kernel, "g_Boids", m_Buffer);
        m_BoidCompute.SetFloat("g_WorldSize", m_WorldSize);
        m_BoidCompute.SetFloat("g_NeighborhoodRad", m_NeighborhoodRadius);
        m_BoidCompute.SetFloat("g_Acceleration", m_Acceleration);
        m_BoidCompute.SetFloat("g_MaxSpeed", m_MaxSpeed);
        m_BoidCompute.SetInt("g_NrBoids", m_NrBoids);
        m_BoidCompute.SetFloat("g_Seperation", m_Seperation);
        m_BoidCompute.SetFloat("g_Cohesion", m_Cohesion);
        m_BoidCompute.SetFloat("g_Allignment", m_Allignment);
    }

    private void ondestroy()
    {
        m_Buffer.Dispose();
    }
}
