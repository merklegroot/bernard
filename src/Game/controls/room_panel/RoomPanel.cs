using Game.Models;
using Game.Models.State;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.controls.room_panel;

public partial class RoomPanel : Panel
{
	private Label _descriptionLabel;
	private Label _titleLabel;

	private IRoomDefRepo _roomDefRepo;

	public override void _Ready()
	{
		_descriptionLabel = GetNode<Label>("DescriptionLabel");
		_titleLabel = GetNode<Label>("TitleLabel");

		_roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();

		UpdateDisplay();
		
		Events.EventBus.Instance.RoomChanged += OnRoomChanged;
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
		if (Events.EventBus.Instance != null)
		{
			Events.EventBus.Instance.RoomChanged -= OnRoomChanged;
		}
	}
}
