using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
	public class MoveManager : MonoBehaviour
	{
		private static List<Move> moves = new List<Move>();


		public static void ExecuteMove(Move move)
		{
			move.Execute();
			moves.Add(move);
		}

		public static Move GetLastMove()
		{
			if (moves.Count > 0)
			{
				return moves[^1];
			}
			else
			{
				return null;
			}

		}
	}
}