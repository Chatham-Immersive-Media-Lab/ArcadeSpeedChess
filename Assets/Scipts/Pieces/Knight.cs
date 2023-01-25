using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Knight : Piece
    {
        public readonly static Vector2Int[] KnightPositions = new[]
        {
            new Vector2Int(1, 2),
            new Vector2Int(1,-2),
            new Vector2Int(2,1),
            new Vector2Int(2,-1),
            new Vector2Int(-1, 2),
            new Vector2Int(-1,-2),
            new Vector2Int(-2,1),
            new Vector2Int(-2,-1)
        };

        public override List<Tile> ValidDestinations()
        {
            var gridManager = _currentTile.GetGridManager();
            var tiles = new List<Tile>();
            foreach(var offset in KnightPositions)
            {
                var tile = gridManager.GetTileAtPosition(_currentTile.Position + offset);
                if (tile != null && IsValidDestination(tile))
                {
                    tiles.Add(tile);
                }
            }

            return tiles;
        }
    }
}
