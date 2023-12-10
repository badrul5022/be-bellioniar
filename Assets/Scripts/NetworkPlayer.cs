using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeName")]
    public string name;
    [SyncVar(hook = "OnChangeID")]
    public int id;
    [SyncVar(hook = "OnChangeFill")]
    public float fill;
    [SyncVar(hook = "OnChangeCash")]
    public int cash;
    [SyncVar(hook = "OnGetBankRuptted")]
    public bool bankRuptted;
    [SyncVar]
    public bool stopped;
    public bool hasAuth;
    public TMPro.TextMeshPro NameUI;
    public int index;
    public GameObject boundry;
    public Timer timer;
    public float rate;
    public bool fillable;
    public bool shiftable;
    public bool myTurn;
    public Piada piada;
    public SpriteRenderer sprite;
    public SpriteRenderer statusSprite;
    public TMPro.TextMeshPro CashUI;
    public int currentRequestedProperty;
    public GameObject decisionUI;
    public GameObject bankRuptcy;
    public float target;
    void Start()
    {
        hasAuth = hasAuthority;
        shiftable = true;
        fillable = true;
        bankRuptcy.SetActive(false);
        if (!hasAuth)
        {
            return;
        }
       // CmdSetName($"Arex{Random.Range(0,100)}");
        CmdSetName(Data.name);
        target = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(id == NetworkGameManager.instance.currentIndex)
        {
            myTurn = true;
            timer.fill.fillAmount = Mathf.Lerp(timer.fill.fillAmount, 1, 6 * Time.deltaTime);
        }
        else
        {
            myTurn = false;
            timer.fill.fillAmount = Mathf.Lerp(timer.fill.fillAmount, 0, 6 * Time.deltaTime);
        }
        if (!NetworkGameManager.instance) return;
        if (!NetworkGameManager.instance.players[NetworkGameManager.instance.currentIndex]) return;
        decisionUI.SetActive(stopped);
        if (stopped) return;
        sprite.color = NetworkGameManager.instance.colors[id];
        statusSprite.color = NetworkGameManager.instance.colors[id];
        boundry.SetActive(id == NetworkGameManager.instance.currentIndex);
        this.transform.position = SpawnManager.instance.spawnPoints[id].position;
        //timer.fill.fillAmount = Mathf.Lerp(timer.fill.fillAmount, target, 6 * Time.deltaTime);
        timer.fill.fillAmount = Mathf.Lerp(timer.fill.fillAmount, fill, 6 * Time.deltaTime);
        if (!hasAuth) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NetworkGameManager.instance.UpdateIndex();
        }
        timer.start = id == NetworkGameManager.instance.currentIndex;
        if(id == NetworkGameManager.instance.currentIndex)
        {
            CmdSetFill((fill - rate * Time.deltaTime));
            if(fillable)
            Invoke(nameof(fillOut), 0.5f);
            myTurn = true;
        }
        else
        {
            if(fill != 1)
            {
                CmdSetFill(1);
            }
            myTurn = false;
        }
        if (!NetworkGameManager.instance.localPiada) return;
        if (!piada)
        {
            piada = NetworkGameManager.instance.localPiada.GetComponent<Piada>();
        }
        CmdSetDecsion(stopped);
        if (Input.GetKeyDown(KeyCode.F))
        {
            CmdSetCash(0);
        }
    }
    public void SetBankRuppted()
    {
        CmdSetBankRuppted(true);
    }
    [Command]
    public void CmdSetDecsion(bool status)
    {
        stopped = status;
    }
    [Command]
    public void CmdSetBankRuppted(bool status)
    {
        bankRuptted = status;
    }
    public void fillOut()
    {
        fillable = false;
        fill -= rate;
        CmdSetFill(fill);
        fillable = true;
    }
    [Command]
    public void CmdSetName(string s)
    {
        name = s;
    }
    [Command]
    public void CmdSetFill(float f)
    {
        fill = f;
    }
    public void OnChangeName(string o, string n)
    {
        NameUI.text = n;
        if(NetworkGameManager.instance)
        NetworkGameManager.instance.Register();
        if (hasAuth)
        {
            CmdSpawnPiada();
            CmdSetCash(2000);
        }
        //this.transform.position = SpawnManager.instance.spawnPoints[index].position;
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnPiada()
    {
        GameObject piada = Instantiate(NetworkManager.singleton.spawnPrefabs[1]);
        NetworkServer.Spawn(piada, connectionToClient);
    }
    public void OnChangeID(int o, int n)
    {
        this.transform.position = SpawnManager.instance.spawnPoints[n].position;
    }
    public void setId(int _id)
    {
        CmdSetId(_id);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetId(int _id)
    {
        id = _id;
    }
    public void OnChangeFill(float o, float n)
    {
        // timer.fill.fillAmount = n;
        target = n;
        if(n <= 0)
        {
            if (hasAuth && shiftable)
            {
                shiftable = false;
                NetworkGameManager.instance.UpdateIndex();
                Invoke(nameof(MakeShiftable), 1f);
            }
           
        }
    }
    public void MakeShiftable()
    {
        shiftable = true;
    }
    [Command(requiresAuthority = false)]
    public void CmdSetCash(int _cash)
    {
        cash = _cash;
    }
    public void OnChangeCash(int o, int n)
    {
        CashUI.text = $"{n}$";
    }
    public void DeductCash(int amount)
    {
        if(cash < amount)
        {
            CmdSetBankRuppted(true);
            return;
        }
        if (hasAuth)
        {
            CmdSetCash(cash - amount);
        }
    }
    public void IncreamentCash(int ammount)
    {
        CmdSetCash(cash + ammount);
    }
    public void OnGetBankRuptted(bool o, bool n)
    {
        if (n)
        {
            bankRuptcy.SetActive(true);
        }
    }
}
