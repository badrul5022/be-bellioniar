using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CustomNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnClientDisconnect()
    {
        //base.OnClientDisconnect();
        print("client left");
        //CustomNetworkManager.singleton.StopClient();
       // NetworkGameManager.instance.Register();
    }
    
}
