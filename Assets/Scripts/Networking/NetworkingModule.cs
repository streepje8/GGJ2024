using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Riptide;
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

public struct ServerData
{
    public bool success;
    public string ip;
    public string hostname;
    public int playerCount;

    public ServerData(bool success, string ip = "null", string hostname = "null", int playerCount = 0)
    {
        this.success = success;
        this.ip = ip;
        this.hostname = hostname;
        this.playerCount = playerCount;
    }
}

public class NetworkingModule : PersistentObject
{
    public override string ID => "Networking";

    public override int ExecutionID => -1;

    public ConnectionState connectionState = ConnectionState.Disconnected;
    public NetworkMode networkMode = NetworkMode.Unknown;

    public Server Server { get; private set; } = new Server();
    public Client Client { get; private set; } = new Client();

    public bool ServerIsRunning { get; private set; } = false;
    public bool ClientIsRunning { get; private set; } = false;
    public bool IsYeeting { get; private set; } = false;
    public bool YeeterSetup { get; private set; } = false;
    public string LobbyName { get; private set; } = "Unknown Host";
    
    public static int broadcastPort = 7778;
    private UdpClient? broadcastYeeter;
    private UdpClient? broadcastYoinker;
    private static string ip = "err";
    private static Timer packetYeeter = new Timer(500);
    private ServerData? dataBuffer = null;
    private bool gotServerData = false;
    private int gotPlayerCount = 0;
    private string gotHostName = "";
    private string gotIp = "";
    
    public override void Create()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
    }

    private void OnApplicationQuit()
    {
        broadcastYoinker?.Close();
        broadcastYeeter?.Close();
        Server?.Stop();
        Client?.Disconnect();
    }

    public static IPAddress? GetLocalIPAddress()
    {
        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
    }
    
    public override void FixedUpdate()
    {
        switch (networkMode)
        {
            case NetworkMode.Host:
                if(ServerIsRunning)Server.Update();
                if(ClientIsRunning)Client.Update();
                break;
            case NetworkMode.Client:
                if(ClientIsRunning)Client.Update();
                break;
        }
    }

    private static bool isYoinking = false;
    
    public async Task<ServerData> TryReceiveIPAsync()
    {
        int count = 0;
        isYoinking = true;
        if(broadcastYoinker == null) broadcastYoinker = new UdpClient(7778);
#pragma warning disable CS4014
        YoinkBroadcasts(broadcastYoinker, (bytesYoinked) =>
        {
            try
            {
                int readPos = 0;
                int ipSize = BitConverter.ToInt32(bytesYoinked, readPos);
                readPos += 4;
                gotIp = Encoding.ASCII.GetString(bytesYoinked, readPos, ipSize);
                readPos += ipSize;
                int hostnameSize = BitConverter.ToInt32(bytesYoinked, readPos);
                readPos += 4;
                gotHostName = Encoding.ASCII.GetString(bytesYoinked, readPos, hostnameSize);
                readPos += hostnameSize;
                gotPlayerCount = BitConverter.ToInt32(bytesYoinked, readPos);
                readPos += 4;
                gotServerData = true;
            }
            catch (Exception e) { Debug.Log("Fuck :(");}
        }).ContinueWith(t =>
#pragma warning restore CS4014
        {
            if (t.IsFaulted) throw t.Exception ?? new Exception("Yoinker failed so hard that the exception was null!?");
        });
        while (!gotServerData && count < 60)
        {
            count++;
            await Task.Delay(100);
        }
        isYoinking = false;
        return new ServerData(gotServerData, gotIp, gotHostName, gotPlayerCount);
    }

    private static async Task YoinkBroadcasts(UdpClient client, Action<byte[]> onBroadcastYoinked)
    {
        while (isYoinking)
        {
            UdpReceiveResult receiveResult = await client.ReceiveAsync();
            byte[] receivedBytes = receiveResult.Buffer;
            onBroadcastYoinked.Invoke(receivedBytes);
        }
    }

    public async Task HostAsync()
    {
        if (networkMode != NetworkMode.Unknown) throw new Exception("Goofy af network mode found, should not happen.");
        networkMode = NetworkMode.Host;
        Server.Start(7777, 5);
        ServerIsRunning = true;
        if(broadcastYeeter == null) broadcastYeeter = new UdpClient(7779);
        if (!IsYeeting)
        {
            IsYeeting = true;
            if (!YeeterSetup)
            {
                ip = GetLocalIPAddress()?.ToString() ?? "err";
                YeeterSetup = true;
                packetYeeter.Elapsed += YeetPacket;
                packetYeeter.AutoReset = true;

            }   
            packetYeeter.Enabled = true;
        }
    }

    private void YeetPacket(object? sender, ElapsedEventArgs elapsedEventArgs)
    {
        IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);

        List<byte> bytes = new List<byte>();
        bytes.AddRange(BitConverter.GetBytes(ip.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(ip));
        bytes.AddRange(BitConverter.GetBytes(LobbyName.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(LobbyName));
        bytes.AddRange(BitConverter.GetBytes(Server.ClientCount));
        YeetBytesUDP(broadcastEndPoint,bytes);
    }
    
    public void YeetBytesUDP(IPEndPoint? clientEndPoint, List<byte> buffer)
    {
        if (IsYeeting)
        {
            if (clientEndPoint != null)
            {
                if (broadcastYeeter == null) throw new Exception($"Fuck, failed to YEET Bytes to {clientEndPoint}. UDPListener == null!");
                broadcastYeeter.BeginSend(buffer.ToArray(), buffer.Count, clientEndPoint, null, null);
            }
        }
    }

    public async Task ConnectToServerAsync(string ip)
    {
        if (networkMode == NetworkMode.Unknown) networkMode = NetworkMode.Client;
        Client.Connect($"{ip}:{7777}");
        ClientIsRunning = true;
    }
}
