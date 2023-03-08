using System.Collections;
using UnityEngine;

namespace Chess
{
	public class Move
	{
		public Piece MovingPiece;//The piece to highlight
		public Tile Destination;

		public virtual void Execute()
		{
			//note: we need to unsure we set HasMoved to true for the piece... basically only a concern for pawn promotion.
			MovingPiece.Move(Destination);
		}

	}
}