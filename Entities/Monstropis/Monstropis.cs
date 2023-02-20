using Godot;
using System;
using System.Collections.Generic;

public class Monstropis : Entity
{

    public override void _Ready()
    {
        base._Ready();
        this.ATKCOOLDOWN = 2;

        this.maxHP = 135;

        this.flippableAnim = true;

        this.animPerBeat = new byte[] { 4 };
        this.atkFolder = "res://zbeubzbeub";



        DOWNATK = new List<List<Dictionary<string, short>>>
        {
            new List<Dictionary<string, short>>
            {
                 new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 2 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 30 },{ "LOCK", 2 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 30 },{ "LOCK", 3 },{ "KEY", 0 },{ "ANIM", 0 } }
            }
        };
        LEFTATK = new List<List<Dictionary<string, short>>>
        {
            new List<Dictionary<string, short>>
            {
                 new Dictionary<string, short>
            { { "X",-1 },{ "Y", 0 },{ "DAMAGE", 30 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 0 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 2 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 3 },{ "KEY", 0 },{ "ANIM", 0 } }
            }
        };
        RIGHTATK = new List<List<Dictionary<string, short>>>
        {
            new List<Dictionary<string, short>>
            {
                 new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 30 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 0 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 2 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 30 },{ "LOCK", 3 },{ "KEY", 0 },{ "ANIM", 0 } }
            }
        };
        UPATK = new List<List<Dictionary<string, short>>>
        {
            new List<Dictionary<string, short>>
            {
                 new Dictionary<string, short>
            { { "X", 0 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-2 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 30 },{ "LOCK", 2 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 30 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 30 },{ "LOCK", 3 },{ "KEY", 0 },{ "ANIM", 0 } }
            }
        };



    }
    public override void HitSomeone(short points)
    {
        base.HitSomeone(points);
        RestoreHealth(15);

    }

}
