using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Models;
using Game.Repo;
using Godot;

public partial class RoomPanel : Panel
{
	private Label _roomIdLabel;
	private Label _descriptionLabel;
	private Label _titleLabel;
	private RoomManipulativesPanel _manipulativesPanel;
	private RoomExitsPanel _exitsPanel;

	private readonly RoomDefRepo _roomDefRepo = new();

	public override void _Ready()
	{
		_roomIdLabel = GetNode<Label>("RoomIdLabel");
		_descriptionLabel = GetNode<Label>("DescriptionLabel");
		_titleLabel = GetNode<Label>("TitleLabel");
		_manipulativesPanel = GetNode<RoomManipulativesPanel>("ManipulativesPanel");
		_exitsPanel = GetNode<RoomExitsPanel>("ExitsPanel");

		UpdateDisplay();
		
		EventBus.Instance.RoomChanged += OnRoomChanged;
	}

	private void OnRoomChanged()
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		var currentRoomId = GameStateContainer.GameState.RoomId;
		_roomIdLabel.Text = $"Room ID: {currentRoomId}";
		
		var roomDef = _roomDefRepo.Get(currentRoomId);

		_titleLabel.Text = string.IsNullOrWhiteSpace(roomDef.Name) ? "Room" : roomDef.Name;
		
		var descriptionBuilder = new StringBuilder();
		descriptionBuilder.AppendLine($"Room ID: {currentRoomId}");
		descriptionBuilder.AppendLine($"Room Name: {roomDef.Name}");
		
		_descriptionLabel.Text = descriptionBuilder.ToString();
	}
}
