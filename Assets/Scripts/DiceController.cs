using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DiceController : NetworkBehaviour
{
    private Animator animator;
    public bool server;
    public static DiceController instance;
    void Start()
    {
        server = isServer;
        animator = this.GetComponent<Animator>();
        instance = this;
    }

    private void OnMouseDown()
    {
        if (NetworkGameManager.instance.localPlayer.myTurn)
        {
            if (NetworkGameManager.instance.localPlayer.bankRuptted)
            {
                Transfer();
                return;
            }
            int index = Random.Range(1, 5);
            CmdRollDice(index);
            //NetworkGameManager.instance.localPlayer.piada.Move(index);
            Piada.instance.Move(index);
        }
        
    }
    [Command(requiresAuthority = false)]
    public void CmdRollDice(int id)
    {
        RpcRoll(id);
    }
    [ClientRpc]
    public void RpcRoll(int id)
    {
        animator.SetInteger("id", id);
        animator.SetTrigger("hit");
        if (isServer)
        {
           // Invoke(nameof(Transfer), 0.5f);
        }
    }
    public void Transfer()
    {
        CmdTransfer();
    }
    [Command(requiresAuthority = false)]
    public void CmdTransfer()
    {
        RpcTransfer();
    }
    [ClientRpc]
    public void RpcTransfer()
    {
        if (isServer)
        {
            NetworkGameManager.instance.UpdateIndex();
        }
    }
}
