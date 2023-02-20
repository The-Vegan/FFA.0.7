using Godot;
using System;

public class ServerController : GenericController
{

    public override void _Ready()
    {
        
    }
    public void SetPacket(short packetFromServer)
    {
        entity.SetPacket(packetFromServer);
    }
}
