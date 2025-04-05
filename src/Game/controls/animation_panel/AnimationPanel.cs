using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Models;
using Game.Models.State;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class AnimationPanel : GamePanel
{
    private TileMapLayer _wallsLayer;
    private TileMapLayer _allDirsLayer;
    private TileMapLayer _noDirsLayer;
	private TileMapLayer _floorLayer;

    private IRoomDefRepo _roomDefRepo;

    // private IGameStateRepo _gameStateRepo;
    
    public override void _Ready()
    {
        base._Ready();
        
        _roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();
        
        _wallsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_Walls");
        _allDirsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_AllDirs");
        _noDirsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_NoDirs");
		_floorLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_Floor");
        Game.Events.EventBus.Instance.RoomChanged += OnRoomChanged;
        
        OnRoomChanged();
    }

    private readonly List<Label> _roomLabels = new();

    private int _updateAttempts = 0;
    private const int MAX_UPDATE_ATTEMPTS = 10;

	private void UpdateTilePos()
	{
		var panelSize = GetNode<Control>("VBoxContainer/BodyContainer").Size;

		// Wait until the panel is ready and has a proper size
		if (panelSize == Vector2.Zero)
		{
			if (_updateAttempts < MAX_UPDATE_ATTEMPTS)
			{
				_updateAttempts++;
				CallDeferred(nameof(UpdateTilePos));
				return;
			}
			
			GD.PrintErr("Failed to update tile position after maximum attempts");
			return;
		}

		var tileSize = _wallsLayer.TileSet.TileSize;
		GD.Print($"Tile size: {tileSize}");

		var tileScale = _wallsLayer.Scale;
		GD.Print($"Tile scale: {tileScale}");

		_updateAttempts = 0;
		
		GD.Print($"Panel size: {panelSize}");

		const int tilesPerRoom = 8;
		var roomWidth = tilesPerRoom * tileSize.X * tileScale.X;
		var roomHeight = tilesPerRoom * tileSize.Y * tileScale.Y;

		// Center horizontally and vertically
		var posX = (panelSize.X - roomWidth) / 2f;
		var posY = (panelSize.Y - roomHeight) / 2f;
		
		_wallsLayer.Position = new Vector2(posX, _wallsLayer.Position.Y);
		_allDirsLayer.Position = _wallsLayer.Position;
		_noDirsLayer.Position = _wallsLayer.Position;
		_floorLayer.Position = _wallsLayer.Position;
	}

    private void ClearLabels()
    {
        foreach (var label in _roomLabels)
        {
            RemoveChild(label);
            label.Dispose();
        }
        
        _roomLabels.Clear();
    }
    
    private void OnRoomChanged()
    {
		UpdateTilePos();
        ClearLabels();
        
        var roomDef = _roomDefRepo.Get(GameStateContainer.GameState.PlayerState.RoomId);
        GenerateDirectionForRoom(roomDef, 0);
        
        var westExit = roomDef.Exits
            .FirstOrDefault(queryExit => queryExit.Direction == Direction.West);
        
        if (westExit != null)
        {
            var westRoom = _roomDefRepo.Get(westExit.DestinationId);
            GenerateDirectionForRoom(westRoom, -1);
        }
        else
        {
            GenerateDirection(new List<Direction>(), -1, "Sorry, nothing");
        }
        
        var eastExit = roomDef.Exits
            .FirstOrDefault(queryExit => queryExit.Direction == Direction.East);
        
        if (eastExit != null)
        {
            GenerateDirectionForRoom(_roomDefRepo.Get(eastExit.DestinationId), 1);
        }
        else
        {
            GenerateDirection(new List<Direction>(), 1, "Sorry, nothing");
        }
    }

    private void GenerateDirectionForRoom(string roomId, int offsetX, string title)
    {
        var roomDef = _roomDefRepo.Get(roomId);
        var roomExits = roomDef.Exits.Select(roomExit => roomExit.Direction).ToList();        
        
        GenerateDirection(roomExits, offsetX, title);
    }
    
    private void GenerateDirectionForRoom(RoomDef roomDef, int offsetX)
    {
        var roomExits = roomDef.Exits.Select(roomExit => roomExit.Direction).ToList();
        GenerateDirection(roomExits, offsetX, roomDef.Name);
    }

    private record CellInfo
    {
        public Vector2I CellCoord { get; init; }
        public Vector2I AtlasCoord { get; init; }
    }

    private void GenerateDirection(Direction direction, int roomOffsetX, string title) =>
        GenerateDirection(new List<Direction> { direction }, roomOffsetX, title);

    private void GenerateDirection(
        List<Direction> directions,
        int roomOffsetX,
        string title)
    {
        var cellInfos = new List<CellInfo>();
        
        const int maxX = 8;
        const int maxY = 8;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            var cellCoord = new Vector2I(x, y);
            if (!directions.Any())
            {
                cellInfos.Add(new CellInfo
                {
                    CellCoord = cellCoord,
                    AtlasCoord = new Vector2I(-1, 0)
                });
            }
            else
            {
                var hasDirection = directions.Any(direction => IsDirectionInSection(direction, x, y));
                var sourceLayer = hasDirection ? _allDirsLayer : _noDirsLayer;
                var atlasCoord = sourceLayer.GetCellAtlasCoords(cellCoord);

                cellInfos.Add(new CellInfo
                {
                    CellCoord = cellCoord,
                    AtlasCoord = atlasCoord
                });
            }
        }
        
        foreach (var cellInfo in cellInfos)
        {
            _wallsLayer.SetCell(new Vector2I(cellInfo.CellCoord.X + 8 * roomOffsetX, cellInfo.CellCoord.Y), 1, cellInfo.AtlasCoord);
        }

        var roomLabel = new Label();
        roomLabel.Text = title;
        roomLabel.HorizontalAlignment = HorizontalAlignment.Center;
        
        const int tileSize = 200;
        const int roomWidth = tileSize;
        
        roomLabel.Position = new Vector2(
            _wallsLayer.Position.X + tileSize * roomOffsetX,
            _wallsLayer.Position.Y + tileSize / 2
        );
        
        roomLabel.CustomMinimumSize = new Vector2(roomWidth, 0);

        _roomLabels.Add(roomLabel);
        AddChild(roomLabel);
    }

    private bool IsDirectionInSection(Direction direction, int x, int y)
    {
        const int lowerY = 2;
        const int upperY = 5;

        const int lowerX = 2;
        const int upperX = 5;

        if (direction == Direction.North)
            return y <= lowerY && x >= lowerX && x <= upperX;

        if (direction == Direction.South)
            return y >= upperY && x >= lowerX && x <= upperX;
        
        if (direction == Direction.East)
            return x >= upperX && y >= lowerY && y <= upperY;
        
        if (direction == Direction.West)
            return x <= lowerX && y >= lowerY && y <= upperY;
        
        return false;
    }
    
    // private byte[] ReadAllDirs() =>
    //     ReadTileFile(TileFile.AllDirs);

    private byte[] ReadAllDirs()
    {
        return _allDirsLayer.TileMapData;
    }
    
    // private byte[] ReadNoDirs() =>
    //     ReadTileFile(TileFile.NoDirs);
    
    private byte[] ReadNoDirs()
    {
        return _noDirsLayer.TileMapData;
    }

    private byte[] ReadTileFile(string resourceName)
    {
        var contents = new ResourceReader().ReadEmbedded(resourceName);
        return Convert.FromBase64String(contents);
    }

    private void ExportTileMap(string fileName)
    {
        var contents = Convert.ToBase64String(_wallsLayer.TileMapData);
        
        var fullFileName = Path.Join("/home/goose/repo/bernard/src/Game/res/", fileName);
        File.WriteAllText(fullFileName, contents);
    }

    private void ExportCoordMap(string fileName)
    {
        var fullFileName = Path.Join("/home/goose/repo/bernard/src/Game/res/", fileName);
        
        const int maxX = 8;
        const int maxY = 8;

        var lines = new List<string>();
        for (var y = 0; y < maxY; y++)
        {
            var lineAtlasCoords = new List<Vector2I>();
            for (var x = 0; x < maxX; x++)
            {
                var cellCoord = new Vector2I(x, y);
                var atlasCoord = _wallsLayer.GetCellAtlasCoords(cellCoord);
                lineAtlasCoords.Add(atlasCoord);
            }
            
            var line = string.Join("  ", lineAtlasCoords.Select(AtlasCoordToString));
            lines.Add(line);
        }
        
        var contents = string.Join(System.Environment.NewLine, lines);
        
        File.WriteAllText(fullFileName, contents);
    }

    private string AtlasCoordToString(Vector2I source)
    {
        string Format(int val)
        {
            var prefix = val >= 0 ? " " : string.Empty;

            return $"{prefix}{val:00}";
        }
        
        return $"[{Format(source.X)}, {Format(source.Y)}]";
    }
    
    // private void ExportNumericTilemap(string fileName)
    // {
    //     var contents = string.Join(System.Environment.NewLine, _wallsLayer.TileMapData);
    //     var fullFileName = Path.Join("/home/goose/repo/bernard/src/Game/res/", "numeric-" + fileName);
    //     File.WriteAllText(fullFileName, contents);
    // }
} 