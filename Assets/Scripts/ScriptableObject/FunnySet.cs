using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1,fileName = "New Funny Set", menuName = "hihi/funnyset")]
public class FunnySet : ScriptableObject
{
    [field: SerializeField] public List<Funny> Funnies { get; private set; } = new List<Funny>();
}
