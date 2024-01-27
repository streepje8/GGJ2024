using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ServerBrowser : MonoBehaviour
{
    public Transform serverContainer;
    public GameObject serverListing;

    private List<ServerListing> listings = new List<ServerListing>();
    private NetworkingModule networking;
    
    private void Awake()
    {
        if (!PersistentGameState.TryFindObject("Networking", out NetworkingModule networking)) enabled = false;
        this.networking = networking;
    }

    private bool isUpdating = false;
    private float updateTimer = 0f;
    
    private void Update()
    {
        if (enabled)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > 1f)
            {
                if (!isUpdating)
                {
                    isUpdating = true;
                    TryUpdateAsync().ContinueWith(t =>
                    {
                        if (t.IsFaulted) throw t.Exception ?? new Exception("WTF something borke");
                    });
                }
                updateTimer = 0;
            }
        }
    }

    private async Task TryUpdateAsync()
    {
        ServerData data = await networking.TryReceiveIPAsync();
        if (data.success)
        {
            bool isAlreadyListing = false;
            foreach (var listing in listings)
            {
                if (listing.ip.Equals(data.ip, StringComparison.OrdinalIgnoreCase)) isAlreadyListing = true;
            }

            if (!isAlreadyListing)
            {
                ServerListing listing = Instantiate(serverListing, serverContainer).GetComponent<ServerListing>();
                listing.ip = data.ip;
                listing.hostname = data.hostname;
                listing.playerCount = data.playerCount;
                listing.UpdateListingText();
                listings.Add(listing);
            }
        }
        isUpdating = false;
    }
}
