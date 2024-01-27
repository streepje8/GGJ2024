using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(order = 0,fileName = "New Funny", menuName = "hihi/funny")]
public class Funny : ScriptableObject
{
    [field: SerializeField] public string FunnyName { get; private set; }
    [field: SerializeField] public Sprite FunnyArt { get; private set; }
}
