using Godot;

public partial class GamePanel : Panel
{
    private Label _titleLabel;
    private HSeparator _separator;
    private string _title = "";

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            UpdateTitleVisibility();
        }
    }

    public override void _Ready()
    {
        _titleLabel = GetNode<Label>("VBoxContainer/TitleLabel");
        _separator = GetNode<HSeparator>("VBoxContainer/HSeparator");
        
        // Initialize visibility based on current title
        UpdateTitleVisibility();
    }

    private void UpdateTitleVisibility()
    {
        bool shouldShow = !string.IsNullOrWhiteSpace(_title);
        _titleLabel.Visible = shouldShow;
        _separator.Visible = shouldShow;
        
        if (shouldShow)
        {
            _titleLabel.Text = _title;
        }
    }
} 