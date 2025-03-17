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

    private IRoomDefRepo _roomDefRepo;

    // private IGameStateRepo _gameStateRepo;

    private static class TileFile
    {
        public const string AllDirs = "all-dirs.txt";
        public const string NoDirs = "no-dirs.txt";
    }
    
    public override void _Ready()
    {
        _roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();
        
        base._Ready();
        
        _wallsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_Walls");
        _allDirsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_AllDirs");
        _noDirsLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_NoDirs");
        
        Game.Events.EventBus.Instance.RoomChanged += OnRoomChanged;
        
        OnRoomChanged();
    }

    private void OnRoomChanged()
    {
        // ExportNumericTilemap("some.txt");
        // ExportTileMap(TileFile.NoDirs);
        
        
        // var contents = new ResourceReader().ReadEmbedded("all-dirs.txt");
        // var contents = new ResourceReader().ReadEmbedded("ns.txt");
        // var data = Convert.FromBase64String(contents);

        var allDirs = ReadAllDirs();
        var noDirs = ReadNoDirs();
        
        // _wallsLayer.TileMapData = noDirs;
        
        // ExportCoordMap("allDoorsCoords.txt");
        
        var currentRoomDef = _roomDefRepo.Get(GameStateContainer.GameState.PlayerState.RoomId);

        var currentRoomExits = currentRoomDef.Exits.Select(roomExit => roomExit.Direction).ToList();        
        
        // GenerateDirection(new List<Direction> { Direction.South, Direction.West }, allDirs, noDirs);
        
        GenerateDirection(currentRoomExits, allDirs, noDirs);
        
        // GenerateDirection(Direction.West, allDirs, noDirs);
        // FirstCellEverywhere(allDirs, noDirs);
    }

    private record CellInfo
    {
        public Vector2I CellCoord { get; init; }
        public Vector2I AtlasCoord { get; init; }
    }

    private void GenerateDirection(Direction direction, byte[] allDirs, byte[] noDirs) =>
        GenerateDirection(new List<Direction> { direction }, allDirs, noDirs);

    private void FirstCellEverywhere(
        byte[] allDirs,
        byte[] noDirs)
    {
        var cellInfos = new List<CellInfo>();
        
        _wallsLayer.TileMapData = allDirs;
        
        const int maxX = 16;
        const int maxY = 16;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            var cellCoord = new Vector2I(x, y);
            var atlasCoord = _wallsLayer.GetCellAtlasCoords(new Vector2I(1, 1));
            cellInfos.Add(new CellInfo
            {
                CellCoord = cellCoord,
                AtlasCoord = atlasCoord
            });
        }
        
        _wallsLayer.TileMapData = noDirs;
        foreach (var cellInfo in cellInfos)
        {
            _wallsLayer.SetCell(cellInfo.CellCoord, 0, cellInfo.AtlasCoord);
        }
    }
    
    private void GenerateDirection(
        List<Direction> directions,
        byte[] allDirs,
        byte[] noDirs)
    {
        var cellInfos = new List<CellInfo>();
        
        _wallsLayer.TileMapData = noDirs;
        
        const int maxX = 8;
        const int maxY = 8;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            if (!directions.Any(direction => IsDirectionInSection(direction, x, y)))
                continue;
            
            var cellCoord = new Vector2I(x, y);
            var atlasCoord = _allDirsLayer.GetCellAtlasCoords(cellCoord);
            cellInfos.Add(new CellInfo
            {
                CellCoord = cellCoord,
                AtlasCoord = atlasCoord
            });
        }
        
        // _wallsLayer.TileMapData = noDirs;
        foreach (var cellInfo in cellInfos)
        {
            _wallsLayer.SetCell(cellInfo.CellCoord, 1, cellInfo.AtlasCoord);
        }
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