using Godot;

public partial class SomeScene : Node2D
{
    private TileMapLayer _tileLayer;
    private Vector2 _mapSize;

    public override void _Ready()
    {
        _tileLayer = GetNode<TileMapLayer>("TileMapLayer");
        CenterTileLayer();
        QueueRedraw();
    }

    public override void _Draw()
    {
    }

    private void CenterTileLayer()
    {
        Vector2 viewportSize = GetViewportRect().Size;
        
        _mapSize = GetMapSize();
        _mapSize *= _tileLayer.Scale;
        
        _tileLayer.Position = (viewportSize - _mapSize) / 2;
        QueueRedraw();
    }

    private Vector2 GetMapSize()
    {
        var usedCells = _tileLayer.GetUsedCells();
        
        if (usedCells.Count == 0)
            return Vector2.Zero;

        float mapWidth = 0;
        float mapHeight = 0;

        foreach (Vector2I cell in usedCells)
        {
            mapWidth = Mathf.Max(mapWidth, cell.X + 1);
            mapHeight = Mathf.Max(mapHeight, cell.Y + 1);
        }

        return new Vector2(mapWidth, mapHeight);
    }
}