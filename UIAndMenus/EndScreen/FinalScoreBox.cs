using Godot;
using System;

public class FinalScoreBox : Control
{

    //Images
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\

    protected String bannerPath;

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Images

    //Variables
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected String name;
    protected short score;
    protected short perfectBeats;
    protected short missedBeats;//mister beast

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Variables

    //Child Nodes
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected Sprite banner;
    protected Sprite portrait;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ChildNode


    //Init Method
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    public void Init(byte podium,Entity player)
    {
        switch (podium)
        {
            case 1://TODO : IMPLEMENT BANNER PATHS (I NEED THE TEXTURES FIRST)
                break;
            case 2:
                break;
            case 3:
                break;

            default: throw new ArgumentException();
        }

        this.name = player.GetNametag();
        this.perfectBeats = player.GetPerBeat();
        this.missedBeats = player.GetMisBeat();


    }
    public override void _Ready()
    {
        banner = this.GetChild(0) as Sprite;
        portrait = banner.GetChild(0) as Sprite;

        banner.GetChild<Label>(1).Text = name;
        banner.GetChild<Label>(2).Text = score.ToString();
        banner.GetChild<Label>(3).Text = perfectBeats.ToString();
        banner.GetChild<Label>(4).Text = missedBeats.ToString();

    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Init Method
}
