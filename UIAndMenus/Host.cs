using Godot;
using System;

public class Host : Button
{

    MainMenu menu;

    public override void _Ready()
    {
        menu = GetParent().GetParent() as MainMenu;
    }

    public override void _Pressed()
    {
        menu.HostGame();
        menu.MoveCameraTo(1);
    }
}
