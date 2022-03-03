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
        base.Start();
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
