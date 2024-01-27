using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2,fileName = "New Funny Prompts", menuName = "hihi/funnyprompts")]
public class FunnyPrompts : ScriptableObject
{
    [field: SerializeField]
    public List<string> Prompts { get; private set; } = new List<string>();
}
