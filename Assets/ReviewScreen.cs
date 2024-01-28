using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ReviewScreen : MonoBehaviour
{
    public List<GameObject> playerIndicators = new List<GameObject>();
    public RawImage storyBoardImg;
    public int frame = 0;
    public int currentPlayerIndex = 0;
    public TMP_Text frameNumDisplay;
    public GameObject toToggle;
    
    public static ReviewScreen instance;

    private List<List<Texture2D>> texs;
    private UICanvas canvas;
    
    private void Awake()
    {
        instance = this;
        foreach (var playerIndicator in playerIndicators)
        {
            playerIndicator.SetActive(false);
        }
        playerIndicators[0].SetActive(true);
        canvas = GetComponentInParent<UICanvas>();
    }

    public void NextIndex()
    {
        frame++;
        if (frame < 0) frame = 2;
        frame = frame % 3;
        frameNumDisplay.text = $"{frame + 1}/3";
        UpdateStoryBoard();
    }
    
    public void PreviousIndex()
    {
        frame--;
        if (frame < 0) frame = 2;
        frame = frame % 3;
        frameNumDisplay.text = $"{frame + 1}/3";
        UpdateStoryBoard();
    }

    public void NextPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex > 4) //Death has finished, ignore button press
        {
            currentPlayerIndex = 4;
            return;
        }
        if (currentPlayerIndex >= FindObjectOfType<NetworkingModule>().PLAYERCOUNT - 1)
        {
            currentPlayerIndex = 4;
            toToggle.SetActive(true);
        }
        foreach (var playerIndicator in playerIndicators)
        {
            playerIndicator.SetActive(false);
        }
        playerIndicators[currentPlayerIndex].SetActive(true);
        frame = 0;
        frameNumDisplay.text = $"{frame + 1}/3";
        UpdateStoryBoard();
    }

    public void Finish()
    {
        canvas.ShowScreen("credits");
    }

    private void UpdateStoryBoard()
    {
        if(currentPlayerIndex < 4) storyBoardImg.texture = texs[currentPlayerIndex][frame];
    }

    public void HandOff(List<List<Texture2D>> textures)
    {
        texs = textures;
        frame = 0;
        currentPlayerIndex = 0;
        frameNumDisplay.text = $"{frame + 1}/3";
        UpdateStoryBoard();
    }

    private void OnDestroy()
    {
        foreach (var texture2Ds in texs)
        {
            foreach (var texture2D in texture2Ds)
            {
                Destroy(texture2D);
            }
        }
    }
}
