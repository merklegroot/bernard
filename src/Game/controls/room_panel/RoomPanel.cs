using Game.Models;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.controls.room_panel;

public partial class RoomPanel : Panel
{
	private Label _descriptionLabel;
	private Label _titleLabel;
	private RoomManipulativesPanel _manipulativesPanel;
	private RoomExitsPanel _exitsPanel;

	private IRoomDefRepo _roomDefRepo;

	public override void _Ready()
	{
		_descriptionLabel = GetNode<Label>("DescriptionLabel");
		_titleLabel = GetNode<Label>("TitleLabel");
		_manipulativesPanel = GetNode<RoomManipulativesPanel>("ManipulativesPanel");
		_exitsPanel = GetNode<RoomExitsPanel>("ExitsPanel");

		_roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();

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
		
		var roomDef = _roomDefRepo.Get(currentRoomId);

		_titleLabel.Text = string.IsNullOrWhiteSpace(roomDef.Name) ? "Room" : roomDef.Name;
		_descriptionLabel.Text = roomDef.Description;
	}

	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.RoomChanged -= OnRoomChanged;
		}
	}
}
