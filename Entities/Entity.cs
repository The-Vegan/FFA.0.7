using Godot;
using System;
using System.Collections.Generic;

public class Entity : AnimatedSprite
{
    //DEPENDENCIES
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected PackedScene controllerScene;
    protected GenericController controller;
    protected Level map;
    protected Tween tween;
    protected AnimationPlayer animPlayer;

    public byte team = 0;
    public byte id;

    protected String nameTag;
    public String GetNametag() { return nameTag; }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //DEPENDENCIES

    //MOVEMENT RELATED
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected byte ATKCOOLDOWN = 1;
    public float timing = -1;

    protected short packet;
    public Vector2 pos = new Vector2(-1,-1);
    public Vector2 prevPos;

    protected byte stun = 0, cooldown = 3, respawnCooldown = 0;
    protected bool isDead = false;

    [Signal]
    public delegate void noteHiter( bool volontary);

    protected short perfectBeats = 0;
    protected short missedBeats = 0;

    public short GetPerBeat() { return perfectBeats; }
    public short GetMisBeat() { return missedBeats; }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //MOVEMENT RELATED

    //ATTACK VARIABLES
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected short healthPoint;
    protected short maxHP;

    public short GetHealthPoint() { return healthPoint; }

    protected bool isValidTarget = true;//Clones and stuff don't give you points for hitting them

    protected short blunderBar = 0;
    protected short itemBar = 0;
    public byte itemID = 0;
    public short GetBlunder() { return blunderBar; }
    protected List<Entity> damagedBy;

    protected String atkFolder;
    protected byte[] animPerBeat;
    protected bool flippableAnim = false;

    protected List<List<Dictionary<String, short>>> DOWNATK = new List<List<Dictionary<String, short>>>();
    protected List<List<Dictionary<String, short>>> LEFTATK = new List<List<Dictionary<String, short>>>();
    protected List<List<Dictionary<String, short>>> RIGHTATK = new List<List<Dictionary<String, short>>>();
    protected List<List<Dictionary<String, short>>> UPATK = new List<List<Dictionary<String, short>>>();

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ATTACK VARIABLES

    //ANIMATION
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected bool damaged = false;
    protected String direction = "Down";
    protected String action;
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //ANIMATION

    //CTF
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    public sbyte heldFlag = -1;

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //CTF

    public void Init(Level level,PackedScene c,String name,byte givenID)
    {
        this.map = level;
        this.controllerScene = c;
        this.nameTag = name;
        this.id = givenID;
    }
    public void Init(Level level, PackedScene c)
    {
        this.map = level;
        this.controllerScene = c;
        this.nameTag = "Bobby";
    }
    public override void _Ready()
    {
        animPlayer = (AnimationPlayer)this.GetNode("AnimationPlayer");
        tween = (Tween)this.GetNode("Tween");
        controller = controllerScene.Instance() as GenericController;
        this.AddChild(controller);
    }


    //GESTION DES INPUTS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected bool atkNoteWereHit = false;
    public virtual void SetPacket(short p)
    {

        if (cooldown != 0 || stun != 0)return;
        
        short comparePacket = packet;
        this.packet |= p;
        if (packet == comparePacket) return;

        float currTime = map.GetTime();
        if (timing < currTime)
        {
            timing = currTime;
            GD.Print("[Entity] Timing is : " + (timing - (1f / 6f)));

            if ((packet & 0b1111_0000) != 0)//If it's an attack
            {
                atkNoteWereHit = true;
                for (byte i = 0; i <= ATKCOOLDOWN;i++) EmitSignal("noteHiter", true);//Can't be registered in this context
            }
            else
            {
                EmitSignal("noteHiter", true);
            }

            
        }
        else if ((packet & 0b1111_0000) != 0 && !atkNoteWereHit)//If delayed input is an attack and is unregistered
        {
            atkNoteWereHit = true;
            for (byte i = 0; i < ATKCOOLDOWN; i++) EmitSignal("noteHiter", true);// < instead of <= to loop one time less
        }
        

    }

    protected virtual short PacketParser(short packetToParse)
    {
        short parsedPacket = 0;

        if ((packetToParse & 0b1111_0000) != 0)//Is it an attack
        {
            if ((packetToParse & 0b0001_0000) != 0) parsedPacket = 16;
            else if ((packetToParse & 0b0010_0000) != 0) parsedPacket = 32;
            else if ((packetToParse & 0b0100_0000) != 0) parsedPacket = 64;
            else if ((packetToParse & 0b1000_0000) != 0) parsedPacket = 128;
        }
        else
        {
            if ((packetToParse & 0b10_0000_0000) != 0) return 512;//Is it rest

            if ((packetToParse & 0b0000_0001) != 0) parsedPacket = 1;       //Is it a movement
            else if ((packetToParse & 0b0000_0010) != 0) parsedPacket = 2;
            else if ((packetToParse & 0b0000_0100) != 0) parsedPacket = 4;
            else if ((packetToParse & 0b0000_1000) != 0) parsedPacket = 8;
        }
        
        if ((packetToParse & 256) != 0) 
        {
            if (parsedPacket >= 16) parsedPacket >>= 4;//offsets attack into move
            parsedPacket |= 256;//enable item flag
        }

        return parsedPacket;
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //GESTION DES INPUTS

    //GESTION DES MOUVEMENTS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    protected void AskMovement()
    {

        //if none of movement bits are set to true
        if ((packet & 0b1111) == 0) return;

        if      ((packet & 0b0001) != 0) map.MoveEntity(this, pos + Vector2.Down);
        else if ((packet & 0b0010) != 0) map.MoveEntity(this, pos + Vector2.Left);
        else if ((packet & 0b0100) != 0) map.MoveEntity(this, pos + Vector2.Right);
        else if ((packet & 0b1000) != 0) map.MoveEntity(this, pos + Vector2.Up);
    }

    protected virtual void AskAtk()
    {
        if ((packet & 0b1111_0000) == 0) return;
        action = "Atk";
        cooldown = ATKCOOLDOWN;
        if      ((packet & 0b0001_0000) != 0) map.CreateAtk(this, DOWNATK, atkFolder + "DownAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b0010_0000) != 0) map.CreateAtk(this, LEFTATK, atkFolder + "LeftAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b0100_0000) != 0) map.CreateAtk(this, RIGHTATK, atkFolder + "RightAtk", animPerBeat, flippableAnim);
        else if ((packet & 0b1000_0000) != 0) map.CreateAtk(this, UPATK, atkFolder + "UpAtk", animPerBeat, flippableAnim);
    }

    public virtual void Moved(Vector2 newTile)
    {
        if (pos == newTile) return;


        map.SetCell((int)pos.x, (int)pos.y, 0);
        
        pos = newTile;
        map.SetCell((int)pos.x, (int)pos.y, 3);
        action = "Idle";
        tween.InterpolateProperty(this, "position",                          //Property to interpolate
            this.Position, new Vector2((pos.x * 64) + 32, (pos.y * 64) + 16),//initVal,FinalVal
            0.33f,                                                           //Duration
            Tween.TransitionType.Sine, Tween.EaseType.Out);                  //Tween says Trans rights
        tween.Start();
        animPlayer.Play("Move");
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //GESTION DES MOUVEMENTS

    //GESTION DES ANIMATIONS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    private async void MidBeatAnimManager()
    {
        await ToSignal(this, "animation_finished");
        if (damaged) 
        {
            this.Play("Damaged");
            damaged = false;
        }
        else
        {
            switch (action)
            {
                case "Idle":
                case "Cooldown":
                    action = "Wait";
                    break;
                case "Atk":
                    action = "Cooldown";
                    break;
                    
            }



            this.Play(action + direction);
        }

    }

    private void DirectionSetter()
    {
        if      ((packet & 0b0001) != 0) direction = "Down";
        else if ((packet & 0b0010) != 0) direction = "Left";
        else if ((packet & 0b0100) != 0) direction = "Right";
        else if ((packet & 0b1000) != 0) direction = "Up";
        else if ((packet & 0b0001_0000) != 0) direction = "Down";
        else if ((packet & 0b0010_0000) != 0) direction = "Left";
        else if ((packet & 0b0100_0000) != 0) direction = "Right";
        else if ((packet & 0b1000_0000) != 0) direction = "Up";
    }
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //GESTION DES ANIMATIONS
    //GESTION DES DEGATS
    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\

    public async void Damaged (Entity source,short damage)
    {
        //If damage is 0, this will not be called
        if (!damagedBy.Contains(source))
        {
            damagedBy.Add(source);
            healthPoint -= damage;
            damaged = true;

            if (isValidTarget)
            {
                source.HitSomeone((short)((damage << 2) + 5));
            }

            await ToSignal(this, "animation_finished");
            if(healthPoint <= 0)
            {
                Death();

            }

        }
    }

    protected void Death()
    {
        map.SetCell((int)pos.x, (int)pos.y, 0);//Makes tile walkable again

        if (!isValidTarget)
        {
            map.DeleteEntity(this);
            return;
        }


        for(int i = 1;i < damagedBy.Count; i++)
        {
            damagedBy[i].HitSomeone((short) (50/(damagedBy.Count - 1)));//Distributes 50 points between all killers
        }

        prevPos = Vector2.NegOne;//Prevents further damage
        pos = Vector2.NegOne;
        this.Visible = false;

        isDead = true;

        respawnCooldown = 6;
    }


    public virtual void HitSomeone(short points) 
    {
        this.itemBar += points;
        this.blunderBar -= (short)(points >> 2);

        if (itemBar > 100) itemBar = 100;
        if (blunderBar < 0) blunderBar = 0;
    }

    public void ResetHealth()
    {
        healthPoint = maxHP;
    }

    public void RestoreHealth(short hp)
    {
        this.healthPoint += hp;
        if (healthPoint > maxHP) healthPoint = maxHP;
    }

    //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\\
    //GESTION DES DEGATS
    


    public void BeatUpdate()
    {
        prevPos = pos;
        

        if (isDead)
        {
            respawnCooldown--;
            if(respawnCooldown <= 0)
            {
                map.Spawn(this);
            }
        }
        if (stun != 0)
        {
            cooldown = 0;
            stun--;
            ResetBeatValues();
            return;
        }
        if (cooldown != 0)
        {
            cooldown--;
            ResetBeatValues();
            GD.Print("[Entity] has cooldown-- = " + cooldown);
            return;
        }
        if (packet == 0)
        {
            this.Play("FailedInput");
            action = "Idle";
            MidBeatAnimManager();
            ResetBeatValues();
            return;
        }

        packet = PacketParser(packet);
        AskMovement();
        DirectionSetter();
        AskAtk();
        
        this.Play(action + direction);
        MidBeatAnimManager();

        ResetBeatValues();
        
    }//End of BeatUpdate
    private void ResetBeatValues()
    {
        timing = -1;
        packet = 0;
        atkNoteWereHit = false;
        damagedBy = new List<Entity>() { this };
    }
}
