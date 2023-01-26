using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
	public class PlayerInputSquare : MonoBehaviour
	{
		[SerializeField] private Player _player;
		private SpriteRenderer _spriteRenderer;
		public bool IsActive => _isActive;
		private bool _isActive;

		private Tile selectedTile;
		private readonly List<Tile> _availableTiles = new List<Tile>();

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
			//selecting pieces when its not our turn might be important for speedchess.
			if (!_isActive)
			{
				return;
			}
			//
			//this will get replaced, so whatever.
			int index = _availableTiles.IndexOf(selectedTile);
			if (_availableTiles.Count == 0)
			{
				Debug.Log("Can't move!");
				return;
			}
			
			if (dir.x > 0)
			{
				index++;
				if (index >= _availableTiles.Count)
				{
					index = 0;
				}
			}
			else
			{
				index--;
				if (index < 0)
				{
					index = _availableTiles.Count - 1;
				}
			}
			PutSelfOnTile(_availableTiles[index]);
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
			
			if (_player.State == InputState.ChoosingPiece)
			{
				_player.ChoosePieceToMove(selectedTile.GetPieceHere());
				//have chosen this piece to select a move from.
			}else if (_player.State == InputState.ChoosingMove)
			{
				//we have chosen this piece to move to.
				_player.Move(_player.SelectedPiece,selectedTile);
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
					_availableTiles.Add(piece.Tile);
					piece.Tile.SetHighlight(true);
				}
				SnapToClosestAvailable();
			}else if (state == InputState.ChoosingMove)
			{
				SetInputActive(true);
				var destinations = _player.SelectedPiece.ValidDestinations();
				//list is readonly so we have to update it one by one. ... maybe it should not be read only.
				foreach (var tile in destinations)
				{
					_availableTiles.Add(tile);
					tile.SetHighlight(true);
				}
				SnapToClosestAvailable();
			}
		}

		private void ClearAvailable()
		{
			foreach (Tile t in _availableTiles)
			{
				t.SetHighlight(false);
			}
			_availableTiles.Clear();
		}

		private void SnapToClosestAvailable()
		{
			if (_availableTiles.Count == 0)
			{
				//No available moves!
				//do... we lose?
				//todo:
				return;
			}
			
			float distance = Mathf.Infinity;
			Tile closest = null;
			foreach (Tile t in _availableTiles)
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
	}
}