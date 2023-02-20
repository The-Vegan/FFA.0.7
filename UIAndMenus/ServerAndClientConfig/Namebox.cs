using Godot;
using System;

public class Namebox : LineEdit
{
    public string[] npcNameTags = new string[] {
        "AmIABot!?",        "Azelma",
        "Bananachi",        "Brown_Bricks",
        "Chugga",           "crab_person",
        "Darius",           "DixDis10",
        "elemental",        "Enby_frog",
        "Fizzerali",        "FlapPhillipe",
        "GRILLBY'S",        "GroovyWorm",
        "HD_am_I",          "HatInTheCat",
        "iiiiiiiiiiiiiiii", "inobtanium",
        "JNPR_NoPn",        "juice_tease",
        "Kennyoung",        "Kaizo",
        "Lammasticot"  ,    "LivingGrave",
        "MadelineMcButt",   "Medhi_Dich",
        "NeotanksVsNell",   "not a brony",
        "OhItsIwata",       "Orangusnake",
        "protogen",         "Paragon",
        "QuandaleDingle",   "Queue_Tea_Pie",
        "RegrowFarm",       "Rush_Garcia",
        "snail_loli",       "StacKelsey",
        "The_SH4DY_GR3Y",   "Todomatsu<3",
        "unnamed_Tank",     "UpsideDownT",
        "Verrax",           "VR_IMAX",
        "Waluigi_Stan",     "WaveFuncClps",
        "xX_victim_Xx",     "X_equals_9",
        "Yeenis",           "youself",
        "ZeldaLulamoon" ,   "Zodiax"
    };

    private Random rd = new Random();
    private MainMenu mm;
    public override void _Ready()
    {
        mm = GetParent().GetParent() as MainMenu;
        mm.nameBox = this;
        this.Text = npcNameTags[rd.Next(npcNameTags.Length)];
    }

    public void FocusExit()
    {
        if(String.IsNullOrEmpty(this.Text)) this.Text = npcNameTags[rd.Next(npcNameTags.Length)];

        mm.SetMultiplayerName(this.Text);
    }

}
