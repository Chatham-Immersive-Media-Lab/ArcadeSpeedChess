using System.Collections;

namespace Chess
{
	public class Castle : Move
	{
		public Rook Rook;
		public Tile RookDestination;
		public override void Execute()
		{
			//move the king...
			base.Execute();
			//also move the rook.
			Rook.Move(RookDestination);
		}
	}
}