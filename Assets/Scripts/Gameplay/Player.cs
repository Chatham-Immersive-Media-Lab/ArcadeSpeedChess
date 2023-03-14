using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
	//Player doesn't really have a representation in the scene, they're just vibing,
	public class Player : MonoBehaviour
	{
		public Action<InputState> OnInputStateChange;
		//input...
		public PieceColor Color => _myColor; 
		private PieceColor _myColor;
		public InputState State => _inputState;
		private InputState _inputState;

		private GameManager _manager;
		private List<Piece> _myPieces;
		private List<Piece> _piecesWithAvailableMoves;

		public Piece SelectedPiece => _selectedPieceToMove;
		private Piece _selectedPieceToMove;

		public King King;
		
		private void Awake()
		{
			_inputState = InputState.NotGameplay;
		}

		public void Init(GameManager manager, PieceColor color, InputState startingState)
		{
			_manager = manager;
			_myColor = color;
			//todo: subscribe to afterMove event to update availableMoves.
			
			SetInputState(startingState);
		}
		public void SetStartingPieces(List<Piece> startingPieces)
		{
			_myPieces = startingPieces;
			_piecesWithAvailableMoves = new List<Piece>();
			
			//set reference to king for conveninence
			foreach (var piece in _myPieces)
			{
				if (piece is King king)
				{
					this.King = king;
					break;
				}
			}
		}

		//Called by gameManager after one of this players pieces has been captured.
		public void PieceCaptured(Piece piece)
		{
			if (_myPieces.Contains(piece))
			{
				_myPieces.Remove(piece);
			}

			if (_piecesWithAvailableMoves.Contains(piece))
			{
				_piecesWithAvailableMoves.Remove(piece);
			}
		}

		public void SetTurnActive()
		{
			//update available moves
			SetPiecesWithAvailableMoves();
			
			//update the UI/selector
			SetInputState(InputState.ChoosingPiece);
		}

		private void SetInputState(InputState state)
		{
			if (state == _inputState)
			{
				Debug.LogError("Cant set state to current state");
				return;
			}
			_inputState = state;
			//fire of event.
			OnInputStateChange?.Invoke(state);
		}

		public void SetPiecesWithAvailableMoves()
		{
			//This is a seemingly odd way to do things - we are updating a list, where we could just clear and recreate it.
			//I think, soon, we will want to use an OrderedList, and then having less operations will be more efficient, as it inserts items into the list at the correct position.
			//basically that kind of list does a sort when adding pieces
			//most of the time, this list wont actually change much.
			
			foreach(Piece piece in _myPieces)
			{
				var moves  = piece.AvailableMoves();
				if (moves.Count > 0)
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

		public void Move(Move move)
		{
			MoveManager.ExecuteMove(move);
			SetInputState(InputState.NotMyTurn);//this will fire off an event that the input square will listen to, and disable.
			
			//todo: Check if we moved a pawn and it is in a promotion position.
			bool pawnCanPromote = false;
			if (pawnCanPromote)
			{
				SetInputState(InputState.ChoosePawnPromotionPiece);
			}
			else
			{
				_manager.OnPlayerFinishedTurn(this);
			}
		}

		public void OnPlayerFinishedChoosingPawnPromotionPiece(Piece promotionPrefab)
		{
			//swap the pawn for promotionPrefab.
			_manager.OnPlayerFinishedTurn(this);
		}

		public void ChoosePieceToMove(Piece piece)
		{
			if (piece.Color == _myColor)
			{
				_selectedPieceToMove = piece;
				SetInputState(InputState.ChoosingMove);
			}
			else
			{
				Debug.LogError("Cant choose that piece!");
			}
		}

		public List<Piece> GetAvailablePieces()
		{
			return _piecesWithAvailableMoves;
		}

		public void CallReadyToStartGame()
		{
			_manager.SelectReadyToStartGame(this);
		}

		public void NewGame(bool isTurnActive)
		{
			//get ready to start the game.
			SetInputState(InputState.NotMyTurn);
			if(isTurnActive)
			{
				SetTurnActive();
			}
		}
	}
}