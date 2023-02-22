using FFA.Empty.Empty.ServerAndNetwork;
using Godot;
using System;

public class LaunchMultiplayerButton : Button
{
    MainMenu menu;
    HostServer server;
    LocalClient client;
    PackedScene lvl;
    public override void _Ready()
    {
        
    }
    public void Init(PackedScene lvlToLoad,HostServer serv, LocalClient cli)
    {
        this.lvl = lvlToLoad;
        this.server = serv;
        this.client = cli;


    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
