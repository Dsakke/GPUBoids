using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BoidData // this is a struct that should only contain data about boids that is needed by the compute shader
{
    public BoidData(Vector3 startPos)
    {
        position = startPos;
        velocity = Vector3.zero;
    }
    public Vector3 position;
    public Vector3 velocity;
}

public class Flock : BaseFlock
{
    [Header("Wander Settings")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_Wander = 0.2f;
    [SerializeField]
    private float m_WanderCubeSize = 5.0f;
    [SerializeField]
    private float m_WanderCubeDistance = 10.0f;

    // Start is called before the first frame update
    protected override void Start()
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

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void SetComputeShaderInput()
    {
        base.SetComputeShaderInput();
        m_BoidCompute.SetFloat("g_Wander", m_Wander);
        m_BoidCompute.SetFloat("g_WanderCubeSize", m_WanderCubeSize);
        m_BoidCompute.SetFloat("g_WanderCubeDistance", m_WanderCubeDistance);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
