using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Piada : NetworkBehaviour
{
    public NetworkPlayer networkPlayer;
    public string name;
    [SyncVar(hook = "OnChangeWayPoint")]
    public int id;
    private Transform target;
    public bool hasAuth;
    public static Piada instance;
    public float speed = 5;
    public SpriteRenderer renderer;
    [SyncVar(hook = "OnChangeColor")]
    public Color color;
    void Start()
    {
       
        hasAuth = hasAuthority;
        if (hasAuth)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuth) return;
        
        if (!NetworkGameManager.instance.localPlayer) return;
        if (!networkPlayer)
        {
            networkPlayer = NetworkGameManager.instance.localPlayer;
            NetworkGameManager.instance.localPiada = this.transform;
            name = networkPlayer.name;
        }
        CmdColor(NetworkGameManager.instance.colors[networkPlayer.id]);
        if (!target) return;
        this.transform.position = Vector3.Lerp(this.transform.position,target.position,speed*Time.deltaTime);
       
    }
    public void Move(int steps)
    {
        CmdSetSteps(id + steps);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetSteps(int steps)
    {
        id = steps;
    }
    public void OnChangeWayPoint(int o,int n)
    {
        int remaining = 35 - n;
       /* if(n > remaining)
        {
            n = remaining;
            if (hasAuth)
            {
                CmdSetSteps(n);
            }
        }*/
       if(n > Waypoints.wayPoints.Length-1)
        {
            CmdReset(0);
        }
        else
        {
            target = Waypoints.wayPoints[n].transform;
            if(hasAuth)
            Waypoints.waypoint[n].RequestBuy(NetworkGameManager.instance.localPlayer.id);
        }
        
    }
    [Command(requiresAuthority = false)]
    public void CmdReset(int index)
    {
        id = index;
    }
    [Command(requiresAuthority = false)]
    public void CmdColor(Color col)
    {
        color = col;
    }
    public void OnChangeColor(Color o, Color n)
    {
        renderer.color = n;
    }
}
