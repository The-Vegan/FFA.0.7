using Godot;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFA.Empty.Empty.ServerAndNetwork
{
    public class LocalClient
    {
        private SimpleTcpClient client;

        public bool running = false;

        private byte clientID = 0;
        private byte charID = 0;

        private MainMenu menu;
        private Level map;

        private List<ScafholdEntity> playerList;

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




        //Basics
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        public LocalClient(string ip, ushort port)
        {

            client = new SimpleTcpClient(ip + ":" + port);
            client.Events.Connected += ClientConnected;
            client.Events.Disconnected += ClientDisonnected;
            client.Events.DataReceived += ReceiveData;

        }

        public bool ConnectClient()
        {
            try
            {
                client.Connect();
                return running = true;
            }
            catch (Exception)
            {
                return running = false;
            }
        }

        public void ShutDown()
        {
            client.Disconnect();
            running = false;
        }

        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            GD.Print("[LocalClient] Connected to server");
        }

        private void ClientDisonnected(object sender, ConnectionEventArgs e)
        {
            GD.Print("[LocalClient] Disconnected from server");
        }

        public void SendData(byte[] data)
        {
            client.Send(data);
        }
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
        //Basics
        private void ReceiveData(object sender, DataReceivedEventArgs e)
        {
            byte[] data = e.Data.Array;

            switch (data[0])
            {
                case SET_CLIENT_OR_ENTITY_ID:
                    clientID = data[1];
                    GD.Print("Client ID assigned by server to " + clientID);
                    if (data[2] == 0) break;
                    charID = data[2];
                    GD.Print("Character assigned by server to " + charID);
                    break;
                case ABOUT_TO_LAUNCH:
                    GD.Print("Will Start soon");
                    break;
                case ABORT_LAUNCH:
                    GD.Print("nvm");
                    break;
                case LAUNCH:
                    GD.Print("Start signal recieved");
                    break;
                case SET_MOVES:
                    for (int i = 2; i < (2 + (data[1] * 7)); i += 7)
                    {
                        byte id = data[i];

                        short packet = (short)((data[i + 1] << 8) + data[i + 2]);

                        float timing = BitConverter.ToSingle(data, i + 3);

                        GD.Print("Entity " + id + " set pckt to " + packet + " with timing of " + timing);
                    }
                    break;
                case SYNC:
                    for (int i = 2; i < (2 + (data[1] * 7)); i += 7)
                    {
                        byte id = data[i];

                        byte xCoord = data[i + 1];
                        byte yCoord = data[i + 2];

                        short hp = (short)((data[i + 3] << 8) + data[i + 4]);

                        byte item = data[i + 5];
                        byte blunder = data[i + 6];

                        GD.Print("Entity " + id + " :\t" + xCoord + "x\t" + yCoord + "y :\t" + hp + " HP. item = " + item + ", blunder = " + blunder);
                    }
                    break;
                case SEND_NAME_LIST:
                    ushort offset = 1;
                    playerList = new List<ScafholdEntity>();
                    while((data[offset] != 0) && (offset < (data.Length)))
                    {
                        byte idOfClient = data[offset]; offset++;
                        byte stringLength = data[offset]; offset++;

                        string nametag = UnicodeEncoding.Unicode.GetString(data, offset, (stringLength * 2));
                        offset += (ushort)(stringLength * 2);
                        ScafholdEntity se = new ScafholdEntity()
                        {
                            scafholdClientID = idOfClient,
                            name = nametag
                        };
                        playerList.Add(se);
                    }
                    //TODO IN MENU
                    //menu.DisplayPlayerList(playerList);
                    break;
            }


        }

        public void SetName(string name)
        {
            byte[] stringAsBytes = UnicodeEncoding.Unicode.GetBytes(name);

            byte[] outstream = new byte[3 + stringAsBytes.Length];

            outstream[0] = SET_CHARACTER;
            outstream[1] = charID;
            outstream[2] = (byte)stringAsBytes.Length;
            for (int i = 0; i < stringAsBytes.Length; i++) outstream[i + 3] = stringAsBytes[i];

            SendData(outstream);
        }

        public void SetChar(byte id)
        {
            charID = id;
            byte[] outstream = new byte[3];

            outstream[0] = SET_CHARACTER;
            outstream[1] = charID;
            SendData(outstream);
        }

    }
}
