using UnityEngine;

[CreateAssetMenu(order = 0,fileName = "New Funny", menuName = "hihi/funny")]
public class Funny : ScriptableObject
{
    [field: SerializeField] public string FunnyName { get; private set; }
    [field: SerializeField] public Sprite FunnyIcon { get; private set; }
    [field: SerializeField] public Sprite FunnyCursorIcon { get; private set; }
    [field: SerializeField] public GameObject FunnyPrefab { get; private set; }
}
