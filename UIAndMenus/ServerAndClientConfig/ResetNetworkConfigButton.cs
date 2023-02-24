using Godot;
using System;

public class ResetNetworkConfigButton : Button
{
    private MainMenu mm;
    

    public override void _Pressed()
    {
        mm = this.GetParent().GetParent() as MainMenu;
        if (mm == null)
        {
            GD.Print("Menu is null");
            return;
        }

        mm.resetNetworkConfigForm.Visible = true;

        Button yes = mm.resetNetworkConfigForm.GetChild(0) as Button;
        Button no = mm.resetNetworkConfigForm.GetChild(1) as Button;

        yes.Connect("pressed", this, "Accepted");
        no.Connect("pressed", this, "Refused");
        GetTree().CallGroup("MenuButton", "set", "disabled", true);

        GD.Print("[ResetNetworkConfigButton] buttons set to Disable = true");
    }
    public void Accepted()
    {
        if (mm == null)
        {
            GD.Print("Menu is null");
            return;
        }
        //avant c'était ici
        mm.MoveCameraTo(-1);
        GetTree().CallGroup("MenuButton", "set", "disabled", false);
        GD.Print("[ResetNetworkConfigButton] buttons set to Disable = false");
        //mtn c'est là
        mm.ResetNetwork();
    }

    public void Refused()
    {
        mm.resetNetworkConfigForm.Visible = false;
        GetTree().CallGroup("MenuButton", "set", "disabled", false);
        GD.Print("[ResetNetworkConfigButton] buttons set to Disable = false");
    }

}
