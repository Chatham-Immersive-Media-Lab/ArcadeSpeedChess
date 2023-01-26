using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Chess
{
    
    public class Piece : MonoBehaviour
    {
        public PieceColor Color => _pieceColor;
        protected PieceColor _pieceColor;
        protected bool _hasMoved; //Todo: initialize this correctly
        public Tile Tile => _currentTile;
        protected Tile _currentTile;
        private GameManager _gameManager;
        protected virtual string DisplayName => "Piece";

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


        public void Init(GameManager gameManager, Tile startingTile, PieceColor color)
        {
            //Keep a reference so we can tell the game manager when we get captured, or subscribe to some event if needed.
            _gameManager = gameManager;
            
            if (!startingTile.IsEmpty())
            {
                Debug.LogError("There is already a piece on this tile, cannot init",startingTile);
            }

            //I hate this coupling, there should be one function to do this.... and that function is "Init". its fine.
            startingTile.SetPiece(this);
            _currentTile = startingTile;
            
            _pieceColor = color;
            transform.position = startingTile.transform.position;
            //todo: update graphics to display white or black sprite.
            Color c = _pieceColor == PieceColor.Black ? new Color(0.1f, 0.1f, 0.1f, 1) : new Color(0.9f, 0.9f, 0.9f, 1);
            Color textCol = _pieceColor != PieceColor.Black ? new Color(0.1f, 0.1f, 0.1f, 1) : new Color(0.9f, 0.9f, 0.9f, 1);
            GetComponent<SpriteRenderer>().color = c;
            GetComponentInChildren<TMP_Text>().color = textCol;
            
            
            //First I wrote this: +(this.GetType().ToString().Split('.')[1]);
            //before deciding to add a display name to the piece class.
            //It would be easier to have the piece class have a display name...
            gameObject.name = _pieceColor.ToString() + DisplayName;
            
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
            _gameManager.PieceCaptured(this);
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

        public void AddValidTilesInDirection(Vector2Int direction, ref List<Tile> tiles,
            bool includeCaptureSquare = true)
        {
            var currentPos = _currentTile.Position;
            var grid = _currentTile.GetGridManager();
            var testPosition = currentPos + direction;
            var testTile = grid.GetTileAtPosition(testPosition);
            while (testTile != null && testTile.IsEmpty())
            {
                tiles.Add(testTile);
                testTile = grid.GetTileAtPosition(testTile.Position + direction);
            }

            if (includeCaptureSquare)
            {
                if (testTile != null && !testTile.IsEmpty())
                {
                    if (OppositeColor(testTile.GetPieceHere().Color) == _pieceColor)
                    {
                        tiles.Add(testTile);
                    }
                }
            }
        }

    }
}