using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using Mirror;
using UnityEngine.Android;
using LightReflectiveMirror;
public class NetworkVoice : MonoBehaviour
{
    public string APP_ID;
    public static NetworkVoice instance;
    public NetworkManager networkManager;
    public LightReflectiveMirrorTransport transport;
    private IRtcEngine engine;
    public bool isjoined;
    public string networkAddress;
    private void Awake()
    {
        instance = this;
        engine = IRtcEngine.getEngine(APP_ID);
        engine.OnJoinChannelSuccess += OnJoinVoice;
        engine.OnLeaveChannel += OnLeaveVoice;
        engine.OnError += OnAudioError;
        engine.AdjustPlaybackSignalVolume(400);
    }
    void Start()
    {
        JoinChannel("test");
    }
    public void Join()
    {
        
    }
    private void Update()
    {
        
        if (!networkManager)
        {
            transport = FindObjectOfType<LightReflectiveMirrorTransport>();
            networkManager = FindObjectOfType<NetworkManager>();
            networkAddress = transport.serverId;
        }
        else
        {
            if (networkManager.numPlayers >= 2 && Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                ///JoinChannel(networkManager.networkAddress);
               // JoinChannel(transport.serverId);
            }
            

        }
    }
    public void JoinChannel(string id)
    {
        if (!isjoined)
        {
            int ret = engine.AdjustAudioMixingVolume(100);
            engine.JoinChannel(id, "extra", 0);
            isjoined = true;
        }
    }
    private void OnJoinVoice(string channelName, uint id, int elasped)
    {
        print("Joined");
    }
    private void OnLeaveVoice(RtcStats stats)
    {
        print("left channel");
    }
    private void OnAudioError(int error, string msg)
    {
        print(msg);
    }
    // Update is called once per frame

    void OnApplicationQuit()
    {
        if (engine != null)
        {
            IRtcEngine.Destroy();
        }
    }
    private void OnDestroy()
    {
        if (engine != null)
        {
            IRtcEngine.Destroy();
        }
    }

}
