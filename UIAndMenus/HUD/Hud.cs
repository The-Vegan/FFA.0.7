using Godot;
using System;
using System.Collections.Generic;

public class Hud : CanvasLayer
{

    private PackedScene note = GD.Load("res://UIAndMenus/HUD/BeatNote.tscn") as PackedScene;
    private Vector2 STARTPOS = new Vector2(88,-92);
    private String nextNote = "M";
    private List<AnimatedSprite> noteList = new List<AnimatedSprite>();

    public void HitNote(bool volontary)
    {
        String anim = "Hit";
        if (!volontary) anim = "destroy";

        Tween tween = noteList[0].GetChild(0) as Tween;
        tween.StopAll();
        noteList[0].Play(anim);

        noteList[0].Connect("animation_finished", noteList[0], "queue_free");

        noteList.RemoveAt(0);
    }

    private void BeatAtkUpdate()
    {
        AnimatedSprite bn = note.Instance() as AnimatedSprite;

        this.AddChild(bn,true);
        Tween tween = bn.GetChild(0) as Tween;

        noteList.Add(bn);

        bn.Position = STARTPOS;
        bn.Play(nextNote);
        tween.InterpolateProperty(bn, "position", bn.Position, new Vector2(88, 440), 2f, Tween.TransitionType.Linear);
        tween.Start();
        SignalAwaiter noteDeleter = new SignalAwaiter(tween, "tween_all_completed", this);

        noteDeleter.OnCompleted(delegate 
        {
            noteList.Remove(bn);
            bn.Play("destroy");
            bn.Connect("animation_finished", bn, "queue_free");
        });


        switch (nextNote)
        {
            case "C": nextNote = "W";
                break;
            case "M": nextNote = "C";
                break;
            case "W": nextNote = "M";
                break;
        }

    }
}
