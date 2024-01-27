using System;
using TMPro;
using UnityEngine;
// ReSharper disable Unity.NoNullPropagation

public class SceneBuilder : MonoBehaviour
{
    [field: SerializeField] public Transform FrameA { get; private set; }
    [field: SerializeField] public Transform FrameB { get; private set; }
    [field: SerializeField] public Transform FrameC { get; private set; }
    [field: SerializeField] public Transform Drawer { get; private set; }
    [field: SerializeField] public TMP_Text FrameText { get; private set; }
    [field: SerializeField] public GameObject ActiveFunnyPrefab { get; private set; }


    [field: Header("Managed by code")]
    [field: SerializeField] public ActiveFunny SelectedFunny { get; private set; } = null;
    [field: SerializeField] public Transform ActiveFrame { get; private set; } = null;
    [field: SerializeField] public bool DrawerState { get; private set; } = false;
    public int ActiveFrameIndex { get; private set; } = 0;

    private static SceneBuilder instance;

    private void Awake()
    {
        instance = this;
        FrameB.gameObject.SetActive(false);
        FrameC.gameObject.SetActive(false);
    }

    public void SetFrame(int frame)
    {
        frame = frame % 3;
        FrameA.gameObject.SetActive(false);
        FrameB.gameObject.SetActive(false);
        FrameC.gameObject.SetActive(false);
        switch (frame)
        {
            case 0:
                FrameA.gameObject.SetActive(true);
                ActiveFrame = FrameA;
                break;
            case 1:
                FrameB.gameObject.SetActive(true);
                ActiveFrame = FrameB;
                break;
            case 2:
                FrameC.gameObject.SetActive(true);
                ActiveFrame = FrameC;
                break;
        }
        ActiveFrameIndex = frame;
        FrameText.text = $"{ActiveFrameIndex + 1}/3";
    }

    public static void CreateFunny(Funny funny)
    {
        instance.CreateFunnyLocal(funny);
    }

    private void CreateFunnyLocal(Funny funny)
    {
        ActiveFunny afunny = Instantiate(ActiveFunnyPrefab, ActiveFrame).GetComponent<ActiveFunny>();
        afunny.SetFunny(funny);
        afunny.transform.localPosition = new Vector3(-300, 150, 0);
    }

    public static void SelectFunny(ActiveFunny funny)
    {
        if(instance.SelectedFunny != null) instance.SelectedFunny.Deselect();
        instance.SelectedFunny = funny;
        if(funny != null)funny.Select();
    }

    public void MoveUp() => SelectedFunny?.Nudge(Vector3.up);
    public void MoveRight() => SelectedFunny?.Nudge(Vector3.right);
    public void MoveDown() => SelectedFunny?.Nudge(Vector3.down);
    public void MoveLeft() => SelectedFunny?.Nudge(Vector3.left);
    public void RotateLeft() => SelectedFunny?.RotateLeft();
    public void RotateRight() => SelectedFunny?.RotateRight();
    public void ZoomIn() => SelectedFunny?.ZoomIn();
    public void ZoomOut() => SelectedFunny?.ZoomOut();

    public void OpenDrawer()
    {
        DrawerState = true;
    }

    public void CloseDrawer()
    {
        DrawerState = false;
    }

    public void Next()
    {
        SetFrame(ActiveFrameIndex + 1);
    }

    public void Prev()
    {
        SetFrame(ActiveFrameIndex - 1);
    }

    public void Camera()
    {
        
    }
    
    private void Update()
    {
        Vector3 goalPosition = DrawerState ? Vector3.zero : Vector3.down * 1080;
        Drawer.transform.localPosition = Vector3.Lerp(Drawer.transform.localPosition, goalPosition, 10f * Time.deltaTime);
    }
    
}
