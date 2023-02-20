using Godot;
using System;

public class KyomiraTexture : TileMap
{
    public byte state = 0;
    public override void _Ready()
    {
        
    }

    public void ChangeLighting(byte newLight)
    {
        if (state == newLight) return;

        sbyte textureIDOffset =(sbyte)(newLight - state);

        GD.Print("[KyomiraTexture] TextureOffset is : " + textureIDOffset);

        var tiles = this.GetUsedCells();

        foreach(Vector2 a in tiles)
        {
            this.SetCell((int)a.x,(int)a.y, (this.GetCell((int)a.x, (int)a.y)) + (textureIDOffset * 47));
        }

        state = newLight;


    }

}
