using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override List<Tile> ValidDestinations()
    {
        //if valid add moves to list
        var tiles = new List<Tile>();
        var grid = _currentTile.GetGridManager();
        var currentPos = _currentTile.Position;
        //todo: can move any number of spaces left, right, forward, backwars, diagnal, as long as the space is valid. 
        //var forward = currentPos + new Vector2Int
        //var left = currentPos + new Vector2Int
        //var right = currentPos + new Vector2Int
        //var back = currentPos + new Vector2Int
        //var frontLeftDiag = currentPos + new Vector2Int
        //var frontRightDiag = currentPos + new Vector2Int
        //var backLeftDiag = currentPos + new Vector2Int
        //var backRightDiag = currentPos + new Vector2Int
        
        return tiles;
    }
}
