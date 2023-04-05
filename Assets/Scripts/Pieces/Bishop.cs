using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Bishop : Piece
    {
        protected override string DisplayName => "Bishop";

        public override List<Tile> ValidDestinations()
        {
            tiles.Clear();

            //diagonals
            AddValidTilesInDirection(Vector2Int.one, ref tiles);
            AddValidTilesInDirection(-Vector2Int.one, ref tiles);
            AddValidTilesInDirection(new Vector2Int(1,-1), ref tiles);
            AddValidTilesInDirection(new Vector2Int(-1,1), ref tiles);
            return tiles;
        }
    }
}