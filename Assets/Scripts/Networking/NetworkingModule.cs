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
    public static int PLAYERCOUNT_CLIENT = 0;

    [MessageHandler(1)]
    private static void ClientJoinMessage(Message message)
    {
        PLAYERCOUNT_CLIENT = message.GetInt();
    }

    public int PLAYERCOUNT_SERVER => Server.ClientCount;

    public int PLAYERCOUNT
    {
        get { return (networkMode == NetworkMode.Host) ? PLAYERCOUNT_SERVER : PLAYERCOUNT_CLIENT; }
    }

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
    public string ReceivedLobbyName { get; set; } = "";

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
        Server.ClientConnected += (e, args) =>
        {
            Message mes = Message.Create(MessageSendMode.Reliable, 1);
            mes.Add(Server.ClientCount);
            Server.SendToAll(mes);
        };
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

    public void SetLobbyName(string newName)
    {
        LobbyName = newName;
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
        if(broadcastYeeter == null) broadcastYeeter = new UdpClient(21);
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

    public void SubmitFrames(List<byte[]> images)
    {
        Message payload = Message.Create(MessageSendMode.Reliable, 3);
        string guid = Guid.NewGuid().ToString();
        payload.Add(guid);
        payload.Add((byte)0); //NEW PAYLOAD
        Client.Send(payload);
        int img = 0;
        foreach (var image in images)
        {
            int bufferSize = 700;
            for (int i = 0; i < image.Length; i += bufferSize)
            {
                int remainingLength = Math.Min(bufferSize, image.Length - i);
                byte[] buffer = new byte[remainingLength];
                Array.Copy(image, i, buffer, 0, remainingLength);

                Message imageData = Message.Create(MessageSendMode.Reliable, 3);
                imageData.Add(guid);
                imageData.Add((byte)1); // Continue PAYLOAD
                imageData.Add((byte)img);
                imageData.Add(buffer);
                Client.Send(imageData);
            }

            Message endImg = Message.Create(MessageSendMode.Reliable, 3);
            endImg.Add(guid);
            endImg.Add((byte)2); // Image End PAYLOAD
            endImg.Add((byte)img);
            Client.Send(endImg);

            img++;
        }
        SendEndDelayed(guid).ContinueWith(t =>
        {
            if (t.IsFaulted) throw t.Exception ?? new Exception("x_x");
        });
    }

    private async Task SendEndDelayed(string guid)
    {
        await Task.Delay(6000);
        Message endTotal = Message.Create(MessageSendMode.Reliable, 3);
        endTotal.Add(guid);
        endTotal.Add((byte)3); //End PAYLOAD
        Client.Send(endTotal);
    }

    public static Dictionary<string, List<List<byte>>> images = new Dictionary<string, List<List<byte>>>();

    [MessageHandler(3)]
    private static void OnReceivePayload(ushort fromClient,Message message)
    {
        string clientGuid = message.GetString();
        switch ((int)message.GetByte())
        {
            case 0: //NEW PAYLOAD
                images.Add(clientGuid, new List<List<byte>>() { new(), new(), new()});
                break;
            case 1: //Continue PAYLOAD
                int img = (int)message.GetByte();
                List<List<byte>> imgs = images[clientGuid];
                List<byte> current = imgs[img];
                current.AddRange(message.GetBytes());
                imgs[img] = current;
                images[clientGuid] = imgs;
                break;
            case 2: //Image End PAYLOAD

                break;
            case 3: //End PAYLOAD
                List<List<byte>> yoinked = images[clientGuid];
                images.Remove(clientGuid);
                Texture2D texA = new Texture2D(2, 2);
                texA.LoadImage(yoinked[0].ToArray());
                Texture2D texB = new Texture2D(2, 2);
                texB.LoadImage(yoinked[1].ToArray());
                Texture2D texC = new Texture2D(2, 2);
                texC.LoadImage(yoinked[2].ToArray());
                GamejamSolutions.instance.textures.Add(new List<Texture2D>() {texA,texB,texC});
                Debug.Log("NICE");
                GamejamSolutions.instance.OnReceive();
                break;
        }
    }
}
