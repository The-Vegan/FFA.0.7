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

    public LineEdit nameBox;
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

    //Network
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    private LocalClient client = null;
    private HostServer server = null;

    public void ConnectToServer(string ipEndpoint)
    {
        client = new LocalClient(ipEndpoint, 1404);
        if (client.ConnectClient())
        {
            
            

            isMultiplayer = true;
            GD.Print("Successful Connexion");
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
        Theme connected = GD.Load<Theme>("res://UIAndMenus/ServerAndClientConfig/ConnectedLabel.tres");
        Theme disconnected = GD.Load<Theme>("res://UIAndMenus/ServerAndClientConfig/DisconnectedLabel.tres");
        //Make every labels theme "disconnected"
        for (byte i = 0; i < playerListBox.GetChildCount(); i++) playerListBox.GetChild<Label>(i).SetTheme(disconnected);
        //Finds the label corresponding to players and sets thier theme to "connected"
        for (byte i = 0; i < playerList.Count; i++)
        {
            playerListBox.GetChild<Label>(playerList[i].scafholdClientID).Text = ((i+1) + " : " + playerList[i].name);
            playerListBox.GetChild<Label>(playerList[i].scafholdClientID).SetTheme(connected);
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

    public async void HostGame()
    {
        string locIP = Dns.GetHostName();
        //create the server
        this.server = new HostServer(locIP, 1404);
        //create the client to connect to itself
        this.ConnectToServer(locIP);
        //Changes the back button to reset networkConfig
        Button backFromModeSelect = GetNode("SoloMenu/Back") as Button;
        backFromModeSelect.SetScript(GD.Load<Reference>("res://UIAndMenus/ServerAndClientConfig/ResetNetworkConfigButton.cs"));
        
        await ToSignal(GetTree(), "idle_frame");
        GD.Print("[MainMenu] one frame since script changed");
        //if(backFromModeSelect == null) GD.Print(null);
        //else GD.Print(backFromModeSelect);

        //((ResetNetworkConfigButton)backFromModeSelect).InitMainMenu(this);
        //GD.Print("[MainMenu] Script changed to fit SERVER");

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

        
        resetNetworkConfigForm = GetNode("Camera2D/CanvasLayer/ResetNetworkConfigForm") as Sprite;
        GD.Print("[MainMenu] networkForm is " + resetNetworkConfigForm);
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
                nameBox.Visible = true;
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
