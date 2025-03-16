using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Models;
using Godot;

public partial class AnimationPanel : GamePanel
{
    private TileMapLayer _tileMapLayer;

    private static class TileFile
    {
        public const string AllDirs = "all-dirs.txt";
        public const string NoDirs = "no-dirs.txt";
    }
    
    public override void _Ready()
    {
        base._Ready();
        
        _tileMapLayer = GetNode<TileMapLayer>("VBoxContainer/BodyContainer/TileMapLayer_Walls");
        SetupTileMap();
    }

    private void SetupTileMap()
    {
        // ExportNumericTilemap("some.txt");
        ExportTileMap(TileFile.NoDirs);
        
        // var contents = new ResourceReader().ReadEmbedded("all-dirs.txt");
        // var contents = new ResourceReader().ReadEmbedded("ns.txt");
        // var data = Convert.FromBase64String(contents);

        // var allDirs = ReadAllDirs();
        // var noDirs = ReadNoDirs();
        
        // GenerateDirection(Direction.North, allDirs, noDirs);
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
        
        _tileMapLayer.TileMapData = allDirs;
        
        const int maxX = 16;
        const int maxY = 16;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            var cellCoord = new Vector2I(x, y);
            var atlasCoord = _tileMapLayer.GetCellAtlasCoords(new Vector2I(1, 1));
            cellInfos.Add(new CellInfo
            {
                CellCoord = cellCoord,
                AtlasCoord = atlasCoord
            });
        }
        
        _tileMapLayer.TileMapData = noDirs;
        foreach (var cellInfo in cellInfos)
        {
            _tileMapLayer.SetCell(cellInfo.CellCoord, 0, cellInfo.AtlasCoord);
        }
    }
    
    private void GenerateDirection(
        List<Direction> directions,
        byte[] allDirs,
        byte[] noDirs)
    {
        var cellInfos = new List<CellInfo>();
        
        _tileMapLayer.TileMapData = allDirs;
        
        const int maxX = 8;
        const int maxY = 8;
        for (var x = 0; x < maxX; x++)
        for (var y = 0; y < maxY; y++)
        {
            if (!directions.Any(direction => IsDirectionInSection(direction, x, y)))
                continue;
            
            var cellCoord = new Vector2I(x, y);
            var atlasCoord = _tileMapLayer.GetCellAtlasCoords(cellCoord);
            cellInfos.Add(new CellInfo
            {
                CellCoord = cellCoord,
                AtlasCoord = atlasCoord
            });
        }
        
        _tileMapLayer.TileMapData = noDirs;
        foreach (var cellInfo in cellInfos)
        {
            _tileMapLayer.SetCell(cellInfo.CellCoord, 0, cellInfo.AtlasCoord);
        }
    }

    private bool IsDirectionInSection(Direction direction, int x, int y)
    {
        const int lowerY = 2;
        const int upperY = 6;

        const int lowerX = 2;
        const int upperX = 6;

        if (direction == Direction.North)
            return y <= lowerY && x >= lowerX && x <= upperX;

        if (direction == Direction.South)
            return y >= upperY && x >= lowerX && x <= upperX;
        
        
        return false;
    }
    
    private byte[] ReadAllDirs() =>
        ReadTileFile(TileFile.AllDirs);
    
    private byte[] ReadNoDirs() =>
        ReadTileFile(TileFile.NoDirs);

    private byte[] ReadTileFile(string resourceName)
    {
        var contents = new ResourceReader().ReadEmbedded(resourceName);
        return Convert.FromBase64String(contents);
    }

    private void ExportTileMap(string fileName)
    {
        var contents = Convert.ToBase64String(_tileMapLayer.TileMapData);
        
        var fullFileName = Path.Join("/home/goose/repo/bernard/src/Game/res/", fileName);
        File.WriteAllText(fullFileName, contents);
    }

    private void ExportNumericTilemap(string fileName)
    {
        var contents = string.Join(System.Environment.NewLine, _tileMapLayer.TileMapData);
        var fullFileName = Path.Join("/home/goose/repo/bernard/src/Game/res/", "numeric-" + fileName);
        File.WriteAllText(fullFileName, contents);
    }
} 