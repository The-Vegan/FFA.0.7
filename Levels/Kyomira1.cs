using Godot;
using System;

public class Kyomira1 : Level
{
    
    public override void _Ready()
    {

        //DEBUG (REMOVE LATER)
        //______________________________________
        SpawnAllEntities();
        //______________________________________
        //DEBUG (REMOVE LATER)

        base._Ready();//When debug is over, this will spawn all entities 
        // -/!\ NEEDS TO BE CALLED AFTER SpawnAllEntities
    }

    protected override void InitSpawnPointsClasssic()
    {
        spawnpoints = new Vector2[]
        {
            new Vector2(12, 6),
            new Vector2(23, 6),
            new Vector2(6, 12),
            new Vector2(12,12),

            new Vector2(23,12),
            new Vector2(29, 12),
            new Vector2(18, 13),
            new Vector2(13, 17),

            new Vector2(22, 18),
            new Vector2(17, 22),
            new Vector2(6, 23),
            new Vector2(12,23),

            new Vector2(23,23),
            new Vector2(29, 23),
            new Vector2(12, 29),
            new Vector2(23, 29)
        };   
    }

    protected override void InitSpawnPointsCTF(int nbrOfTeams)
    {
        switch (nbrOfTeams)
        {
            case 2:
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                         new Vector2(12,  6),
                         new Vector2( 6, 12),
                         new Vector2(12, 12),
                         new Vector2( 6, 23),
                         new Vector2(12, 23),
                         new Vector2(12, 29)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                         new Vector2(23,  6),
                         new Vector2(23, 12),
                         new Vector2(29, 12),
                         new Vector2(23, 23),
                         new Vector2(23, 23),
                         new Vector2(29, 29)
                    });

                break;
            case 3:
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(10,  5),
                        new Vector2(13,  7),
                        new Vector2( 5, 10),
                        new Vector2(12, 12),
                        new Vector2( 7, 13),
                        new Vector2(10, 17)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(25,  5),
                        new Vector2(22,  7),
                        new Vector2(30, 10),
                        new Vector2(23, 12),
                        new Vector2(29, 13),
                        new Vector2(25, 17)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(12, 23),
                        new Vector2(23, 23),
                        new Vector2(15, 26),
                        new Vector2(20, 26),
                        new Vector2(11, 30),
                        new Vector2(24, 30)
                    });
                break;
            case 4:
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(8, 5),
                        new Vector2(12, 6),
                        new Vector2(5, 8),
                        new Vector2(6, 12),
                        new Vector2(12, 12)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(23, 5),
                        new Vector2(29, 6),
                        new Vector2(30, 8),
                        new Vector2(23, 12),
                        new Vector2(27, 12)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(8, 23),
                        new Vector2(12, 23),
                        new Vector2(5, 27),
                        new Vector2(6, 29),
                        new Vector2(12, 30)
                    });
                TeamSpawnPoints.Add(
                    new Vector2[]
                    {
                        new Vector2(23, 23),
                        new Vector2(29, 23),
                        new Vector2(30, 27),
                        new Vector2(23, 29),
                        new Vector2(27, 30)
                    });
                break;
            default:
                throw new Exception("Invalid Number of Teams");
        }
    }

    protected override void InitSpawnPointsSiege()
    {
        throw new NotImplementedException();
    }

    protected override void InitSpawnPointsTeam(int nbrOfTeams)
    {
        throw new NotImplementedException();
    }


    protected override void ClassicEndCond()
    {
        if (globalBeat > 150)
        {
            KyomiraTexture texture = GetNode("Texture") as KyomiraTexture;
            if (texture.state == 2) texture.ChangeLighting(1);
            else texture.ChangeLighting(2);
        }
    }

}
