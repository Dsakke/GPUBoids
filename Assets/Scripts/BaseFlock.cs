using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlock : MonoBehaviour
{
    readonly protected int m_ThreadGroupSize = 1024; // This needs to be hardcoded in the shader so you should make sure this is the same as "threadGroupsSize" in the compute shader


    [Header("General settings")]
    [SerializeField]
    protected ComputeShader m_BoidCompute = null;
    [Header("Flock Settings")]
    [SerializeField]
    protected int m_NrBoids = 100;
    protected ComputeBuffer m_BoidBuffer;
    protected BoidData[] m_BoidData;
    [SerializeField]
    protected float m_WorldSize = 1000.0f;
    [SerializeField]
    protected float m_NeighborhoodRadius = 20.0f;
    [Header("Boid Settings")]
    [SerializeField]
    protected float m_Acceleration = 10.0f;
    [SerializeField]
    [Range(0.1f, 20.0f)]
    protected float m_MaxSpeed = 50.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_Cohesion = 0.158f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_Allignment = 0.492f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_Seperation = 0.162f;

    [Header("Graphics Settings")]
    [SerializeField]
    protected Mesh m_BoidMesh = null;
    [SerializeField]
    protected Material m_Material = null; // Material must be instanced for good performance
    protected uint[] m_Args = new uint[5] { 0, 0, 0, 0, 0 };
    protected ComputeBuffer m_BufferWithArgs = null;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        m_BoidData = new BoidData[m_NrBoids];
        for (int i = 0; i < m_NrBoids; ++i)
        {
            m_BoidData[i] = new BoidData(new Vector3(Random.Range(-m_WorldSize, m_WorldSize), Random.Range(-m_WorldSize, m_WorldSize), Random.Range(-m_WorldSize, m_WorldSize)));
            m_BoidData[i].velocity = Vector3.forward;
        }

        int posVecSize = sizeof(float) * 3;
        int velocityVecSize = sizeof(float) * 3;
        int boidSize = posVecSize + velocityVecSize;
        m_BoidBuffer = new ComputeBuffer(m_NrBoids, boidSize);
        m_BoidBuffer.SetData(m_BoidData);

        m_BufferWithArgs = new ComputeBuffer(1, sizeof(uint) * 5, ComputeBufferType.IndirectArguments);
        m_Args[0] = (uint)m_BoidMesh.GetIndexCount(0);
        m_Args[1] = (uint)m_NrBoids;
        m_Args[2] = (uint)m_BoidMesh.GetIndexStart(0);
        m_Args[3] = (uint)m_BoidMesh.GetBaseVertex(0);
        m_BufferWithArgs.SetData(m_Args);
    }

    virtual protected void FixedUpdate()
    {
        SetComputeShaderInput();
        int kernelId = m_BoidCompute.FindKernel("CSMain");
        int threadGroups = Mathf.CeilToInt(m_NrBoids / m_ThreadGroupSize);
        m_BoidCompute.Dispatch(kernelId, threadGroups + 1, 1, 1);
        m_BoidBuffer.GetData(m_BoidData);
    }

    virtual protected void Update()
    {
        // Render
        Bounds bounds = new Bounds(Vector3.zero, new Vector3(m_WorldSize * 2, m_WorldSize * 2, m_WorldSize * 2));
        m_Material.SetBuffer("g_Boids", m_BoidBuffer);
        Graphics.DrawMeshInstancedIndirect(m_BoidMesh, 0, m_Material, bounds, m_BufferWithArgs);
    }

    virtual protected void SetComputeShaderInput()
    {
        int kernel = m_BoidCompute.FindKernel("CSMain");
        m_BoidCompute.SetBuffer(kernel, "g_Boids", m_BoidBuffer);
        m_BoidCompute.SetFloat("g_WorldSize", m_WorldSize);
        m_BoidCompute.SetFloat("g_NeighborhoodRad", m_NeighborhoodRadius);
        m_BoidCompute.SetFloat("g_Acceleration", m_Acceleration);
        m_BoidCompute.SetFloat("g_MaxSpeed", m_MaxSpeed);
        m_BoidCompute.SetInt("g_NrBoids", m_NrBoids);
        m_BoidCompute.SetFloat("g_Seperation", m_Seperation);
        m_BoidCompute.SetFloat("g_Cohesion", m_Cohesion);
        m_BoidCompute.SetFloat("g_Allignment", m_Allignment);
    }

    virtual protected void OnDestroy()
    {
        m_BoidBuffer.Dispose();
        m_BufferWithArgs.Dispose();
    }
}
