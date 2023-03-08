namespace Chess
{
	public class EnPassant : Move
	{
		public Piece AdjacentPawn;
		public override void Execute()
		{
			AdjacentPawn.CaptureMe();
			base.Execute();
		}
	}
}