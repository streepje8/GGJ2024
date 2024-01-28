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
        nameField.onValueChanged.AddListener(ChangeName);
    }

    private void ChangeName(string newName)
    {
        networking.SetLobbyName(newName);
    }
}
