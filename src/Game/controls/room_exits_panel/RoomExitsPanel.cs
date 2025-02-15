using Godot;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Models.State;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;

public partial class RoomExitsPanel : GamePanel
{
	private Button _northButton;
	private Button _eastButton;
	private Button _southButton;
	private Button _westButton;

	private IRoomDefRepo _roomDefRepo;

	public override void _Ready()
	{
		base._Ready();
		
		_northButton = GetNode<Button>("VBoxContainer/BodyContainer/ExitsContainer/NorthButton");
		_eastButton = GetNode<Button>("VBoxContainer/BodyContainer/ExitsContainer/EastButton");
		_southButton = GetNode<Button>("VBoxContainer/BodyContainer/ExitsContainer/SouthButton");
		_westButton = GetNode<Button>("VBoxContainer/BodyContainer/ExitsContainer/WestButton");
		
		_northButton.Pressed += () => OnDirectionButtonPressed(Direction.North);
		_eastButton.Pressed += () => OnDirectionButtonPressed(Direction.East);
		_southButton.Pressed += () => OnDirectionButtonPressed(Direction.South);
		_westButton.Pressed += () => OnDirectionButtonPressed(Direction.West);
		
		_roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();
		Game.Events.EventBus.Instance.RoomChanged += OnRoomChanged;
		UpdateDisplay();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventKey keyEvent) 
			return;
			
		if (!keyEvent.Pressed || keyEvent.Echo)
			return;

		var direction = keyEvent.Keycode switch
		{
			Key.W when !_northButton.Disabled => Direction.North,
			Key.A when !_westButton.Disabled => Direction.West,
			Key.S when !_southButton.Disabled => Direction.South,
			Key.D when !_eastButton.Disabled => Direction.East,
			_ => Direction.Invalid
		};

		if (direction != Direction.Invalid)
		{
			OnDirectionButtonPressed(direction);
		}
	}

	private void OnRoomChanged()
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		var roomDef = _roomDefRepo.Get(GameStateContainer.GameState.PlayerState.RoomId);

		UpdateExits(roomDef.Exits);
	}

	private void OnDirectionButtonPressed(Direction direction)
	{
		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.ExitRoom, (int)direction);
	}

	private void UpdateExits(List<RoomExit> exits)
	{
		_northButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.North);
		_eastButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.East);
		_southButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.South);
		_westButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.West);
	}
}
