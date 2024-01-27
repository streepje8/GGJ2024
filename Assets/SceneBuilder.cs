using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    [field: SerializeField] public Transform FrameA { get; private set; }
    [field: SerializeField] public Transform FrameB { get; private set; }
    [field: SerializeField] public Transform FrameC { get; private set; }

    [field: SerializeField] public ActiveFunny SelectedFunny { get; private set; } = null;

    private static SceneBuilder instance;

    private void Awake()
    {
        instance = this;
    }
    
    public static void SelectFunny(ActiveFunny funny)
    {
        if(instance.SelectedFunny != null) instance.SelectedFunny.Deselect();
        instance.SelectedFunny = funny;
        funny.Select();
    }

    public void MoveUp() => SelectedFunny.Nudge(Vector3.up);
    public void MoveRight() => SelectedFunny.Nudge(Vector3.right);
    public void MoveDown() => SelectedFunny.Nudge(Vector3.down);
    public void MoveLeft() => SelectedFunny.Nudge(Vector3.left);
    public void RotateLeft() => SelectedFunny.RotateLeft();
    public void RotateRight() => SelectedFunny.RotateRight();
    public void ZoomIn() => SelectedFunny.ZoomIn();
    public void ZoomOut() => SelectedFunny.ZoomOut();

    public void OpenDrawer()
    {
        
    }

    public void Next()
    {
        
    }

    public void Prev()
    {
        
    }

    public void Camera()
    {
        
    }
    
}
