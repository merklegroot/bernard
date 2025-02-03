using System;
using Game.Models;
using Godot;

public partial class ManipulativeButton : Button
{
	private readonly IManipulativeDefRepo _manipulativeDefRepo = new ManipulativeDefRepo();
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");

	private string _selectionDataText;
	
	[Export]
	public string SelectionDataText
	{
		get => _selectionDataText;
		set
		{
			_selectionDataText = value;
			UpdateDisplay();
		}
	}

	private InventoryItemSelectionData SelectionData =>
		_selectionDataText;
	
	public override void _Ready()
	{
		LayoutMode = 2;
		Alignment = HorizontalAlignment.Left;
		ExpandIcon = true;
		
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		if (string.IsNullOrWhiteSpace(_selectionDataText))
			return;
		
		var manipulativeDef = _manipulativeDefRepo.Get(SelectionData.ManipulativeDefId);
		
		Text = !string.IsNullOrWhiteSpace(manipulativeDef?.Name)
			? manipulativeDef.Name
			: "Unknown Item";
		
		Icon = !string.IsNullOrWhiteSpace(manipulativeDef?.ImageRes)
			? GD.Load<Texture2D>(manipulativeDef.ImageRes)
			: _defaultTexture;
	}
} 
