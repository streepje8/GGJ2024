using System;
using TMPro;
using UnityEngine;

public class ServerListing : MonoBehaviour
{
    public string hostname;
    public string ip;
    public int playerCount = 0;

    public TMP_Text hostNameText;
    public TMP_Text playerCountText;

    private NetworkingModule networking;
    
    private void Awake()
    {
        if (!PersistentGameState.TryFindObject("Networking", out NetworkingModule networking)) enabled = false;
        this.networking = networking;
    }

    public void Join()
    {
        GetComponentInParent<UICanvas>(true).ShowScreen("joinload");
        networking.ConnectToServerAsync(ip).ContinueWith(t =>
        {
            if (t.IsFaulted) throw t.Exception ?? new Exception("jldfhbglfjkgfsjhlg");
        });
    }
    
    public void UpdateListingText()
    {
        hostNameText.text = hostname;
        playerCountText.text = $"{playerCount}/5";
    }
}
