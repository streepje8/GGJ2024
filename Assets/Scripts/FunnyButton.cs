using UnityEngine;
using UnityEngine.UI;

public class FunnyButton : MonoBehaviour
{
    public Image image;
    private Funny funny;
    
    public void SetFunny(Funny funny)
    {
        this.funny = funny;
        image.sprite = funny.FunnyArt;
    }
    
    public void Press()
    {
        SceneBuilder.CreateFunny(funny);
        SceneBuilder.CloseDrawerStatic();
    }
}
