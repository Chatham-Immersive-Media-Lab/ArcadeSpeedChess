using System.Collections.Generic;
using UnityEngine;

namespace Chess
{


    public class Queen : Piece
    {
        protected override string DisplayName => "Queen";

        public override List<Tile> ValidDestinations(bool checkTest = false)
        {
            //if valid add moves to list
            var tiles = new List<Tile>();
            var grid = _currentTile.GetGridManager();
            var currentPos = _currentTile.Position;
            //todo: can move any number of spaces left, right, forward, backwards, diagonal, as long as the space is valid. 
            //var forward = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.up, ref tiles);
            //var left = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.left, ref tiles);
            //var right = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.right, ref tiles);
            //var back = currentPos + new Vector2Int
            AddValidTilesInDirection(Vector2Int.down, ref tiles);
            //diagonals
            AddValidTilesInDirection(Vector2Int.one, ref tiles);
            AddValidTilesInDirection(-Vector2Int.one, ref tiles);
            AddValidTilesInDirection(new Vector2Int(1, -1), ref tiles);
            AddValidTilesInDirection(new Vector2Int(-1, 1), ref tiles);


            return tiles;
        }


    }
}