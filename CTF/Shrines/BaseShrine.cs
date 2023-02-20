using Godot;
using System;
using System.Collections.Generic;

public class BaseShrine : AnimatedSprite
{

    private byte capturePoint = 0;

    private AnimatedSprite[] capturedFlags = new AnimatedSprite[12];
    private Vector2 coordinates;
    private List<float> flagTiming = new List<float>();

    protected List<Entity> teamMates;

    protected Level map;
    private byte allFlags = 12;

    [Signal]
    protected delegate void EndGameSignal();


    public void InitShrine(Vector2 coords, List<Entity> team)
    {
        this.coordinates = coords;
        this.teamMates = team;
    }

    public override void _Ready()
    {

        map = this.GetParent() as Level;
        this.Play("default");

        for (byte i = 0; i < allFlags; i++)
        {
            capturedFlags[i] = this.GetChild(i) as AnimatedSprite;
        }

        //TODO : Connect EndGameSignal to level (no methods in level yet)

    }
    public void ShrineUpdate(List<Entity> sameTeamEntities)
    {
        //Checks all entities in team
        for (int i = 0; i < sameTeamEntities.Count; i++)
        {
            if (sameTeamEntities[i].heldFlag == -1)//if entity isn't holding a flag
                continue;//Check next entity

            String flagType;

            //Checks for entity in proximity
            if (((sameTeamEntities[i].pos.x >= (coordinates.x - 2)) && (sameTeamEntities[i].pos.x <= (coordinates.x + 2)))
            && ((sameTeamEntities[i].pos.y >= (coordinates.x - 2)) && (sameTeamEntities[i].pos.y <= (coordinates.x + 2))))
            {
                switch (sameTeamEntities[i].heldFlag)
                {
                    case 0:
                        flagType = "Savana";
                        break;
                    case 1:
                        flagType = "Psi";
                        break;
                    case 2:
                        flagType = "Cosmic";
                        break;
                    case 3:
                        flagType = "Gem";
                        break;
                    default:
                    return;
                
                }

                capturedFlags[capturePoint].Visible = true;//Makes Flag Visible
                capturedFlags[capturePoint].Play(flagType);//Makes it the right color
                capturedFlags[capturePoint].Frame = this.Frame;//Syncs the animation

                capturePoint++;

                if (capturePoint >= allFlags)
                {
                    EmitSignal("EndGameSignal");
                }

            }//End of if (Entity was in proximity)
        }
    }//End of ShrineUpdate

}//End of class
    

