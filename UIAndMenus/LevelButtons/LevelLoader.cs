using Godot;
using System;

public class LevelLoader : Button
{

    protected Level loadedLevel;

    [Export]
    protected PackedScene lvlToLoad;
    protected MainMenu mainMenu;

    public override void _Ready()
    {
        mainMenu = GetParent().GetParent() as MainMenu;
    }

    public override void _Pressed()
    {
        if (mainMenu.isMultiplayer)
        {
            mainMenu.MoveCameraTo(6);
        }



        loadedLevel = lvlToLoad.Instance() as Level;

        if (loadedLevel == null)
        {
            GD.Print("[LevelLoader] Err no levelToLoad");
            return;
        }
        mainMenu.SetLevel(lvlToLoad);

        if (!mainMenu.isMultiplayer)
            if (loadedLevel.InitPlayerAndMode(
                mainMenu.playerCharacter,
                mainMenu.gameMode,
                mainMenu.numberOfEntities,
                mainMenu.teams,
                mainMenu.chosenTeam))
            {
                GetTree().Root.AddChild(loadedLevel);

                mainMenu.QueueFree();
            }
        
    }

    
            
        
}
