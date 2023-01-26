using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
	public class ChessBoardInitializer : MonoBehaviour
	{
		[SerializeField] private Piece pawnPrefab;
		[SerializeField] private Piece rookPrefab;
		[SerializeField] private Piece knightPrefab;
		[SerializeField] private Piece bishopPrefab;
		[SerializeField] private Piece queenPrefab;
		[SerializeField] private Piece kingPrefab;

		private GameManager _gameManager;
		private List<Piece> _createdPieces;//these doesn't follow our kinda-functional or kinda-static pattern of having all relevant references injected, but.... its fine? I would rather this all be a static utility class than a monobehaviour.
		//and I would pass in a ScriptableObject reference that keeps the set of prefabs to spawn in. Then we can make multiple sets easily, and change "theme" more easily too.
		//but this works.
		public List<Piece> CreateStartingChessBoard(GameManager gameManager, GridManager grid)
		{
			_createdPieces = new List<Piece>();
			//Pawns
			for (int i = 0; i < 8; i++)
			{
				Vector2Int blackPawnPos = new Vector2Int(i, 6);
				PutPieceOnPosition(grid,pawnPrefab,PieceColor.Black,blackPawnPos);
				Vector2Int whitePawnPos = new Vector2Int(i, 1);
				PutPieceOnPosition(grid,pawnPrefab, PieceColor.White, whitePawnPos);
			}
			
			//rooks
			PutPieceOnPosition(grid,rookPrefab, PieceColor.Black, new Vector2Int(0,7));
			PutPieceOnPosition(grid,rookPrefab, PieceColor.Black, new Vector2Int(7, 7));
			PutPieceOnPosition(grid,rookPrefab, PieceColor.White, new Vector2Int(0, 0));
			PutPieceOnPosition(grid,rookPrefab, PieceColor.White, new Vector2Int(7, 0));
			
			//knights
			PutPieceOnPosition(grid,knightPrefab, PieceColor.Black, new Vector2Int(1, 7));
			PutPieceOnPosition(grid,knightPrefab, PieceColor.Black, new Vector2Int(6, 7));
			PutPieceOnPosition(grid,knightPrefab, PieceColor.White, new Vector2Int(1, 0));
			PutPieceOnPosition(grid,knightPrefab, PieceColor.White, new Vector2Int(6, 0));
			
			//bishops
			PutPieceOnPosition(grid,bishopPrefab, PieceColor.Black, new Vector2Int(2, 7));
			PutPieceOnPosition(grid,bishopPrefab, PieceColor.Black, new Vector2Int(5, 7));
			PutPieceOnPosition(grid,bishopPrefab, PieceColor.White, new Vector2Int(2, 0));
			PutPieceOnPosition(grid,bishopPrefab, PieceColor.White, new Vector2Int(5, 0));
			
			//Queens
			PutPieceOnPosition(grid,queenPrefab, PieceColor.Black, new Vector2Int(3, 7));
			PutPieceOnPosition(grid,queenPrefab, PieceColor.White, new Vector2Int(3, 0));

			//kings
			PutPieceOnPosition(grid,kingPrefab, PieceColor.Black, new Vector2Int(4, 7));
			PutPieceOnPosition(grid,kingPrefab, PieceColor.White, new Vector2Int(4, 0));
			
			//return
			return _createdPieces;
		}

		public void PutPieceOnPosition(GridManager grid, Piece prefab, PieceColor color, Vector2Int position)
		{
			var tile = grid.GetTileAtPosition(position);
			Piece piece = Instantiate(prefab);
			piece.Init(_gameManager,tile,color);
			_createdPieces.Add(piece);
		}
	}
}