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
}
