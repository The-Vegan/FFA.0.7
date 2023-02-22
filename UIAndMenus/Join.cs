using Godot;
using System;
using System.Text.RegularExpressions;

public class Join : Button
{
    private MainMenu menu;
    private LineEdit ipTextbox;
    public override void _Ready()
    {
        menu = GetParent().GetParent() as MainMenu;
        ipTextbox = GetParent().GetNode("IpTextBox") as LineEdit;
    }
    public override void _Pressed()
    {
        Regex regex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

        if (!regex.IsMatch(ipTextbox.Text))
        {
            Label l = ipTextbox.GetNode("Label") as Label;
            l.Text = "Invalid IP adress";
            return;
        }//if regex ip
        GD.Print("Ip valide");
        menu.ConnectToServer(ipTextbox.Text.Trim());

        

    }
}
