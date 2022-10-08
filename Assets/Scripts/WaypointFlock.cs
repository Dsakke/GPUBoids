using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaypointFlock : BaseFlock
{
    private int[] m_WaypointGroups;

    [Header("Waypoint Settings")]
    [SerializeField]
    private static int m_MaxWaypoints = 10;
    [SerializeField]
    private static int m_NumberOfGroups = 10;

    private List<Vector3> m_Waypoints =  new List<Vector3>();
    private ComputeBuffer m_WayPointsBuffer = null;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void SetComputeShaderInput()
    {
        base.SetComputeShaderInput();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

}
