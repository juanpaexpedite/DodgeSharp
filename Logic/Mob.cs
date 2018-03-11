using Godot;
using System;

using GodotUtils;
using static GodotUtils.DesignerManager;

public enum MobStates
{
    Walk,
    Swim,
    Fly
}

public class Mob : RigidBody2D
{
    public int MinSpeed = 100;

    public int MaxSpeed = 200;

    private MobStates State;

    [Design] AnimatedSprite Animations;
    [Design] CollisionShape2D Shape;
    [Design] VisibilityNotifier2D Notifier;

    Random gen = new Random();

    public override void _Ready()
    {
        InitializeComponents(this);

        SetRandomState();
    }

    private void SetRandomState()
    {
        Animations.Animation = ((MobStates)gen.Next(0, 3)).ToString(); 
    }

    //Signal
    private void OnNotifierScreenExited()
    {
        //Removes item from the tree
        QueueFree();
    }

}



