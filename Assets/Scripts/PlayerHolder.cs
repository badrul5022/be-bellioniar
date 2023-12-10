using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerHolder : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdSpawn();
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject plr = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
        NetworkServer.Spawn(plr, connectionToClient);
    }
}
