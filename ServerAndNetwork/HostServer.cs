using Godot;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFA.Empty.Empty.ServerAndNetwork
{
    public class HostServer
    {
        private SimpleTcpServer server;

        private Random rd = new Random();

        private MainMenu menu;
        private Level map;

        public void InitParent(MainMenu isItMenu)
        {
            menu = isItMenu ?? throw new Exception("Menu is null in initParent");
            map = null;
        }
        public void InitParent(Level isItMap)
        {
            map = isItMap ?? throw new Exception("Level is null in initParent");
            menu = null;
        }

        public bool running = false;
        public bool isLaunched = false;
        public bool abortLaunch = true;
        //Packet constants
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        /*CLIENT -> SERVER*/
        private const byte MOVE = 1;
        private const byte SET_CHARACTER = 2;
        private const byte CHOSE_TEAM = 3;
        /*SERVER -> CLIENT*/
        //Pre launch
        private const byte ABOUT_TO_LAUNCH = 255;
        private const byte ABORT_LAUNCH = 254;
        private const byte LAUNCH = 253;
        private const byte SET_CLIENT_OR_ENTITY_ID = 252;
        private const byte SEND_NAME_LIST = 251;
        private const byte SET_LEVEL_CONFIG = 250;
        //Post launch
        private const byte GAME_OVER = 249;
        private const byte GAME_SOON_OVER = 248;
        private const byte SET_MOVES = 247;
        private const byte SYNC = 246;
        private const byte ITEM_GIVEN_BY_SERVER = 245;
        private const byte BLUNDERED_BY_SERVER = 244;
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Packet constants

        //Players
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        private string[] listOfIPs = new string[16];

        private const byte numberOfPlayers = 16;
        private byte connected = 0;
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Players

        //Values
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        public Dictionary<string, ScafholdEntity> ipToEntity = new Dictionary<string, ScafholdEntity>();
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Values
        //Automated work
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        public HostServer(string ip, ushort port)
        {
            server = new SimpleTcpServer(ip + ":" + port);
            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.DataReceived += DataRecieved;
            server.Start();
            running = true;
        }

        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            if (isLaunched)
            {
                GD.Print("[HostServer] Client Denied : Server is launched");
                server.DisconnectClient(e.IpPort);
                return;
            }
            if (connected == numberOfPlayers)
            {
                GD.Print("[HostServer] Client Denied : SERVER FULL");
                server.DisconnectClient(e.IpPort);
                return;
            }
            abortLaunch = true;
            AddClientToIPList(e.IpPort);
            UpdateAllPlayers();
        }
        private void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            if (listOfIPs.Contains(e.IpPort))
            {
                abortLaunch = true;
                RemoveClientFromIPList(e.IpPort);
                UpdateAllPlayers();
            }
        }
        private void AddClientToIPList(string ipPort)
        {
            for (byte i = 0; i < listOfIPs.Length; i++)
            {
                if (listOfIPs[i] == null)
                {
                    listOfIPs[i] = ipPort;
                    server.Send(listOfIPs[i], new byte[] { SET_CLIENT_OR_ENTITY_ID, (byte)(i + 1), 0 });
                    ipToEntity.Add(ipPort, new ScafholdEntity { scafholdClientID = (byte)(i + 1) });
                    break;
                }
            }
            connected++;
            GD.Print("[HostServer] Client Connected");
        }
        private void RemoveClientFromIPList(string ipPort)
        {
            for (byte i = 0; i < listOfIPs.Length; i++)
            {
                if (ipPort == listOfIPs[i])
                {
                    GD.Print("[HostServer] Client Disconnected");
                    listOfIPs[i] = null;
                    ipToEntity.Remove(ipPort);
                    connected--;
                }
                else if (i == listOfIPs.Length) GD.Print("[HostServer] Client Not found");
            }
            for (byte i = 0; i < listOfIPs.Length; i++)
            {
                if (listOfIPs[i] == null) continue;
                if (ipToEntity[listOfIPs[i]].name == null) { GD.Print("[HostServer] " + (i + 1) + " : UNNAMED"); continue; }
                GD.Print("[HostServer] " + (i + 1) + "\t: " + ipToEntity[listOfIPs[i]].name);
            }
        }
        
        private void DataRecieved(object sender, DataReceivedEventArgs e)
        {
            byte[] data = e.Data.Array;

            switch (data[0])
            {
                case MOVE:
                    GD.Print("Move");
                    break;
                case SET_CHARACTER:
                    ScafholdEntity se = ipToEntity[e.IpPort];
                    se.scafholdEntityID = data[1];
                    if (data[2] == 0) break;

                    se.name = UnicodeEncoding.Unicode.GetString(data, 3, data[2] * 2);
                    ipToEntity[e.IpPort] = se;

                    UpdateAllPlayers();
                    break;
                case CHOSE_TEAM:
                    GD.Print("Team");
                    break;
            }
        }
        public void ShutDown()
        {
            server.Stop();
            running = false;
            isLaunched = false;
        }

        private void UpdateAllPlayers()
        {
            List<byte> stream = new List<byte>() { SEND_NAME_LIST };
            for(byte i = 0; i < listOfIPs.Length; i++)
            {
                
                if (listOfIPs[i] == null) continue;
                GD.Print("[HostServer] ip " + i + " is " + listOfIPs[i]);
                ScafholdEntity se = ipToEntity[listOfIPs[i]];
                stream.Add(se.scafholdClientID);
                stream.Add(se.scafholdEntityID);
                
                if (se.name == null) se.name = "UNNAMED";
                byte[] nameAsByte = Encoding.Unicode.GetBytes(se.name);
                GD.Print("[HostServer] id = " + se.scafholdClientID + ", char = " + se.scafholdEntityID + ", name = " + se.name);
                stream.Add((byte)nameAsByte.Length);
                for (byte j = 0; j < nameAsByte.Length; j++) stream.Add(nameAsByte[j]);
            }

            byte[] outStream = stream.ToArray();
            foreach (string ip in listOfIPs)
            {
                if (ip == null) continue;
                server.Send(ip, outStream);
            }
        }
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Automated work
        
        //Send Data to clients
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        public void SendMoves(Dictionary<byte, PacketPlusTiming> idToPacket)
        {
            byte[] keys = idToPacket.Keys.ToArray();

            ushort allocLength = (ushort)(2 + (keys.Length * (1 + 2 + 4)));
            //first byte for the instruction //second byte for number of entities
            //Every entity stored one after the other following following pattern :
            //1byte : ID // 2byte HP (short) // 4 byte :timeing (float)
            byte[] outStream = new byte[allocLength];

            outStream[0] = SET_MOVES;
            outStream[1] = (byte)keys.Length;

            for (byte i = 0; i < keys.Length; i++)
            {
                //Set ID
                outStream[(i * 7) + 2] = keys[i];
                //Set Packet
                PacketPlusTiming packet = idToPacket[keys[i]];
                outStream[(i * 7) + 3] = (byte)(packet.packet >> 8);
                outStream[(i * 7) + 4] = (byte)packet.packet;
                //Set Timeing
                byte[] floatAsByte = BitConverter.GetBytes(packet.timing);
                outStream[(i * 7) + 5] = floatAsByte[0];
                outStream[(i * 7) + 6] = floatAsByte[1];
                outStream[(i * 7) + 7] = floatAsByte[2];
                outStream[(i * 7) + 8] = floatAsByte[3];
            }

            foreach (string ip in listOfIPs)
            {
                if (ip == null) continue;
                server.Send(ip, outStream);
            }
        }
        public void SendSync(Dictionary<byte, VariableSyncPacket> idToCharSync)
        {
            byte[] keys = idToCharSync.Keys.ToArray();

            ushort allocLength = (ushort)(2 + (keys.Length * 7));
            byte[] outStream = new byte[allocLength];

            outStream[0] = SYNC;
            outStream[1] = (byte)keys.Length;

            for (byte i = 0; i < keys.Length; i++)
            {
                VariableSyncPacket sync = idToCharSync[keys[i]];

                //Set ID
                outStream[(i * 7) + 2] = keys[i];
                //Set Coords
                outStream[(i * 7) + 3] = sync.xCoord;
                outStream[(i * 7) + 4] = sync.yCoord;
                //Set HP
                outStream[(i * 7) + 5] = (byte)(sync.hp >> 8);
                outStream[(i * 7) + 6] = (byte)sync.hp;
                //Set Item
                outStream[(i * 7) + 7] = sync.item;
                //Set Blunder
                outStream[(i * 7) + 8] = sync.blunder;
            }
            foreach (string ip in listOfIPs)
            {
                if (ip == null) continue;
                server.Send(ip, outStream);
            }
        }

        public bool SendStart()
        {
            abortLaunch = false;

            foreach (string ip in listOfIPs)
            {
                if (ip == null) continue;
                server.Send(ip, new byte[] { ABOUT_TO_LAUNCH });
            }
            GD.Print();
            GD.Print("Launches in ");
            for (byte i = 3; i != 0; i--)
            {
                menu.Countdown(i);

                for(byte j = 0; j < 30; j++)//Waits one frame 30 times
                {
                    System.Threading.Thread.Sleep(33);
                    if (abortLaunch) break;//If launch is stopped, stops
                }
                if (abortLaunch)  break; 
            }
            if (!abortLaunch)
            {
                foreach (string ip in listOfIPs)
                {
                    if (ip == null) continue;
                    if (ipToEntity[ip].scafholdEntityID == 0) server.Send(ip, new byte[] { SET_CLIENT_OR_ENTITY_ID, ipToEntity[ip].scafholdClientID, (byte)rd.Next(1, 4) });

                    server.Send(ip, new byte[] { LAUNCH });
                }
                isLaunched = true;
                abortLaunch = true;

                return true;
            }
            else
            {
                menu.Countdown(0);
                foreach (string ip in listOfIPs)
                {
                    if (ip == null) continue;
                    server.Send(ip, new byte[] { ABORT_LAUNCH });
                }
                return false;
            }
        }

        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Send Data to clients
    }
}
