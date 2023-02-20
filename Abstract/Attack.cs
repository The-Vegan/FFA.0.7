using Godot;
using System;
using System.Collections.Generic;

public class Attack : Node2D
{
    //TECHNICAL
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected byte currentBeat = 0;
    protected byte maxBeat;
    protected List<List<Dictionary<String, short>>> packagedAtkData;
    protected List<short> keyChain = new List<short>();

    protected Vector2 gridPos;

    public Vector2 GetGridPos() { return gridPos; }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //TECHNICAL

    //DEPENDANCIES
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected Entity source;
    protected Level level;
    protected String folderPath;
    protected String beatAnimPath;

    protected PackedScene damageTileScene = GD.Load("res://Abstract/DamageTile.tscn") as PackedScene;

    public Entity GetSource() { return source; }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //DEPENDANCIES

    //ANIMATIONS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected bool flipableAnims = false;
    protected byte[] animations;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ANIMATIONS
    public void InitAtk(Entity attacker ,List<List<Dictionary<String, short>>> atkData ,Level map,String path ,byte[] collumns ,bool flipable)
    {

        this.source = attacker;
        this.packagedAtkData = atkData;
        this.level = map;
        this.folderPath = path;
        this.animations = collumns;
        this.flipableAnims = flipable;
        
        this.maxBeat = (byte)atkData.Count;

        this.gridPos = source.pos;


        this.Position = (source.pos * 64) + new Vector2(32,16);
    }

    public override void _Ready()
    {
        keyChain.Add(0);
    }

    private void BeatAtkUpdate()
    {
        currentBeat++;
        
        if (currentBeat > maxBeat)
        {
            this.QueueFree();
            return;
        }
        beatAnimPath = folderPath + "F" + currentBeat + ".png";
        var textureAnime = LoadSpriteSheet();

        var frameAtkData = packagedAtkData[currentBeat - 1];
        for(int tile = 0; tile < frameAtkData.Count; tile++)
        {
            var currentTile = frameAtkData[tile];//Select one tile at a time

            if (keyChain.Contains(currentTile["LOCK"]))
            {
                if (level.GetCell((int)(gridPos.x + currentTile["X"]),(int) (gridPos.y + currentTile["Y"])) != 2)//checks for wall
                {
                    CreateDamageTile(textureAnime,
                                     new Vector2((gridPos.x + currentTile["X"]), (gridPos.y + currentTile["Y"])),
                                     currentTile["ANIM"],
                                     currentTile["DAMAGE"]);
                    

                    if (!keyChain.Contains(currentTile["KEY"]))
                    {
                        
                        keyChain.Add(currentTile["KEY"]);
                    }

                }
            }


        }

        level.DamageEntity(this);

    }
    //DAMAGETILES
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected void CreateDamageTile(SpriteFrames texture, Vector2 tilePos, short anim, short damage)
    {
        DamageTile instancedDTS = damageTileScene.Instance() as DamageTile;
        Vector2 tile = tilePos - gridPos;

        
        bool flippableX = false, flippableY = false;
        if (flipableAnims)
        {
            if (tile.x < 0) flippableX = true;
            if (tile.y < 0) flippableY = true;

            
        }

         
        instancedDTS.InitDamageTile(source, tilePos ,"c" + anim,texture,damage,flippableX,flippableY);//Change the falses by flipable arguments later
        
        this.AddChild(instancedDTS, true);
        instancedDTS.Position += (tilePos - gridPos) * 64;
    }


    protected SpriteFrames LoadSpriteSheet()
    {
        SpriteFrames sf = new SpriteFrames();
        byte rows = 20;
        if (currentBeat == maxBeat) rows = 10;
       
        Texture spriteSheet = GD.Load(beatAnimPath) as Texture;
        
        //File not found
        if (spriteSheet == null)
        {
           GD.Print("[Attack] Can't find texture : " + beatAnimPath);
            spriteSheet = GD.Load("res://Entities/Default.png") as Texture;
            
        }
        //File not found

        for (byte col = 0; col < animations[currentBeat - 1]; col++)
        {
            //create animations
            sf.AddAnimation("c" + col);
            sf.SetAnimationSpeed("c" + col,30);
            sf.SetAnimationLoop("c" + col, false);

            
            for (byte r = 0; r < rows; r++)
            {
                AtlasTexture atlas = new AtlasTexture();
                atlas.Atlas = spriteSheet;
                atlas.Region = new Rect2(new Vector2(col*64,r*64), new Vector2(64,64));
                sf.AddFrame("c" + col, atlas, r);
            }

        }

        
        return sf;
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //DAMAGETILES
}
