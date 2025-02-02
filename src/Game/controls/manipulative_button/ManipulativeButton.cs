using System;
using Godot;

public partial class ManipulativeButton : Button
{
	private readonly IManipulativeDefRepo _manipulativeDefRepo = new ManipulativeDefRepo();
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");
	
	private string _manipulativeDefId;
	
	[Export]
	public string ManipulativeDefId
	{
		get => _manipulativeDefId;
		set
		{
			_manipulativeDefId = value;
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
		var parsedManipulativeDefId = Guid.Parse(_manipulativeDefId);
		
		var manipulativeDef = !string.IsNullOrWhiteSpace(_manipulativeDefId)
			? _manipulativeDefRepo.Get(parsedManipulativeDefId)
			: null;
		
		Text = !string.IsNullOrWhiteSpace(manipulativeDef?.Name)
			? manipulativeDef.Name
			: "Unknown Item";
		
		Icon = !string.IsNullOrWhiteSpace(manipulativeDef?.ImageRes)
			? GD.Load<Texture2D>(manipulativeDef.ImageRes)
			: _defaultTexture;
	}
} 
