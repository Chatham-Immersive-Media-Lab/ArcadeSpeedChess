using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class King : Piece
    {
        public override List<Tile> ValidDestinations()
        {
            var tiles = new List<Tile>();
            var gridManager = _currentTile.GetGridManager();

            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    var dir = new Vector2Int(x, y);
                    if (dir == Vector2Int.zero)
                    {
                        continue;
                    }

                    var tile = gridManager.GetTileAtPosition(_currentTile.Position + dir);
                    if (tile != null && IsValidDestination(tile))
                    {
                        tiles.Add(tile);
                    }
                }
            }
            //Todo: castling

            return tiles;
        }
    }
}