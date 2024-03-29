﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
	public class PlayerInputSquare : MonoBehaviour
	{
		[SerializeField] private Player _player;
		[SerializeField] private PromotionDialogue _promotionUI;
		private SpriteRenderer _spriteRenderer;
		public bool IsActive => _isActive;
		private bool _isActive;
		private Tile selectedTile;
		private readonly Dictionary<Tile,Move> _availableMoves = new Dictionary<Tile, Move>();
		private readonly List<Tile> _inputOptionTiles = new List<Tile>();
		private static Dictionary<Vector2Int, Dictionary<Vector2Int, float>> bestTileDirLookup = new Dictionary<Vector2Int, Dictionary<Vector2Int, float>>();
		//keep a list of available tiles.
		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			SetInputActive(false); //as long as 
		}

		private void OnEnable()
		{
			_player.OnInputStateChange += OnInputStateChange;
		}

		private void OnDisable()
		{
			_player.OnInputStateChange -= OnInputStateChange;
		}

		public void Move(Vector2Int dir)
		{
			if (_player.State == InputState.NotGameplay || _player.State == InputState.StartSplash)
			{
				return;
			}
			//selecting pieces when its not our turn might be important for speedchess.
			if (!_isActive)
			{
				return;
			}

			if (_player.State == InputState.ChoosePawnPromotionPiece)
			{
				if (dir.x == -1)
				{
					_promotionUI.CycleLeft();
				}else if (dir.x == 1)
				{
					_promotionUI.CycleRight();
				}

				return;
			}
			
			var best = GetBestMovementTile(selectedTile,dir,_inputOptionTiles);
			if (best != null)
			{
				PutSelfOnTile(best);
			}
		}

		public void PutSelfOnTile(Tile tile)
		{
			selectedTile = tile;
			transform.position = tile.transform.position;
		}

		public void Select()
		{
			if (!_isActive)
			{
				return;
			}

			switch (_player.State)
			{
				case InputState.ChoosingMove:
					_player.Move(_availableMoves[selectedTile]);
					break;
				case InputState.ChoosingPiece:
					_player.ChoosePieceToMove(selectedTile.GetPieceHere());
					break;
				case InputState.StartSplash:
					_player.CallReadyToStartGame();
					break;
				case InputState.ChoosePawnPromotionPiece:
					_promotionUI.DisplayPromotionPanel(false);
					_player.OnPlayerFinishedChoosingPawnPromotionPiece(_promotionUI.GetChosenPiecePrefab());
					break;
				default:
					return;
			}
		}

		private void SetInputActive(bool active)
		{
			_isActive = active;
			_spriteRenderer.enabled = active;
		}

		private void OnInputStateChange(InputState state)
		{
			// selectedTile = null;
			ClearAvailable();
			if (state == InputState.NotMyTurn || state == InputState.NotGameplay)
			{
				SetInputActive(false);
			}else if (state == InputState.ChoosingPiece)
			{
				SetInputActive(true);
				//get the players available moves. Put ourselves on the closest one.
				var pieces = _player.GetAvailablePieces();
				foreach (var piece in pieces)
				{
					_inputOptionTiles.Add(piece.Tile);
					piece.Tile.SetHighlight(true);
				}
				SnapToClosestAvailable();
			}else if (state == InputState.ChoosingMove)
			{
				SetInputActive(true);
				var moves = _player.SelectedPiece.AvailableMoves();
				//list is readonly so we have to update it one by one. ... maybe it should not be read only.
				foreach (var move in moves)
				{
					//add to movement options and highlight it
					_inputOptionTiles.Add(move.Destination);//we need a local list of tiles for moving the input square around
					move.Destination.SetHighlight(true);
					
					//add tile.
					_availableMoves.Add(move.Destination,move);
				}
				SnapToClosestAvailable();
			}else if (state == InputState.ChoosePawnPromotionPiece)
			{
				SetInputActive(true);
				//open pop-up. Activate piece selection, and wait for callback that it finished selection.
				//then call this move with the correct piece.
				_promotionUI.DisplayPromotionPanel(true);
				// _player.OnPlayerFinishedChoosingPawnPromotionPiece(null);
			}else if (state == InputState.StartSplash)
			{
				SetInputActive(true);//turns on the sprite renderer
				_spriteRenderer.enabled = false;//doesnt.
			}
		}

		private void ClearAvailable()
		{
			foreach (Tile t in _inputOptionTiles)
			{
				t.SetHighlight(false);
			}
			
			_inputOptionTiles.Clear();
			_availableMoves.Clear();
		}

		private void SnapToClosestAvailable()
		{
			if (_inputOptionTiles.Count == 0)
			{
				//No available moves!
				//do... we lose?
				//todo:
				return;
			}
			
			float distance = Mathf.Infinity;
			Tile closest = null;
			foreach (Tile t in _inputOptionTiles)
			{
				float d = Vector3.Distance(transform.position, t.transform.position);
				if (d < distance)
				{
					closest = t;
					distance = d;
				}
			}
			PutSelfOnTile(closest);
		}

		/// <summary>
		/// Given a list of possible moves, a current position, and a desired direction, determine the best move to make, if any. This function may return null.
		/// </summary>
		public static Tile GetBestMovementTile(Tile current, Vector2Int dir,List<Tile> tiles)
		{
			//lazy init dictionary
			if (!bestTileDirLookup.ContainsKey(dir))
			{
				bestTileDirLookup.Add(dir,new Dictionary<Vector2Int, float>());
			}

			var lookup = bestTileDirLookup[dir];
			Tile best = null;
			float highestRank = Mathf.NegativeInfinity;
			foreach (var tile in tiles)
			{
				Vector2Int offset = tile.Position - current.Position;
				
				//Calculate or lookup rank for this offset.
				float rank;
				if (lookup.ContainsKey(offset))
				{
					rank = lookup[offset];
				}
				else
				{
					rank = GetMovementRank(dir, offset);
					lookup.Add(offset,rank);
				}
				//see if its the new best. Only positive ranks allowed. Negative means "Not moving is preferred to choosing this space to move to". 
				if (rank > 0 && rank > highestRank)
				{
					highestRank = rank;
					best = tile;
				}
			}

			return best;
		}

		/// <summary>
		/// Given a direction and the offset of the chosen square, calculate an arbitrary float whose value will be higher for better pieces to move to, and negative for pieces we should never move to.
		/// </summary>
		public static float GetMovementRank(Vector2Int dir, Vector2Int offset)
		{
			//Everything is in place, now we just need to write an algorithm that calculates the move.
			//if the offset given should not be selected, we return any negative number. Negative ranks don't need to be sorted.
			
			float angle = Vector2.Angle(dir, offset);//unsigned, should always be positive.
			
			//first, lets quickly cull out any bad angles before bothering to do the slow distance calculation.
			if (angle >= 90)
			{
				//todo: this prevents wrapping from being allowed. We may want to change these offsets to non-wrapped out-of-bounds coordinates, and then calculate the rank for those, but with a rank penalty.
				return -10;
			}

			angle = angle / 90;//normalize to value from 0 to 1.
			if (angle == 0)
			{
				//dont divide by zero, but rank should basically always choose the closest piece with 0 angle.
				angle = 0.001f;
			}
			
			float distance = offset.magnitude/8f;//maximum move distance should be 8, so this attempts to normalize distance to between 0 and 1.

			if (distance == 0)
			{
				//we dont value being on the same square as ourselves. this shouldn't happen tho.
				return -1;
			}
			
			//The numerators are how much we factor in both of these measurements.  
			//we super prefer close angles (hence the square), then we will prefer closer distances (big numerator)
			float rank = (50/distance) * (1/(angle*angle));
			return rank;
		}

	}
}

