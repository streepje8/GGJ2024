using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [field: SerializeField]
    public string DefaultScreen { get; private set; }
    private Dictionary<string, UIScreen> screens = new Dictionary<string, UIScreen>();
    private void Awake()
    {
        foreach (var screen in GetComponentsInChildren<UIScreen>(true))
        {
            screens.Add(screen.ID.ToLower(),screen);
        }
        ShowScreen(DefaultScreen);
    }

    public bool ShowScreen(string id)
    {
        id = id.ToLower();
        if (TryGetScreen(id, out UIScreen screen))
        {
            foreach (var uiScreen in screens)
            {
                uiScreen.Value.gameObject.SetActive(false);
            }
            screen.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    public bool TryGetScreen(string id, out UIScreen screen)
    {
        id = id.ToLower();
        if (screens.TryGetValue(id, out UIScreen value))
        {
            screen = value;
            return true;
        }
        screen = null;
        return false;
    }
}
