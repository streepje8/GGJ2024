using System.Collections.Generic;
using UnityEngine;

public class PersistentGameData : PersistentObject
{
    public override string ID => "Data";
    [field: SerializeField] public FunnySet FunnySet { get; private set; }
    [field: SerializeField] public FunnyPrompts FunnyPrompts { get; private set; }
    [field: SerializeField] public Material FunnyDefaultMaterial { get; private set; }
    [field: SerializeField] public Material FunnySelectedMaterial { get; private set; }
}
