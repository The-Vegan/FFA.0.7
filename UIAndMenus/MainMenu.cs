//using FFA.Empty.Empty.ServerAndNetwork;
using FFA.Empty.Empty.ServerAndNetwork;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class MainMenu : Control
{
    //Nodes
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected Camera2D camera;
    protected VBoxContainer playerListBox;

    protected Label ipLineEditMessage;
    public byte postCharacterDestination = 3;

    private LineEdit nameBox;
    public Sprite resetNetworkConfigForm;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Nodes

    //Level Instancing Values
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    private PackedScene levelToLoad;
    public void SetLevel(PackedScene lvl) 
    {
        if(lvl == null) return;
        levelToLoad =lvl;
 
    }
    public byte gameMode = 0;
    public byte playerCharacter = 0;
    public byte teams = 1;
    public byte chosenTeam = 0;
    public byte numberOfEntities = 12;
    public byte numberOfPlayers = 1;
    public bool isMultiplayer = false;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Level Instancing Values
    //Debug
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    [Export]
    public byte[] lastDataIn;
    [Export]
    public byte[] lastDataOut;

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Debug
    //Network
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    private LocalClient client = null;
    private HostServer server = null;

    public void ConnectToServer(string ipEndpoint)
    {
        GD.Print("[MainMenu] Creating  ,:Client");
        client = new LocalClient(ipEndpoint, 414);
        if (client.ConnectClient())
        {
            client.InitParent(this);
            isMultiplayer = true;
            GD.Print("[MainMenu] Successful Connexion , multiplayer is :" + isMultiplayer);
            client.SetName(this.nameBox.Text);
            if (server == null)
            {
                Button backFromIpForm = GetNode("ConnectToServer/Back") as Button;
                backFromIpForm.SetScript(GD.Load<Reference>("res://UIAndMenus/ServerAndClientConfig/ResetNetworkConfigButton.cs"));//Makes the "Back" button destroy the client
                
                GD.Print("[MainMenu] Script changed to fit CLIENT");

                MoveCameraTo(2);//Goes to CHARSELECT
                postCharacterDestination = 6;
            }
        }
        else
        {
            ipLineEditMessage.Text = "Could not connect to server";
        }
    }

    public void SetMultiplayerName(string name)
    {
        if(client != null) client.SetName(name);
    }
    public void DisplayPlayerList(List<ScafholdEntity> playerList)
    {
        GD.Print("[MainMenu] playerListCount : " + playerList.Count);
        //Finds the label corresponding to players and sets thier theme to "connected"
        for (byte i = 0; i < playerList.Count; i++)
        {
            CheckButton playerLabel = playerListBox.GetChild<CheckButton>(playerList[i].scafholdClientID - 1);
            playerLabel.Pressed = true;
            playerLabel.Text = playerList[i].name;
            GD.Print("[MainMenu] changed button " + playerList[i].scafholdClientID);
        }
    }

    public void Countdown(byte sec)
    {
        //if valid argument, makes it visible
        //else makes it invisible
        if (sec != 0 && sec < 4) this.GetNode<Label>("WaitForPlayers/Countdown").Visible = true;
        else this.GetNode<Label>("WaitForPlayers/Countdown").Visible = false;
        this.GetNode<Label>("WaitForPlayers/Countdown").Text = sec + "   ";
    }

    public void HostGame()
    {
        GD.Print("[MainMenu] HostGame called");
        string locIP = Dns.GetHostName();
        //create the server
        this.server = new HostServer(locIP, 414);
        GD.Print("[MainMenu] Server created on IP : " + locIP + ":414");
        //create the client to connect to itself
        this.ConnectToServer(locIP);
        //Changes the back button to reset networkConfig
        Button backFromModeSelect = GetNode("SoloMenu/Back") as Button;
        backFromModeSelect.SetScript(GD.Load<Reference>("res://UIAndMenus/ServerAndClientConfig/ResetNetworkConfigButton.cs"));


        GD.Print("[MainMenu] Script changed to fit SERVER");

        postCharacterDestination = 3;
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Network

    //Camera Position
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected List<Vector2> back = new List<Vector2>() { new Vector2(0, 0) };

    protected Vector2 MAINMENU = new Vector2(0, 0);
    protected Vector2 SOLO = new Vector2(0, -576);
    protected Vector2 CHARSELECT = new Vector2(-1024, 0);
    protected Vector2 LEVELSELECT = new Vector2(-2048, 0);
    protected Vector2 MULTI = new Vector2(0, 576);
    protected Vector2 JOIN = new Vector2(-1024, 576);
    protected Vector2 WAITFOROTHERS = new Vector2(-3072, 0);

    public override void _Ready()
    {
        camera = this.GetNode("Camera2D") as Camera2D;
        
        playerListBox = this.GetNode("WaitForPlayers/VBoxContainer") as VBoxContainer;
        ipLineEditMessage = this.GetNode("ConnectToServer/IpTextBox/Label") as Label;
        nameBox = GetNode("Camera2D/CanvasLayer/Namebox") as LineEdit;
        
        resetNetworkConfigForm = GetNode("Camera2D/CanvasLayer/ResetNetworkConfigForm") as Sprite;
    }

    public void MoveCameraTo(sbyte destination)
    {
        if(destination == -1)
        {
            camera.Position = back[back.Count - 1];
            back.RemoveAt(back.Count - 1);

            if (back.Count == 0) back.Add(MAINMENU);
            return;
        }

        back.Add(camera.Position);
        nameBox.Visible = false;
        switch (destination)
        {
            case 0://MainMenu                                                          
                camera.Position = MAINMENU;
                break;
            case 1://ModeSelect
                camera.Position = SOLO;
                break;
            case 2://Character Select
                camera.Position = CHARSELECT;
                if (isMultiplayer)
                {
                    nameBox.Visible = true;
                }
                else GD.Print("[MainMenu] Keeping namebox invisible");
                break;
            case 3://Level Select
                camera.Position = LEVELSELECT;
                break;
            case 4://Multi
                camera.Position = MULTI;
                break;
            case 5://Join
                camera.Position = JOIN;
                break;
            case 6://WaitForHostToStartGame
                camera.Position = WAITFOROTHERS;
                break;
            default://Returns to MainMenu in case of error
                GD.Print("[MainMenu] Invalid destination for camera :" +  destination);
                MoveCameraTo(0);
                back = new List<Vector2>() {new Vector2(0,0) };
                ResetNetwork();
                break;
        }
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Camera Position
    public void SetGame(byte mode)
    {
        this.gameMode = mode;
        GD.Print("[MainMenu] gameMode set to : " + gameMode);
    }

    public void SetCharacter(byte id)
    {
        if(client != null)
        {
            client.SetChar(id);
        }
        this.playerCharacter = id;
    }
    public void ShowNetworkForm()
    {
        this.resetNetworkConfigForm.Visible = true;
        GetTree().CallGroup("MenuButton", "_Set", "enable", false);
    }

    public void ResetNetwork()
    {
        GD.Print("[MainMenu] ResetNetworkConfig Reset");
        if(client != null)
        {
            client.ShutDown();
            client = null;
        }
        if(server != null)
        {
            server.ShutDown();
            server = null;
        }

        Reference script = GD.Load<Reference>("res://UIAndMenus/DestinationButton/Back.gd");

        Button backFromIpForm = GetNode("ConnectToServer/Back") as Button;
        backFromIpForm.SetScript(script);
        Button backFromModeSelect = GetNode("SoloMenu/Back") as Button;
        backFromModeSelect.SetScript(script);

        resetNetworkConfigForm.Visible = false;
        isMultiplayer = false;
        GD.Print("[MainMenu] set multiplayer to :" + false);
        postCharacterDestination = 3;
        GC.Collect();
    }

    public void LaunchGameMultiplayer()
    {
        Level loadedLevel = levelToLoad.Instance() as Level;
        if (loadedLevel == null) throw new Exception("Couldn't load Level from MainMenu");

        if(server != null)
        {
            List<ScafholdEntity> entities = new List<ScafholdEntity>();
            string[] keys = server.ipToEntity.Keys.ToArray();
            for(byte i = 0; i < keys.Length; i++)
            {
                entities.Add(server.ipToEntity[keys[i]]);
            }
            


            GetTree().Root.AddChild(loadedLevel);
            this.QueueFree();
        }

        loadedLevel.InitPlayerAndMode(
            this.playerCharacter,
            this.gameMode,
            this.numberOfEntities,
            this.teams,
            this.chosenTeam);
    }
    
}
