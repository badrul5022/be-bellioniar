using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Dictionary<int, Waypoint> waypoint;
    public static Waypoint[] wayPoints;
    public Waypoint[] localWayPoints;
    public static Waypoints instance;
    void Start()
    {
        instance = this;
        waypoint = new Dictionary<int, Waypoint>();
        wayPoints = this.GetComponentsInChildren<Waypoint>();
        localWayPoints = wayPoints;
    }
    public void updatePoints()
    {
        wayPoints = this.GetComponentsInChildren<Waypoint>();
        localWayPoints = wayPoints;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
