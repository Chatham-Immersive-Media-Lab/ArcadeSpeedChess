using System.Collections;
using UnityEngine;

namespace Chess.Move
{
	public class Move
	{
		public PieceColor MovingColor;
		public Piece MovingPiece;//The piece to highlight
		public Vector2Int Destination;

		public IEnumerator Execute()
		{
			yield break;
		}
	}
}