using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    
public class Pawn : Piece
{
    protected override string DisplayName => "Pawn";

    public override List<Tile> ValidDestinations()
    { 
        //if valid add moves to list
        tiles.Clear();
        var grid = _currentTile.GetGridManager();
        var currentPos = _currentTile.Position;
        var upOne = currentPos + new Vector2Int(0, GetFacingDirection());
        var upTwo = currentPos + new Vector2Int(0, GetFacingDirection()*2);
        var upLeftDiag = upOne + Vector2Int.left;
        var upRightDiag = upOne + Vector2Int.right;

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
            if (upOneTile.IsEmpty() && upTwoTile.IsEmpty() && !_hasMoved)
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
        //Piece upgrade... don't bother when testing for check.
        
        
        return tiles;
    }

    public override List<Move> AvailableMoves()
    {
        var moves = new List<Move>();
        Vector2Int twoOffset = new Vector2Int(0, GetFacingDirection() * 2);
        foreach (var tile in ValidDestinations())
        {
            if ((tile.Position - _currentTile.Position) == twoOffset)
            {
                var skippedPos = _currentTile.Position + new Vector2Int(0, GetFacingDirection());
                PawnMoveTwo m = new PawnMoveTwo()
                {
                    skippedTile = _gameManager.GridManager.GetTileAtPosition(skippedPos),
                    MovingPiece = this,
                    Destination = tile
                };
                moves.Add(m);
            }
            else
            {
                Move m = new Move()
                {
                    MovingPiece = this,
                    Destination = tile
                };
                moves.Add(m);
            }
        }

        //todo: piece promotion
        
        //En Passant
        var lastMove = MoveManager.GetLastMove();
        //if the very previous move was the opponent moving a pawn two squares forward
        if (lastMove != null && lastMove is PawnMoveTwo theirMove)
        {
            if (lastMove.MovingPiece is Pawn otherPawn)
            {
                var offset = Mathf.Abs(lastMove.Destination.Position.x - _currentTile.Position.x);
                if (offset == 1)
                {
                    //we COULD en passant!
                    var move = new EnPassant();
                    move.AdjacentPawn = otherPawn;
                    move.Destination = theirMove.skippedTile;//we know we wont capture here because they JUST moved through the empty space.
                    move.MovingPiece = this;
                    moves.Add(move);
                }
            }
        }
        //if we are adjacent to that pawn piece
        return moves;
    }
}
}
