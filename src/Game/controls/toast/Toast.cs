using Godot;

public partial class Toast : PanelContainer
{
    private Label _label;
    private Timer _timer;
    
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _timer = GetNode<Timer>("Timer");
        
        _timer.Timeout += OnTimeout;
    }
    
    public void Show(string message, double duration = 2.0)
    {
        _label.Text = message;
        Visible = true;
        _timer.WaitTime = duration;
        _timer.Start();
    }
    
    private void OnTimeout()
    {
        Visible = false;
    }
} 