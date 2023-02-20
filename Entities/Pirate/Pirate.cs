using Godot;
using System;
using System.Collections.Generic;

public class Pirate : Entity
{
    /// A FAIRE
    /// RESOUDRE BUG DE ATKMOVE
    /// 
    /// A FAIRE
    private List<List<Dictionary<String, short>>> DOWNMOVEATK = new List<List<Dictionary<string, short>>>();
    private List<List<Dictionary<String, short>>> LEFTMOVEATK = new List<List<Dictionary<string, short>>>();
    private List<List<Dictionary<String, short>>> RIGHTMOVEATK = new List<List<Dictionary<string, short>>>();
    private List<List<Dictionary<String, short>>> UPMOVEATK = new List<List<Dictionary<string, short>>>();

    protected bool reallyMoved = false;
    public override void _Ready()
    {
        base._Ready();

        this.atkFolder = "res://Entities/Pirate/atk/";

        this.maxHP = 125;
        


        this.animPerBeat = new byte[] {5};
        {//Declare downAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", -1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } }
            };

            DOWNATK.Add(frame1);
        }//Declare downAtkTiles
        {//Declare leftAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", -1 },{ "Y", -1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", -1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", -1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } }
            };

            LEFTATK.Add(frame1);
        }//Declare leftAtkTiles
        {//Declare rightAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", -1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } }
            };


            RIGHTATK.Add(frame1);
        }//Declare rightAtkTiles
        {//Declare upAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", -1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", -1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", -1 },{ "Y", -1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } }
            };

            UPATK.Add(frame1);
        }//Declare upAtkTiles
        {//Declare downMoveAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 4 } }
            };

            DOWNMOVEATK.Add(frame1);
        }//Declare downMoveAtkTiles
        {//Declare leftMoveAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 4 } }
            };

            LEFTMOVEATK.Add(frame1);
        }//Declare leftMoveAtkTiles
        {//Declare rightMoveAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 4 } }
            };

            RIGHTMOVEATK.Add(frame1);
        }//Declare rightMoveAtkTiles
        {//Declare upMoveAtkTiles

            List<Dictionary<String, short>> frame1 = new List<Dictionary<String, short>>
            {
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 0 },{ "DAMAGE", 42 },{ "LOCK", 0 },{ "KEY", 0 },{ "ANIM", 4 } }
            };

            UPMOVEATK.Add(frame1);
        }//Declare upMoveAtkTiles


    }

    protected override short PacketParser(short packetToParse)
    {
        if ((packetToParse >> 4) == (packetToParse & 0b1111))//move and Attack At the same time (no rest/items)
        {
            short parsedPacket = 0;
            if ((packetToParse & 0b0001) != 0) parsedPacket = 17;
            else if ((packetToParse & 0b0010) != 0) parsedPacket = 34;
            else if ((packetToParse & 0b0100) != 0) parsedPacket = 68;
            else if ((packetToParse & 0b1000) != 0) parsedPacket = 136;

            return parsedPacket;
        }

        return base.PacketParser(packetToParse);
    }



    

    public override void Moved(Vector2 newTile)
    {
        if (pos == newTile)
        {
            reallyMoved = false;
            return;
        }
        else
            reallyMoved = true;

        base.Moved(newTile);
    }

    protected override void AskAtk()
    {
        if ((packet & 0b1111_0000) == 0) return;
        action = "Atk";
        cooldown = ATKCOOLDOWN;
        if (((packet >> 4) == (packet & 0b1111)) && (reallyMoved))//if dash-atk
        {

            if      (packet == 0b0001_0001) map.CreateAtk(this, DOWNMOVEATK, atkFolder + "DownMoveAtk", animPerBeat, flippableAnim);
            else if (packet == 0b0010_0010) map.CreateAtk(this, LEFTMOVEATK, atkFolder + "LeftMoveAtk", animPerBeat, flippableAnim);
            else if (packet == 0b0100_0100) map.CreateAtk(this, RIGHTMOVEATK, atkFolder + "RightMoveAtk", animPerBeat, flippableAnim);
            else if (packet == 0b1000_1000) map.CreateAtk(this, UPMOVEATK, atkFolder + "UpMoveAtk", animPerBeat, flippableAnim);

            return;
        }

        if      ((packet & 0b0001_0000) != 0) map.CreateAtk(this, DOWNATK, atkFolder + "DownAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b0010_0000) != 0) map.CreateAtk(this, LEFTATK, atkFolder + "LeftAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b0100_0000) != 0) map.CreateAtk(this, RIGHTATK, atkFolder + "RightAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b1000_0000) != 0) map.CreateAtk(this, UPATK, atkFolder + "UpAtk", animPerBeat, flippableAnim);
    }
}
