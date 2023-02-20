using Godot;
using System;
using System.Collections.Generic;

public class Blahaj : Entity
{

    public override void _Ready()
    {
        base._Ready();

        this.maxHP = 175;

        this.flippableAnim = true;
        this.atkFolder = "res://Entities/Blahaj/atk/";

        animPerBeat = new byte[]{4,5,3};

        DOWNATK = new List<List<Dictionary<String, short>>>
        {
            new List<Dictionary<String, short>> //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 1 },{ "DAMAGE", 54 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 4 },{ "ANIM", 1 } }
            },                                  //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 2 },{ "DAMAGE", 27 },{ "LOCK", 2 },{ "KEY", 5 },{ "ANIM", 0 } },

                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 3 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 8 },{ "ANIM", 4 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 9 },{ "ANIM", 3 } },
                 new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY",10 },{ "ANIM", 3 } },


                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 27 },{ "LOCK", 3 },{ "KEY", 6 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY", 9 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY",11 },{ "ANIM", 2 } },


                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 27 },{ "LOCK", 4 },{ "KEY", 7 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",10 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",12 },{ "ANIM", 2 } },


            },                                  //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 3 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 12 },{ "LOCK",11 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 15 },{ "LOCK", 9 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y", 3 },{ "DAMAGE", 12 },{ "LOCK", 8 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 15 },{ "LOCK",10 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 12 },{ "LOCK",12 },{ "KEY", 0 },{ "ANIM", 2 } }
            }                                   //PHASE 3 :::::::::::::::::::::::::::::::::::::::


        };


        LEFTATK = new List<List<Dictionary<String, short>>>
        {
            new List<Dictionary<String, short>> //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 0 },{ "DAMAGE", 54 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 0 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 2} },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 4 },{ "ANIM", 1 } }
            },                                  //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 0 },{ "DAMAGE", 27 },{ "LOCK", 2 },{ "KEY", 5 },{ "ANIM", 0 } },

                new Dictionary<string, short>
            { { "X",-3 },{ "Y", 0 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 8 },{ "ANIM", 4 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 9 },{ "ANIM", 3 } },
                 new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY",10 },{ "ANIM", 3 } },


                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 1 },{ "DAMAGE", 27 },{ "LOCK", 3 },{ "KEY", 6 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY", 9 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY",11 },{ "ANIM", 2 } },


                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 27 },{ "LOCK", 4 },{ "KEY", 7 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",10 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",12 },{ "ANIM", 2 } },


            },                                  //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 3 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X",-1 },{ "Y", 2 },{ "DAMAGE", 12 },{ "LOCK",11 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y", 1 },{ "DAMAGE", 15 },{ "LOCK", 9 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-3 },{ "Y", 0 },{ "DAMAGE", 12 },{ "LOCK", 8 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 15 },{ "LOCK",10 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 12 },{ "LOCK",12 },{ "KEY", 0 },{ "ANIM", 2 } }
            }                                   //PHASE 3 :::::::::::::::::::::::::::::::::::::::


        };

        RIGHTATK = new List<List<Dictionary<String, short>>>
        {
            new List<Dictionary<String, short>> //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 0 },{ "DAMAGE", 54 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 0 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 4 },{ "ANIM", 1 } }
            },                                  //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 0 },{ "DAMAGE", 27 },{ "LOCK", 2 },{ "KEY", 5 },{ "ANIM", 0 } },

                new Dictionary<string, short>
            { { "X", 3 },{ "Y", 0 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 8 },{ "ANIM", 4 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 9 },{ "ANIM", 3 } },
                 new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY",10 },{ "ANIM", 3 } },


                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 1 },{ "DAMAGE", 27 },{ "LOCK", 3 },{ "KEY", 6 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY", 9 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY",11 },{ "ANIM", 2 } },


                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 27 },{ "LOCK", 4 },{ "KEY", 7 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",10 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",12 },{ "ANIM", 2 } },


            },                                  //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 3 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 1 },{ "Y", 2 },{ "DAMAGE", 12 },{ "LOCK",11 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y", 1 },{ "DAMAGE", 15 },{ "LOCK", 9 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 3 },{ "Y", 0 },{ "DAMAGE", 12 },{ "LOCK", 8 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 15 },{ "LOCK",10 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 12 },{ "LOCK",12 },{ "KEY", 0 },{ "ANIM", 2 } }
            }                                   //PHASE 3 :::::::::::::::::::::::::::::::::::::::


        };


        UPATK = new List<List<Dictionary<String, short>>>
        {
            new List<Dictionary<String, short>> //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-1 },{ "DAMAGE", 54 },{ "LOCK", 0 },{ "KEY", 1 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 2 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 3 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 1 },{ "KEY", 4 },{ "ANIM", 1 } }
            },                                  //PHASE 1 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-2 },{ "DAMAGE", 27 },{ "LOCK", 2 },{ "KEY", 5 },{ "ANIM", 0 } },

                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-3 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 8 },{ "ANIM", 4 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY", 9 },{ "ANIM", 3 } },
                 new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 5 },{ "KEY",10 },{ "ANIM", 3 } },


                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-1 },{ "DAMAGE", 27 },{ "LOCK", 3 },{ "KEY", 6 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY", 9 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 6 },{ "KEY",11 },{ "ANIM", 2 } },


                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-1 },{ "DAMAGE", 27 },{ "LOCK", 4 },{ "KEY", 7 },{ "ANIM", 1 } },

                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",10 },{ "ANIM", 3 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 0 },{ "LOCK", 7 },{ "KEY",12 },{ "ANIM", 2 } },


            },                                  //PHASE 2 :::::::::::::::::::::::::::::::::::::::
            new List<Dictionary<string, short>> //PHASE 3 :::::::::::::::::::::::::::::::::::::::
            {
                new Dictionary<string, short>
            { { "X", 2 },{ "Y",-1 },{ "DAMAGE", 12 },{ "LOCK",11 },{ "KEY", 0 },{ "ANIM", 2 } },
                new Dictionary<string, short>
            { { "X", 1 },{ "Y",-2 },{ "DAMAGE", 15 },{ "LOCK", 9 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X", 0 },{ "Y",-3 },{ "DAMAGE", 12 },{ "LOCK", 8 },{ "KEY", 0 },{ "ANIM", 0 } },
                new Dictionary<string, short>
            { { "X",-1 },{ "Y",-2 },{ "DAMAGE", 15 },{ "LOCK",10 },{ "KEY", 0 },{ "ANIM", 1 } },
                new Dictionary<string, short>
            { { "X",-2 },{ "Y",-1 },{ "DAMAGE", 12 },{ "LOCK",12 },{ "KEY", 0 },{ "ANIM", 2 } }
            }                                   //PHASE 3 :::::::::::::::::::::::::::::::::::::::


        };

    }//Ready

    protected override void AskAtk()
    {
        if ((packet & 0b1111_0000) == 0) return;
        action = "Atk";
        cooldown = ATKCOOLDOWN;
        if ((packet & 0b0001_0000) != 0) map.CreateAtk(this, DOWNATK, atkFolder + "V", animPerBeat, flippableAnim);
        else if ((packet & 0b0010_0000) != 0) map.CreateAtk(this, LEFTATK, atkFolder + "H", animPerBeat, flippableAnim);
        else if ((packet & 0b0100_0000) != 0) map.CreateAtk(this, RIGHTATK, atkFolder + "H", animPerBeat, flippableAnim);
        else if ((packet & 0b1000_0000) != 0) map.CreateAtk(this, UPATK, atkFolder + "V", animPerBeat, flippableAnim);
    }


}
