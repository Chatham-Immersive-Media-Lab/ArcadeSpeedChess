using System.Collections.Generic;
using Scipts;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceColor Color => _pieceColor;
    protected PieceColor _pieceColor;
    protected bool _hasMoved; //Todo: initialize this correctly
    protected Tile _currentTile;

    public static PieceColor OppositeColor(PieceColor pieceColor)
    {
        if (pieceColor == PieceColor.Black)
        {
            return PieceColor.White;
        }
        
        if (pieceColor == PieceColor.White)
        {
            return PieceColor.Black;
        }

        Debug.Log("wtf");
        
        return PieceColor.White;
    }
    
    public void Move(Tile tile)
    {
        _hasMoved = true;
        if (!tile.IsEmpty())
        {
            //if this is an opponent piece...
            if (tile.GetPieceHere().Color == OppositeColor(_pieceColor))
            {
                tile.GetPieceHere().CaptureMe();
            }
            else
            {
                Debug.LogError("invalid movement");
            }
        }
        
        transform.position = tile.transform.position;
        _currentTile = tile;
        tile.SetPiece(this);

    }

    public virtual List<Tile> ValidDestinations()
    {
        var grid = _currentTile.GetGridManager();
        return new List<Tile>();
    }

    public void CaptureMe()
    {
        _currentTile.ClearPiece();
        Destroy(gameObject);
        //Todo: probably should have a manager for capture pieces
    }

    public virtual bool IsValidDestination(Tile tile)
    {
        if (tile.IsEmpty())
        {
            return true;
        }
        else
        {
            return OppositeColor(tile.GetPieceHere().Color) == _pieceColor; //is opponent piece
        }
    }

    public int GetFacingDirection()
    {
        if (_pieceColor == PieceColor.White)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    

}