using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class King : Piece
    {
        protected override string DisplayName => "King";

        public override List<Tile> ValidDestinations()
        {
            tiles.Clear();
            var gridManager = _currentTile.GetGridManager();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
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
            //Detect if the rook has not moved

            
            return tiles;
        }

        public override List<Move> AvailableMoves()
        {
            var moves= base.AvailableMoves();
            //remove spaces where king is in check.
            for (int i = moves.Count-1; i >= 0; i--)
            {
                if(_gameManager.IsSpaceInCheck(moves[i].Destination.Position, OppositeColor(_pieceColor)))
                {
                    moves.Remove(moves[i]);
                }
            }
            //if we are white or black, set the values for "nearside spaces" and "farside spaces" values accordingly.
            //todo: rename "near" to "kingside"
            //todo: rename "far" to "queenside"
            Vector2Int nearRookPos = Vector2Int.zero;
            Vector2Int farRookPos = Vector2Int.zero;
            List<Vector2Int> nearBetweenPos = new List<Vector2Int>(); //empty spaces between king and rook. We have to check that the king does not "pass through" check for these spaces.
            List<Vector2Int> farBetweenPos = new List<Vector2Int>(); //empty spaces between king and rook. We have to check that the king does not "pass through" check for these spaces.

            if (_pieceColor == PieceColor.White)
            {
                nearRookPos = new Vector2Int(7, 0); //bottom right
                farRookPos = new Vector2Int(0, 0);//bottom left
                nearBetweenPos.Add(new Vector2Int(6, 0));
                nearBetweenPos.Add(new Vector2Int(5, 0));
                farBetweenPos.Add(new Vector2Int(1, 0));
                farBetweenPos.Add(new Vector2Int(2, 0));
                farBetweenPos.Add(new Vector2Int(3, 0));
            }else if (_pieceColor == PieceColor.Black)
            {
                nearRookPos = new Vector2Int(7, 7); //top right
                farRookPos = new Vector2Int(7, 7); //top left
                nearBetweenPos.Add(new Vector2Int(6, 7));
                nearBetweenPos.Add(new Vector2Int(5, 7));
                farBetweenPos.Add(new Vector2Int(1, 7));
                farBetweenPos.Add(new Vector2Int(2, 7));
                farBetweenPos.Add(new Vector2Int(3, 7));
            }
            
            if (!_hasMoved && !IsInCheck())//and if not in check
            {
                //check nearside 
                //Get the value of the nearside rook (if there is a piece at that position, and it has not moved (and thus must be the rook)
                var nearRook = _gameManager.GridManager.GetTileAtPosition(nearRookPos).GetPieceHere();
                var farRook = _gameManager.GridManager.GetTileAtPosition(farRookPos).GetPieceHere();
                if (nearRook != null)
                {
                    if (!nearRook.HasMoved)
                    {
                        //we have not moved the king
                        //we have not moved the rook
                        //we are not in check
                        //check for empty spaces and that those spaces aren't in check
                        bool canCastleNear = true;
                        foreach (var space in nearBetweenPos)
                        {
                            var tile = _gameManager.GridManager.GetTileAtPosition(space);
                            if (tile.GetPieceHere() != null)
                            {
                                canCastleNear = false;
                                break;
                            }
                            else
                            {
                                if (_gameManager.IsSpaceInCheck(space, OppositeColor(_pieceColor)))
                                {
                                    canCastleNear = false;
                                    break;
                                }
                            }
                        }

                        if (canCastleNear)
                        {
                            Tile rookDest = null;
                            Tile kingDest = null;
                            if (_pieceColor == PieceColor.White)
                            {
                                rookDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(5, 0));
                                kingDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(6, 0));
                            }
                            else
                            {
                                rookDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(5, 7));
                                kingDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(6, 7));
                            }

                            Castle move = new Castle()
                            {
                                Rook = (Rook)nearRook,
                                RookDestination = rookDest,
                                MovingPiece = this,
                                Destination = kingDest,
                            };
                            moves.Add(move);
                        }
                    }
                }

                if (farRook != null)
                {
                    if (!farRook.HasMoved)
                    {
                        bool canCastleFar = true;
                        
                        foreach (var space in farBetweenPos)
                        {
                            var tile = _gameManager.GridManager.GetTileAtPosition(space);
                            if (tile.GetPieceHere() != null)
                            {
                                canCastleFar = false;
                                break;
                            }
                            else
                            {
                                if (_gameManager.IsSpaceInCheck(space, OppositeColor(_pieceColor)))
                                {
                                    canCastleFar = false;
                                    break;
                                }
                            }
                        }

                        //we have not moved the king
                        //we have not moved the rook
                        //we are not in check
                        //All spaces between king and rook are empty and they are not in check.
                        if (canCastleFar)
                        {
                            Tile rookDest = null;
                            Tile kingDest = null;
                            if (_pieceColor == PieceColor.White)
                            {
                                rookDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(3, 0));
                                kingDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(2, 0));
                            }
                            else
                            {
                                rookDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(3, 7));
                                kingDest = _gameManager.GridManager.GetTileAtPosition(new Vector2Int(2, 7));
                            }

                            Castle move = new Castle()
                            {
                                Rook = (Rook)nearRook,
                                RookDestination = rookDest,
                                MovingPiece = this,
                                Destination = kingDest,
                            };
                            moves.Add(move);
                        }
                    }
                }
            }

            return moves;
        }

        public bool IsInCheck()
        {
            return _gameManager.IsSpaceInCheck(_currentTile.Position, OppositeColor(_pieceColor));
        }
    }
}