using Godot;
using System.Linq;
using System;

using GodotUtils;
using static GodotUtils.DesignerManager;

public class Player : Area2D
{
    public int Speed = 200;

    [Export]
    public Vector2 Velocity = new Vector2();

    [Design] AnimatedSprite Animations;
    [Design] CollisionShape2D Shape;

    public override void _Ready()
    {
        InitializeComponents(this);

        Settings.ScreenSize = GetViewportRect().Size;
        SetProcessInput(true);
        SetProcess(true);
    }


    public override void _Input(InputEvent ie)
    {
        if(ie.IsActionPressed("ui_right"))
        {
            Velocity.y = 0;
            Velocity.x = 1;
        }
        if(ie.IsActionPressed("ui_left"))
        {
            Velocity.y = 0;
            Velocity.x = -1;
        }

        if (ie.IsActionPressed("ui_up"))
        {
            Velocity.x = 0;
            Velocity.y = -1;
        }
        if (ie.IsActionPressed("ui_down"))
        {
            Velocity.x = 0;
            Velocity.y = 1;
        }

        if(Velocity.Length() > 0)
        {
            Velocity = Velocity.Normalized();
            Animations.Play();
        }
        else
        {
            Animations.Stop();
        }
    }

    public override void _Process(float delta)
    {
        UpdatePosition(delta);
        UpdateAnimation();
        base._Process(delta);
    }

    private void UpdatePosition(float delta)
    {
        Position += Velocity * Speed * delta;
        var x = Mathf.Clamp(Position.x, 0, Settings.ScreenSize.x);
        var y = Mathf.Clamp(Position.y, 0, Settings.ScreenSize.y);
        Position = new Vector2(x, y);
    }

    private void UpdateAnimation()
    {
        if(Velocity.x != 0)
        {
            Animations.Animation = "right";
            Animations.FlipV = false;
            Animations.FlipH = Velocity.x < 0;
        }
        else if(Velocity.y != 0)
        {
            Animations.Animation = "up";
            Animations.FlipV = Velocity.y > 0;
        }
    }

    #region Collision
    public EventHandler OnCollision;

    private void OnPlayerBodyEntered(Godot.Object body)
    {
        Hide();
        OnCollision?.Invoke(body, new EventArgs());
        Shape.Disabled = true;
    }
    #endregion

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        Shape.Disabled = false;
    }

}



