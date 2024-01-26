using UnityEngine;
public abstract class PersistentObject : MonoBehaviour
{
    public virtual string ID { get; private set; } = "NULL";
    public virtual int ExecutionID { get; private set; } = 0;
    public virtual void Create() {}
    public virtual void Update() {}
    public virtual void Destroy() {}
    public virtual void FixedUpdate() {}
}
