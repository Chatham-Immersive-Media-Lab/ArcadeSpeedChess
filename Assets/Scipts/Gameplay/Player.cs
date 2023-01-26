using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
	//Player doesn't really have a representation in the scene, they're just vibing,
	public class Player : MonoBehaviour
	{
		//input...
		public PieceColor Color => _myColor; 
		private PieceColor _myColor;

		private GameManager _manager;
		private List<Piece> _myPieces;
		private List<Piece> _piecesWithAvailableMoves;
		public void Init(GameManager manager, PieceColor color)
		{
			_manager = manager;
			_myColor = color;
			//todo: subscribe to afterMove event to update availableMoves.
		}
		public void SetStartingPieces(List<Piece> startingPieces)
		{
			_myPieces = startingPieces;
			_piecesWithAvailableMoves = new List<Piece>();
		}

		public void PieceCaptured(Piece piece)
		{
			if (_myPieces.Contains(piece))
			{
				_myPieces.Remove(piece);
			}
		}

		public void SetTurnActive()
		{
			//update available moves
			SetPiecesWithAvailableMoves();
			
			//update the UI?
			
			Debug.Log(_myColor + " Player has "+_piecesWithAvailableMoves.Count+ " Available Moves.");
		}
		
		public void SetPiecesWithAvailableMoves()
		{
			//This is a seemingly odd way to do things - we are updating a list, where we could just clear and recreate it.
			//I think, soon, we will want to use an OrderedList, and then having less operations will be more efficient, as it inserts items into the list at the correct position.
			//basically that kind of list does a sort when adding pieces
			//most of the time, this list wont actually change much.
			
			foreach(Piece piece in _myPieces)
			{
				var destinations  = piece.ValidDestinations();
				if (destinations.Count > 0)
				{
					//this piece has an available move. add them to the list if we need to.
					if (!_piecesWithAvailableMoves.Contains(piece))
					{
						_piecesWithAvailableMoves.Add(piece);
					}
				}
				else
				{
					//no available move.
					if (_piecesWithAvailableMoves.Contains(piece))
					{
						_piecesWithAvailableMoves.Remove(piece);
					}
				}
			}
		}

		public void Move(Piece piece, Tile destination)
		{
			piece.Move(destination);
			_manager.OnPlayerFinishedTurn(this);
		}
	}
}