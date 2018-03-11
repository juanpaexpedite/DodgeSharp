using Godot;
using System;

using GodotUtils;
using static GodotUtils.DesignerManager;
using System.Threading.Tasks;

public class HUD : CanvasLayer
{
    [Design] Label ScoreLabel;
    [Design] Label MessageLabel;
    [Design] Button StartButton;
    [Design] Timer MessageTimer;

    public EventHandler StartGame;

    public override void _Ready()
    {
        InitializeComponents(this);
    }

    private void OnStartButtonPressed()
    {
        StartButton.Hide();
        StartGame?.Invoke(this, new EventArgs());
    }

    private void OnMessageTimerTimeout()
    {
        MessageLabel.Hide();
    }

    public void UpdateScore(int score)
    {
        ScoreLabel.Text = score.ToString();
    }

    public void ShowMessage(string message)
    {
        MessageLabel.Text = message;
        MessageLabel.Show();
        MessageTimer.Start();
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over");
        while(!MessageTimer.IsStopped())
        {
            await Task.Delay(100);
        }
        StartButton.Show();
        MessageLabel.Text = "Dogde the Creeps";
        MessageLabel.Show();
    }
}






