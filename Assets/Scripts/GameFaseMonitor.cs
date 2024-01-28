using System;
using Riptide;
using TMPro;
using UnityEngine;

public class GameFaseMonitor : MonoBehaviour
{
    public static GameFaseMonitor instance;

    private NetworkingModule network;
    private UICanvas canvas;
    private PersistentGameData data;
    
    private void Awake()
    {
        instance = this;
        canvas = GetComponent<UICanvas>();
        if (!PersistentGameState.TryFindObject("Networking", out NetworkingModule net))
        {
            enabled = false;
            return;
        }
        network = net;
        if (!PersistentGameState.TryFindObject("Data", out PersistentGameData dat))
        {
            enabled = false;
            return;
        }
        data = dat;
    }

    [MessageHandler(2)]
    private static void HandleGameStart(Message message)
    {
        int promptRNG = message.GetInt();
        instance.StartGame(promptRNG);
    }

    private void StartGame(int promptRng)
    {
        if (network.networkMode == NetworkMode.Host)
        {
            canvas.ShowScreen("gamedead");
        }
        else
        {
            canvas.ShowScreen("gamealive");
            GameObject.Find("PromptText").GetComponent<TMP_Text>().text = data.FunnyPrompts.Prompts[Mathf.Abs(promptRng) % (data.FunnyPrompts.Prompts.Count + 1)];
        }
    }
}
