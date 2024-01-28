using Riptide;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private NetworkingModule network;

    private void Awake()
    {
        if (!PersistentGameState.TryFindObject("Networking", out NetworkingModule net))
        {
            enabled = false;
            return;
        }
        network = net;
        gameObject.SetActive(network.ServerIsRunning);
    }

    public void Press()
    {
        Message startMessage = Message.Create(MessageSendMode.Reliable, 2);
        startMessage.Add(Random.Range(0, 9999999));
        network.Server.SendToAll(startMessage);
    }
}
