using FFA.Empty.Empty.ServerAndNetwork;
using Godot;
using System;
using System.Collections.Generic;

public abstract class Level : TileMap
{
    //Level Variable
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected short globalBeat = 0;
    protected Random rand = new Random();
    protected Vector2[] spawnpoints = new Vector2[12];

    protected List<Vector2[]> TeamSpawnPoints = new List<Vector2[]>();

    protected Timer timer;
    protected Camera2D camera;
    protected PackedScene hudScene = GD.Load<PackedScene>("res://UIAndMenus/HUD/Hud.tscn");

    protected bool teamMode = false;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Level Variable

    //Entities Variables
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected byte idToGive = 0;
    protected byte NumberOfEntities = 1;
    protected List<Entity> allEntities = new List<Entity>();
    protected Dictionary<byte, Entity> idToEntity = new Dictionary<byte, Entity>();
    protected Dictionary<Vector2, Entity> coordToEntity = new Dictionary<Vector2, Entity>();
    protected Dictionary<Vector2, Entity> oldCoordToEntity = new Dictionary<Vector2, Entity>();

    public string[] npcNameTags = new string[] {
        "Aro-spec",         "Azelma",           "Bananachi",        "Brown_Bricks",
        "Chugga",           "crab_person",      "Darius",           "DixDis10",
        "elemental",        "Enby_frog",        "Fizzerali",        "FlapPhillipe",
        "GRILLBY'S",        "GroovyWorm",       "HD_am_I",          "HootHoot!Bozo!",
        "iiiiiiiiiiiiiiii", "inobtanium",       "JNPR_NoPn",        "juice_tease",
        "Kennyoung",        "Kaizo",            "Lammasticot"  ,    "LivingGrave",
        "MadelineMcButt",   "Medhi_Dich",       "NeotanksVsNell",   "not a brony",
        "OhItsIwata",       "Orangusnake",      "protogen",         "Paragon",
        "QuandaleDingle",   "Queue_Tea_Pie",    "Rigged",           "Rush_Garcia",
        "snail_loli",       "StacKelsey",       "The_SH4DY_GR3Y",   "Todomatsu<3",
        "unnamed_Tank",     "UpsideDownT",      "Verrax",           "VR_IMAX",
        "Waluigi_Stan",     "WaveFuncClps",     "xX_victim_Xx",     "X_equals_9",
        "Yeenis",           "youself",          "ZeldaLulamoon" ,   "Zephyris"
    };
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //Entities Variables

    //DEPENDANCIES
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\  
    protected HostServer server;
    protected LocalClient client;

    protected Entity mainPlayer;
    
    protected PackedScene atkScene = GD.Load("res://Abstract/Attack.tscn") as PackedScene;
    
    [Signal]
    protected delegate void loadComplete(bool success);

    [Signal]
    protected delegate void checkEndingCondition();

    protected PackedScene pirateScene = GD.Load("res://Entities/Pirate/Pirate.tscn") as PackedScene;
    protected PackedScene blahajScene = GD.Load("res://Entities/Blahaj/Blahaj.tscn") as PackedScene;
    protected PackedScene monstropisScene = GD.Load("res://Entities/Monstropis/Monstropis.tscn") as PackedScene;

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //DEPENDANCIES


    //INIT METHODS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    public void InitPlayerAndMode(List<ScafholdEntity> entities,byte playerIdx, byte gameMode,byte numberOfTeam,HostServer theEntireFreakingServer)//TODO
    {
        this.server = theEntireFreakingServer;
        #pragma warning disable IDE0004 
        server.InitParent((Level) this);//the cast is, in fact, NOT redundant (child classes)
        #pragma warning restore IDE0004 

        switch (gameMode)//TODO : code the modes
        {
            /*
             0: Classic
             1: Team
             2: CTF
             3: Siege
             */
            case 0:
            default://fail-safe
                this.Connect("checkEndingCondition", this, "ClassicEndCond");
                teamMode = false;
                InitSpawnPointsClasssic();
                GD.Print("[Level] Classic");
                break;
            case 1:
                this.Connect("checkEndingCondition", this, "TeamEndCond");
                teamMode = true;
                InitSpawnPointsTeam(numberOfTeam);
                GD.Print("[Level] Team");
                break;
            case 2:
                this.Connect("checkEndingCondition", this, "CTFEndCond");//CTF NOT CODED
                teamMode = true;
                InitSpawnPointsCTF(numberOfTeam);
                GD.Print("[Level] CTF");
                break;
            case 3:
                this.Connect("checkEndingCondition", this, "SiegeEndCond");//SACKING NOT CODED
                teamMode = true;
                InitSpawnPointsSiege();
                GD.Print("[Level] Siege");
                break;
        }
        PackedScene controllerServer = GD.Load<PackedScene>("res://Abstract/ServerController.tscn");
        for (byte i = 0; i < entities.Count; i++)
        {
            if(i == playerIdx)
            {
                CreateEntityInstance(GD.Load<PackedScene>("res://Abstract/ControllerPlayer.tscn"));

                continue;
            }
        }
    }

    public void InitPlayerAndMode(byte chosenCharacter, byte gameMode, byte numberOfPlayers, byte numberOfTeams, byte chosenTeam)
    {
        NumberOfEntities = numberOfPlayers;

        //waitForOtherPlayer can also be set to false if the distant players are ready before the local player
        

        switch (gameMode)//TODO : code the modes
        {
            /*
             0: Classic
             1: Team
             2: CTF
             3: Siege
             */
            case 0:
            default://fail-safe
                this.Connect("checkEndingCondition", this, "ClassicEndCond");
                teamMode = false;
                InitSpawnPointsClasssic();
                GD.Print("[Level] Classic");
                break;
            case 1:
                this.Connect("checkEndingCondition", this, "TeamEndCond");
                teamMode = true;
                InitSpawnPointsTeam(numberOfTeams);
                GD.Print("[Level] Team");
                break;
            case 2:
                this.Connect("checkEndingCondition", this, "CTFEndCond");//CTF NOT CODED
                teamMode = true;
                InitSpawnPointsCTF(numberOfTeams);
                GD.Print("[Level] CTF");
                break;
            case 3:
                this.Connect("checkEndingCondition", this, "SiegeEndCond");//SACKING NOT CODED
                teamMode = true;
                InitSpawnPointsSiege();
                GD.Print("[Level] Siege");
                break;
        }
        PackedScene controllScene = GD.Load("res://Abstract/ControllerPlayer.tscn") as PackedScene;

        mainPlayer = CreateEntityInstance(chosenCharacter,controllScene);//CreateEntityInstance Adds the entity to the List of all entities

        if (numberOfPlayers > 16) throw new ArgumentException();
        for(byte i = 1; i < numberOfPlayers; i++)
        {
            CreateEntityInstance(GD.Load<PackedScene>("res://Abstract/CPUController.tscn"));
        }

        GD.Print("[Level] LoadCompleted in level");
        

    }
    //OVERRIDE
    protected abstract void InitSpawnPointsClasssic();
    protected abstract void InitSpawnPointsTeam(int nbrOfTeams);
    protected abstract void InitSpawnPointsCTF(int nbrOfTeams);
    protected abstract void InitSpawnPointsSiege();//Always 4 teams
    //OVERRIDE

    public override void _Ready()
    {
        camera = this.GetNode("Camera2D") as Camera2D;
        timer = this.GetNode("Timer") as Timer;
        this.RemoveChild(camera);
        
        Hud hud = hudScene.Instance() as Hud;

        mainPlayer.AddChild(hud);
        mainPlayer.Connect("noteHiter", hud, "HitNote");

        mainPlayer.AddChild(camera);
        

        camera.Current = true;
    }

    public float GetTime()
    {
        return timer.TimeLeft;
    }

    protected void SpawnAllEntities()
    {
        //PackedScene cpu = GD.Load("res://Abstract/GenericController.cs") as PackedScene;
        PackedScene cpu = GD.Load("res://Abstract/GenericController.tscn") as PackedScene;
        for (int i = allEntities.Count; i < NumberOfEntities; i++)
        {
           CreateEntityInstance(cpu);
        }
        for (int i = 0; i < allEntities.Count; i++)
        {
            Spawn(allEntities[i]);
        }

    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //INIT METHODS

    //ENTITY RELATED METHODS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected Entity CreateEntityInstance(PackedScene pcs)
    {
        return CreateEntityInstance(rand.Next(1,4), pcs);//Creates random entity
    }
    protected Entity CreateEntityInstance(PackedScene pcs,string nametag)
    {
        return CreateEntityInstance(rand.Next(1, 4), pcs,nametag);//Creates random entity
    }
    protected Entity CreateEntityInstance(int entityID,PackedScene controllScene)
    {
        Entity entity;
        //Selects correct entity from parameter ID
        switch (entityID)
        {
            case 1://Pirate
                GD.Print("[Level] Make a pirate");
                entity = pirateScene.Instance() as Pirate;       
                break;
            case 2://♥
                GD.Print("[Level] Make a ♥");
                entity = blahajScene.Instance() as Blahaj;
                break;
            case 3:
                GD.Print("[Level] Make a monstropis");
                entity = monstropisScene.Instance() as Monstropis;
                break;

            default://Random
                return CreateEntityInstance(rand.Next(1, 4), controllScene);

        }//End of characters switch statement

        //Finalizes configurations for player entity
        allEntities.Add(entity);
        do //Give an ID and adds it in dictionary
        {
            if (idToEntity.Count >= 250) throw new OverflowException("How did you even summon 250+ entities??? ;-;");

            idToGive++;
            try{idToEntity.Add(idToGive, entity);}
            catch (ArgumentException){continue;}
        
        } while (false);

        entity.Init(this, controllScene,npcNameTags[rand.Next(npcNameTags.Length)],idToGive);
        this.AddChild(entity);

        return entity;
    }
    protected Entity CreateEntityInstance(int entityID, PackedScene controllScene,string nametag)
    {
        Entity entity;
        //Selects correct entity from parameter ID
        switch (entityID)
        {
            case 1://Pirate
                GD.Print("[Level] Make a pirate");
                entity = pirateScene.Instance() as Pirate;
                break;
            case 2://♥
                GD.Print("[Level] Make a ♥");
                entity = blahajScene.Instance() as Blahaj;
                break;
            case 3:
                GD.Print("[Level] Make a monstropis");
                entity = monstropisScene.Instance() as Monstropis;
                break;

            default://Random
                return CreateEntityInstance(rand.Next(1, 4), controllScene);

        }//End of characters switch statement

        //Finalizes configurations for player entity
        allEntities.Add(entity);
        do
        {
            if (idToEntity.Count >= 250) throw new OverflowException("How did you even summon 250+ entities??? ;-;");

            idToGive++;
            try
            {
                idToEntity.Add(idToGive, entity);
            }
            catch (ArgumentException)//Give an ID and adds it in dictionary
            {
                continue;
            }

        } while (false);

        entity.Init(this, controllScene, nametag, idToGive);
        this.AddChild(entity);

        return entity;
    }
    public void DeleteEntity(Entity entity)
    {
        idToEntity.Remove(entity.id);
        allEntities.Remove(entity);

        entity.QueueFree();
        GD.Print("{[Level] " + entity + " has been deleted from existance");
    }

    public void MoveEntity(Entity entity,Vector2 newTile)
    {
        //Checks if tile is walkable
        if (this.GetCell((int)(newTile.x),(int)(newTile.y)) == 0)
        {
            entity.Moved(newTile);
        }
        else
        {
            entity.Moved(entity.pos);
        }
    }

    public async void Spawn(Entity entity)
    {
        byte failures = 0;//Forces spawning if fails too much

        entity.ResetHealth();

        while (failures < 65)
        {
            if (!teamMode)
            {
                int randomTile = rand.Next(spawnpoints.Length);

                if ((this.GetCell((int)spawnpoints[randomTile].x, (int)spawnpoints[randomTile].y) == 0) && (failures < 64))//If tile isn't occupied
                {
                    entity.Moved(spawnpoints[randomTile]);
                    await ToSignal(entity.GetNode("Tween"), "tween_completed");
                    entity.Visible = true;
                    break;
                }
            }
            else //if (teamMode)
            {
                int randomTile = rand.Next(TeamSpawnPoints[entity.team].Length);

                if ((this.GetCell((int)TeamSpawnPoints[entity.team][randomTile].x, (int)TeamSpawnPoints[entity.team][randomTile].y) == 0) && (failures < 64))
                {
                    entity.Moved(TeamSpawnPoints[entity.team][randomTile]);
                    entity.Visible = true;
                    break;
                }
            }
            failures++;
        }
       
    }
    public void SetEntityPacket(byte entityID, short packet, float timing)
    {
        if (!idToEntity.ContainsKey(entityID))
        {
            GD.Print("[Level] ERROR : Entity "+ entityID +" not found");
            return;
        }

        Entity entity = idToEntity[entityID];
        entity.SetPacket(packet);

        float delta = Math.Abs(entity.timing - timing);//Obtient l'écart entre lvl et entité

        if (delta < 0.03f) entity.timing = timing;//Cecks for lag(in seconds)


    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ENTITY RELATED METHODS

    //ATTACK RELATED METHODS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    public void CreateAtk(Entity source, List<List<Dictionary<String, short>>> atkData, String path, byte[] collumns, bool flipable)
    {
        Attack atkInstance = atkScene.Instance() as Attack;
        atkInstance.InitAtk(source, atkData, this, path, collumns, flipable);
        this.AddChild(atkInstance);
    }

    public void DamageEntity(Attack atk)
    {
        int damageTiles = atk.GetChildCount();
        for(int i = 0; i < damageTiles; i++)//Goes through all the damagetiles in the Attack
        {
            DamageTile dt = atk.GetChild<DamageTile>(i);

            if (dt.GetDamage() <= 0) continue;
            try
            {
                coordToEntity[dt.GetCoords()].Damaged(atk.GetSource(),dt.GetDamage());//Finds entity on tile and damages it
                oldCoordToEntity[dt.GetCoords()].Damaged(atk.GetSource(),dt.GetDamage());
            }
            catch (KeyNotFoundException)//No entities on that tile
            {
                continue;
            }
        }
    }

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ATTACK RELATED METHODS

    //ENDING CONDITION
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\

    protected void ClosingArena()
    {
        GD.Print("[Level] Hello World");
    }

    protected virtual void ClassicEndCond()
    {
        if(globalBeat > 200)
        {
            ClosingArena();
        }
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ENDING CONDITION
    public void TimerUpdate()
    {
        globalBeat++;
        GD.Print("[Level] - - - - - - - - - - - - - - - - - - - - - - - - " + globalBeat);

        UpdateAllEntities();

        UpdatePositionDictionary();

        GetTree().CallGroup("Attacks", "BeatAtkUpdate");//Also updates the hud

        EmitSignal("checkEndingCondition");
    }

    protected void UpdateAllEntities()
    {
        SortAllEntities();

        for (int i = 0;i < allEntities.Count; i++)
        {
            allEntities[i].BeatUpdate();
        }
    }

    protected void SortAllEntities()//CombSort
    {
        int gap = allEntities.Count >> 1;

        while(gap != 0)
        {
            Entity tempEntity;

            for(int i = 0;i < allEntities.Count - gap; i++)
            {
                
                if(Math.Abs(allEntities[i].timing - (1f / 6f)) > Math.Abs( allEntities[i + gap].timing - (1f / 6f)))//If entity in front has bigger score(worse)
                {
                    //Swap
                    tempEntity = allEntities[i];
                    allEntities[i] = allEntities[i + gap];
                    allEntities[i + gap] = tempEntity;
                }
            }

            gap--;
        }
    }

    private void UpdatePositionDictionary()
    {
        coordToEntity = new Dictionary<Vector2, Entity>();
        oldCoordToEntity = new Dictionary<Vector2, Entity>();
        
        for (int i= 0; i < allEntities.Count; i++)
        {

            try
            {
                coordToEntity.Add(allEntities[i].pos, allEntities[i]);//Adds the position of the entity as it's key
            }
            catch (ArgumentException) { }
            try
            {
                
                oldCoordToEntity.Add(allEntities[i].prevPos, allEntities[i]);//Separate dictionaries to avoid problems
            }
            catch (ArgumentException)
            {
                continue;
            }

            
        }
    }



}
