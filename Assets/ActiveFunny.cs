using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveFunny : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public Funny Funny { get; private set; }
    [field: SerializeField]public string FunnyName { get; private set; }
    [field: SerializeField]public bool IsSelected { get; private set; }
    [field: SerializeField]public bool InDrag { get; private set; }
    [field: SerializeField]public Vector2 DragStartDelta { get; private set; }
    [field: SerializeField] public bool CanDrag { get; private set; } = false;

    private Image visual;
    private PersistentGameData data;
    private Canvas canvas;
    private Camera cam;
    
    private void Awake()
    {
        visual = GetComponent<Image>();
        if (!PersistentGameState.TryFindObject("Data", out PersistentGameData data)) enabled = false;
        this.data = data;
        canvas = GetComponentInParent<Canvas>();
        cam = Camera.main;
    }

    public void SetFunny(Funny funny)
    {
        Funny = funny;
        FunnyName = funny.name;
        visual.sprite = funny.FunnyArt;
    }

    public void RotateRight()
    {
        transform.rotation *= Quaternion.Euler(0,0,-10);
    }

    public void RotateLeft()
    {
        transform.rotation *= Quaternion.Euler(0,0,10);
    }

    public void ZoomIn()
    {
        transform.localScale *= 1.1f;
    }
    
    public void ZoomOut()
    {
        transform.localScale /= 1.1f;
    }

    public void Nudge(Vector3 direction)
    {
        transform.position += direction * 0.5f * transform.localScale.x;
    }

    public void OnPress()
    {
        if (!IsSelected)
        {
            SceneBuilder.SelectFunny(this);
        }
    }

    private void Update()
    {
        if (InDrag)
        {
            if (!Input.GetMouseButton(0))
            {
                InDrag = false;
                Debug.Log("DragEnd");
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(((RectTransform)canvas.transform), Input.mousePosition, cam, out Vector2 mouseCanvasPos);
                transform.localPosition = Vector3.Lerp(transform.localPosition,mouseCanvasPos + DragStartDelta,10f * Time.deltaTime);
            }
        }
        else
        {
            if (IsSelected && Input.GetMouseButton(0))
            {
                if (CanDrag)
                {
                    InDrag = true;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(((RectTransform)canvas.transform), Input.mousePosition, cam, out Vector2 mouseCanvasPos);
                    DragStartDelta = new Vector2(transform.localPosition.x,transform.localPosition.y) - mouseCanvasPos;
                }
            }
        }
    }

    public void Deselect()
    {
        visual.material = data.FunnyDefaultMaterial;
        IsSelected = false;
    }

    public void Select()
    {
        visual.material = data.FunnySelectedMaterial;
        IsSelected = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CanDrag = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanDrag = true;
    }
}
