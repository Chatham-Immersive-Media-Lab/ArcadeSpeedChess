using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    
    public override List<Tile> ValidDestinations()
    { 
        //if valid add moves to list
        var tiles = new List<Tile>();
        var grid = _currentTile.GetGridManager();
        var currentPos = _currentTile.Position;
        var upOne = currentPos + new Vector2Int(0, GetFacingDirection());
        var upTwo = currentPos + new Vector2Int(0, GetFacingDirection()*2);
        var upLeftDiag = upOne + Vector2Int.left;
        var upRightDiag = upOne + Vector2Int.right;
        //todo: en passant 
        //todo: piece promotion

        var upOneTile = grid.GetTileAtPosition(upOne);
        var upTwoTile = grid.GetTileAtPosition(upTwo);
        var upLeftDiagTile = grid.GetTileAtPosition(upLeftDiag);
        var upRightDiagTile = grid.GetTileAtPosition(upRightDiag);

        if (upOneTile != null)
        {
            if (upOneTile.IsEmpty())
            {
                tiles.Add(upOneTile);
            }
        }
        
        if (upTwoTile != null)
        {
            if (upTwoTile.IsEmpty() && !_hasMoved)
            {
                tiles.Add(upTwoTile);
            }
        }
        
        if (upLeftDiagTile != null)
        {
            if (!upLeftDiagTile.IsEmpty())
            {
                if (OppositeColor(upLeftDiagTile.GetPieceHere().Color) == _pieceColor)
                {
                    tiles.Add(upLeftDiagTile);
                }
            }
        }
        
        if (upRightDiagTile != null)
        {
            if (!upRightDiagTile.IsEmpty())
            {
                if (OppositeColor(upRightDiagTile.GetPieceHere().Color) == _pieceColor)
                {
                    tiles.Add(upRightDiagTile);
                }
            }
        }
        
        return tiles;
    }
    
    
}