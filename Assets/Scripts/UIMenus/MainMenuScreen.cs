using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    private UICanvas canvas;
    private NetworkingModule networking;
    private void Awake()
    {
        canvas = GetComponentInParent<UICanvas>(true);
        if (!PersistentGameState.TryFindObject("Networking", out NetworkingModule networking)) enabled = false;
        this.networking = networking;
    }

    public void Host()
    {
        canvas.ShowScreen("host");
        HostAsync().ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.LogError(t.Exception);
            }
        });
    }

    public async Task HostAsync()
    {
        await networking.HostAsync();
        await networking.ConnectToServerAsync("127.0.0.1").ContinueWith(t =>
        {
            if (t.IsFaulted) throw t.Exception ?? new Exception("jldfhbglfjkgfsjhlg");
        });
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Join()
    {
        canvas.ShowScreen("join");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
