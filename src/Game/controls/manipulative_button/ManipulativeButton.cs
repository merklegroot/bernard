using Godot;

public partial class ManipulativeButton : Button
{
	private readonly IManipulativeDefRepo _manipulativeDefRepo = new ManipulativeDefRepo();
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");
	
	private string _manipulativeId;
	
	[Export]
	public string ManipulativeId
	{
		get => _manipulativeId;
		set
		{
			_manipulativeId = value;
			UpdateDisplay();
		}
	}
	
	public override void _Ready()
	{
		LayoutMode = 2;
		Alignment = HorizontalAlignment.Left;
		ExpandIcon = true;
		
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		var manipulativeDef = !string.IsNullOrWhiteSpace(_manipulativeId)
			? _manipulativeDefRepo.Get(_manipulativeId)
			: null;
		
		Text = !string.IsNullOrWhiteSpace(manipulativeDef?.Name)
			? manipulativeDef.Name
			: "Unknown Item";
		
		Icon = !string.IsNullOrWhiteSpace(manipulativeDef?.ImageRes)
			? GD.Load<Texture2D>(manipulativeDef.ImageRes)
			: _defaultTexture;
	}
} 
