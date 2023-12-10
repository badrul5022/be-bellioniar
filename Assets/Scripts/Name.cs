using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour
{
    private TMPro.TextMeshPro name;
    void Start()
    {
        name = this.GetComponent<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkGameManager.instance.localPlayer == null) return;
        name.text = NetworkGameManager.instance.localPlayer.name;
    }
}
