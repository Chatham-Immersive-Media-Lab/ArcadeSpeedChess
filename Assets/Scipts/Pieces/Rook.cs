using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Rook: Piece
    {
        public override List<Tile> ValidDestinations()
        {
            var tiles = new List<Tile>();
            //var forward = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.up, ref tiles);
            //var left = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.left, ref tiles);
            //var right = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.right, ref tiles);
            //var back = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.down, ref tiles);

            return tiles;
        }
    }
}