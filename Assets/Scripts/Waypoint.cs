using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Waypoint : NetworkBehaviour
{
    public float radius;
    [SyncVar]
    public int id;
    [SyncVar(hook = "OnSetOwner")]
    public int owner;
    [SyncVar]
    public bool purchashed;
    public int debugID;
    private Transform parent;
    public SpriteRenderer sprite;
    public PropertyNature nature;
    public Property property;
    void Start()
    {
        Waypoints.instance.updatePoints();
        Waypoints.waypoint.Add(id, this.GetComponent<Waypoint>());
        debugID = Waypoints.waypoint[id].id;
        print($"added {id}");
        sprite = this.GetComponentInChildren<SpriteRenderer>();
        nature = property.nature;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sprite) return;
        if (!purchashed)
        {
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;
            sprite.color =  NetworkGameManager.instance.colors[owner];
        }
        if (!NetworkGameManager.instance.players[owner])
        {
            CmdReset();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, radius);
    }
    public void RequestBuy(int _id)
    {
        if (purchashed)
        {
            NetworkGameManager.instance.localPlayer.DeductCash(100);
            if(!NetworkGameManager.instance.players[owner].bankRuptted)
                NetworkGameManager.instance.players[owner].IncreamentCash(100);
            DiceController.instance.Transfer();
            return;
        }
        NetworkGameManager.instance.localPlayer.currentRequestedProperty = id;
        if (property.ownable)
        {
            UIManager.instance.showPropertyInfo(property);
        }
        else
        {
            DiceController.instance.Transfer();
            NetworkGameManager.instance.localPlayer.DeductCash(property.propertyPrice);
        }
        
        //CmdSetOwner(id);
    }
    public void PurchaseProperty(int id)
    {
        CmdSetOwner(id);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetOwner(int _id)
    {
        owner = _id;
        purchashed = true;
    }
    public void OnSetOwner(int o, int n)
    {

    }
    [Command(requiresAuthority = false)]
    public void CmdReset()
    {
        owner = 0;
        purchashed = false;
    }
}
