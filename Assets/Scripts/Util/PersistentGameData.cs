using UnityEngine;

public class PersistentGameData : PersistentObject
{
    public override string ID => "Data";
    [field: SerializeField] public FunnySet FunnySet { get; private set; }
    [field: SerializeField] public FunnyPrompts FunnyPrompts { get; private set; }
}
