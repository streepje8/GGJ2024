using TMPro;
using UnityEngine;

public class LobbyScreen : MonoBehaviour
{
    private NetworkingModule networking;

    public TMP_Text player1;
    public TMP_Text player2;
    public TMP_Text  player3;
    public TMP_Text  player4;
    public TMP_Text  player5;
    
    private void Awake()
    {
        if(!PersistentGameState.TryFindObject("Networking", out NetworkingModule networking))
        {
            enabled = false;
            return;
        }
        this.networking = networking;
        player1.text = "waiting for player...";
        player2.text = "waiting for player...";
        player3.text = "waiting for player...";
        player4.text = "waiting for player...";
        player5.text = "waiting for player...";
    }

    private void Update()
    {
        switch (networking.PLAYERCOUNT)
        {
            case 0:
                player1.text = "waiting for player...";
                player2.text = "waiting for player...";
                player3.text = "waiting for player...";
                player4.text = "waiting for player...";
                player5.text = "waiting for player...";
                break;
            case 1:
                player1.text = "Player 1 (DEAD)";
                player2.text = "waiting for player...";
                player3.text = "waiting for player...";
                player4.text = "waiting for player...";
                player5.text = "waiting for player...";
                break;
            case 2:
                player1.text = "Player 1 (DEAD)";
                player2.text = "Player 2";
                player3.text = "waiting for player...";
                player4.text = "waiting for player...";
                player5.text = "waiting for player...";
                break;
            case 3:
                player1.text = "Player 1 (DEAD)";
                player2.text = "Player 2";
                player3.text = "Player 3";
                player4.text = "waiting for player...";
                player5.text = "waiting for player...";
                break;
            case 4:
                player1.text = "Player 1 (DEAD)";
                player2.text = "Player 2";
                player3.text = "Player 3";
                player4.text = "Player 4";
                player5.text = "waiting for player...";
                break;
            case 5:
                player1.text = "Player 1 (DEAD)";
                player2.text = "Player 2";
                player3.text = "Player 3";
                player4.text = "Player 4";
                player5.text = "Player 5";
                break;
        }
    }
}
