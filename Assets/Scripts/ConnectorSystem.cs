using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LightReflectiveMirror;
using UnityEngine.Networking;
using TMPro;
using System;
public class ConnectorSystem : MonoBehaviour
{
    public LightReflectiveMirrorTransport transport;
    public static ConnectorSystem instance;
    public TextMeshProUGUI serverText;
    public TMP_InputField matchID;
    public List<Room> rooms;
    public TextMeshProUGUI servername;
    public GameObject inputHolder;
    public TMP_InputField input;
    public TMP_InputField username;
    public string id;
    void Start()
    {
        instance = this;
        
        //DontDestroyOnLoad(this.gameObject);
    }
    private void Update() {
      //  if(transport.serverId !="")
      // serverText.text = $"invite code : {transport.serverId}";
      if(!transport){
        transport = GameObject.FindObjectOfType<LightReflectiveMirrorTransport>();
      }else{
        id = transport.serverId;
         Data.serverID = transport.serverId;
      }
        if (Input.GetKeyDown(KeyCode.O))
        {
            NetworkManager.singleton.networkAddress = matchID.text;
            Data.serverID = matchID.text;
            NetworkManager.singleton.StartClient();
        }
    }
    public void StopClient()
    {
        CustomNetworkManager.singleton.StopClient();
    }
    public void ConnectToServer(){
        Data.name = username.text;
        if (Data.name == "") return;
        Host();
        //CustomNetworkManager.singleton.StartHost();
        //return;
        //190.92.179.189
        //121.52.158.157
       // StartCoroutine(FindHost("http://121.52.158.157:3001/"));

       // transport.RequestServerList();
      //  string id = input.text;
      //  id = id.ToUpper();
      //  NetworkManager.singleton.networkAddress = id.ToString();
      //  Destroy(inputHolder);
      //  NetworkManager.singleton.StartClient();
    }
    public void Host(){
        Data.isServer = true;
        //Data.name = username.text;
        //inputHolder.SetActive(false);
         CustomNetworkManager.singleton.StartHost();
       
    }
     IEnumerator FindHost(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    if(webRequest.downloadHandler.text != ""){
                        Destroy(inputHolder);
                        NetworkManager.singleton.networkAddress = webRequest.downloadHandler.text;
                        Data.serverID = webRequest.downloadHandler.text;
                        print(webRequest.downloadHandler.text);
                        NetworkManager.singleton.StartClient();
                    }else{
                        Host();
                    }
                    break;
            }
        }
    }
}
