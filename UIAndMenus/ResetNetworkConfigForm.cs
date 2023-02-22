using Godot;
using System;

public class ResetNetworkConfigForm : Sprite
{

    MainMenu menu;
    public override void _Ready()
    {
        menu = GetParent().GetParent() as MainMenu;
    }

    public void YesPressed()
    {
        menu.ResetNetwork();
        menu.MoveCameraTo(-1);
        GetTree().CallGroup("MenuButton", "_Set", "enable", true);
    }
    public void NoPressed()
    {
        this.Visible = false;
        GetTree().CallGroup("MenuButton", "_Set", "enable", true);
    }

}
