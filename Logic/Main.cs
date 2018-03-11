using Godot;
using System;

using GodotUtils;
using static GodotUtils.DesignerManager;

public class Main : Node
{
    [Export] public PackedScene Mob;

    [Export] public int Score = 0;

    [Design] Player Player;
    [Design] Timer MobTimer;
    [Design] Timer ScoreTimer;
    [Design] Timer StartTimer;
    [Design] Node2D StartPosition;
    [Design] Path2D MobPath;
    [Design(parent:nameof(MobPath))] PathFollow2D MobSpawnLocation;
    [Design] HUD HUD;

    Random rnd = new Random();
    public override void _Ready()
    {
        InitializeComponents(this);

        HUD.StartGame += (s, e) =>
        {
            NewGame();
        };

        Player.OnCollision += (s, e) =>
        {
            GameOver();
        };
    }

    public void GameOver()
    {
        ScoreTimer.Stop();
        MobTimer.Stop();
        HUD.ShowGameOver();
    }

    public void NewGame()
    {
        Score = 0;
        HUD.UpdateScore(Score);
        HUD.ShowMessage("Get Ready");
        Player.Start(StartPosition.Position);
        StartTimer.Start();
    }

    private void OnStartTimerTimeout()
    {
        MobTimer.Start();
        ScoreTimer.Start();
    }

    private void OnScoreTimerTimeout()
    {
        Score += 1;
        HUD.UpdateScore(Score);
    }

    private void OnMobTimerTimeout()
    {
        MobSpawnLocation.Offset = (rnd.Next(0,1000));

        var mob = (Mob)Mob.Instance();
        AddChild(mob);

        mob.Position = MobSpawnLocation.Position;

        float direction = (float)(MobSpawnLocation.Rotation + Math.PI / 2);
        direction += rnd.Next((int)(-Math.PI * 10 / 4), (int)(Math.PI * 10 / 4));
        mob.Rotation = direction;

        mob.LinearVelocity = new Vector2(rnd.Next(mob.MinSpeed, mob.MaxSpeed), 0).Rotated(direction);

    }

}



