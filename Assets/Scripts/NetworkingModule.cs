using Riptide.Utils;
using UnityEngine;

public enum ConnectionState
{
    Disconnected,
    Connecting,
    Connected
}

public enum NetworkMode
{
    Unknown,
    Host,
    Client
}

public class NetworkingModule : PersistentObject
{
    public override string ID => "Networking";

    public override int ExecutionID => -1;

    public ConnectionState connectionState = ConnectionState.Disconnected;
    public NetworkMode networkMode = NetworkMode.Unknown;

    public override void Create()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
    }
}
