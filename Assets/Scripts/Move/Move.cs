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
			//todo: when creating a default move, we can put the check for capture logic there. This would be a step needed to add undo support.
			
			//if this is an opponent piece...
			if (Destination.GetPieceHere() != null)
			{
				if (Destination.GetPieceHere().Color == Piece.OppositeColor(MovingPiece.Color))
				{
					Destination.GetPieceHere().CaptureMe();
				}
			}

			//note: we need to unsure we set HasMoved to true for the piece... basically only a concern for pawn promotion.
			MovingPiece.Move(Destination);
		}
	}
}