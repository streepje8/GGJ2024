using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;
using UnityEngine.UI;

public class GamejamSolutions : MonoBehaviour
{
    public static GamejamSolutions instance;

    public List<List<Texture2D>> textures = new List<List<Texture2D>>();
    public static int ReceivedClients = 0;

    public void OnReceive()
    {
        ReceivedClients++;
        if (ReceivedClients >= FindObjectOfType<NetworkingModule>().PLAYERCOUNT - 1)
        {
            FindObjectOfType<UICanvas>().ShowScreen("voting");
            ReviewScreen.instance.HandOff(textures);
        }
    }


    private void OnApplicationQuit()
    {
        foreach (var texture2Ds in textures)
        {
            foreach (var texture2D in texture2Ds)
            {
                Destroy(texture2D);
            }
        }
    }

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
