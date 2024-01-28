using TMPro;
using UnityEngine;

public class LobbyNameField : MonoBehaviour
{
    private NetworkingModule networking;
    private TMP_InputField nameField;
    private void Awake()
    {
        if (!PersistentGameState.TryFindObject("Networking",out NetworkingModule net))
        {
            enabled = false;
            return;
        }
        networking = net;
        nameField = GetComponent<TMP_InputField>();
        nameField.onValueChanged.AddListener(ChangeName);
    }

    private void FixedUpdate()
    {
        if (networking.networkMode != NetworkMode.Host)
        {
            nameField.SetTextWithoutNotify(networking.ReceivedLobbyName);
        }
    }

    private void ChangeName(string newName)
    {
        if (networking.networkMode == NetworkMode.Host) networking.SetLobbyName(newName);
    }
}
