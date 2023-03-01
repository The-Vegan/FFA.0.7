using Godot;
using System;

public class Host : Button
{

    private MainMenu menu;

    public override void _Ready()
    {
        menu = GetParent().GetParent() as MainMenu;
    }

    public override void _Pressed()
    {
        menu.HostGame();
        GD.Print("[Host] Created HostServer");
        menu.MoveCameraTo(1);
        GD.Print("[Host] MoveCamTo ModeSelect");
    }
}
