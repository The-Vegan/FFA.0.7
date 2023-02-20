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
    }
    public void Accepted()
    {
        mm.ResetNetwork();
        mm.MoveCameraTo(-1);
    }

    public void Refused()
    {
        mm.resetNetworkConfigForm.Visible = false;
    }

}
