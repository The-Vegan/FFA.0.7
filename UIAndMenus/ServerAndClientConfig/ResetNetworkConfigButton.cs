using Godot;
using System;

public class ResetNetworkConfigButton : Button
{
    private MainMenu mm;
    public void InitMainMenu(MainMenu menu) {mm = menu;}

    public override void _Pressed()
    {
        mm.resetNetworkConfigForm.Visible = true;

        mm.resetNetworkConfigForm.GetChild(0).Connect("pressed", this, "Accepted");
        mm.resetNetworkConfigForm.GetChild(1).Connect("pressed", this, "Refused");
        GetTree().CallGroup("MenuButton", "_set", "disabled", true);
    }
    public void Accepted()
    {
        mm.ResetNetwork();
        mm.MoveCameraTo(-1);
        GetTree().CallGroup("MenuButton", "_set", "disabled", false);
    }

    public void Refused()
    {
        mm.resetNetworkConfigForm.Visible = false;
        GetTree().CallGroup("MenuButton", "_set", "disabled", false);
    }

}
