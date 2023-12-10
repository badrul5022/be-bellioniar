using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkGameManager : NetworkBehaviour
{
    public bool auth;
    public List<NetworkPlayer> players;
    public NetworkPlayer localPlayer;
    public Transform localPiada;
    public static NetworkGameManager instance;
    [SyncVar]
    public int currentIndex;
    public Color[] colors;
    void Start()
    {
        auth = isServer;
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }
        if (players.Count == 0) return;
        if (!players[currentIndex])
        {
            Register();
            return;
        }
        else if (players[currentIndex].bankRuptted)
        {
            CmdUpdateIndex();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {

        }
    }
    public void Register()
    {
        if (NetworkClient.ready)
        {
            CmdRegister();
        }
        
    }
   
    public void UpdateIndex()
    {
        CmdUpdateIndex();
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateIndex()
    {
        RpcUpdateTurn();
    }
    [ClientRpc]
    public void RpcUpdateTurn()
    {
        if (!players[currentIndex])
        {
            UpdateIndex();
        }
        else
        {
            if (isServer)
            {
                currentIndex++;
                if(currentIndex >= players.Count)
                {
                    currentIndex = 0;
                }
                CmdSetIndex(currentIndex);
            }
           
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdSetIndex(int _index)
    {
        currentIndex = _index;
    }
    [Command(requiresAuthority = false)]
    public void CmdRegister()
    {
        RpcRegister();
    }
    [ClientRpc]
    public void RpcRegister()
    {
        players.Clear();
        NetworkPlayer[] _players = FindObjectsOfType<NetworkPlayer>();
        int index = 0;
        foreach(NetworkPlayer plr in _players)
        {
            players.Add(plr);
            if (plr.hasAuth)
            {
                localPlayer = plr;
                localPlayer.setId(index);
            }
            index++;
        }
    }
}
