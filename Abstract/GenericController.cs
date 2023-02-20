using Godot;
using System;



public class GenericController : Node2D
{
    

    protected Entity entity;

    protected short packet;
    public override void _Ready()
    {
        entity = (Entity)this.GetParent();
    }


}
