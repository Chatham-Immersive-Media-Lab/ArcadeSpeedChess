using System;
using System.Diagnostics;
using UnityEngine;

namespace Chess
{
	public class ChessTimer
	{
		private readonly Stopwatch _whiteTime;
		private readonly Stopwatch _blackTime;
		private TimeSpan _totalTime;
		
		private PieceColor _currentPlayer = PieceColor.White;
		private bool _active;

		public ChessTimer(TimeSpan total)
		{
			_totalTime = total;
			_whiteTime = new Stopwatch();
			_blackTime = new Stopwatch();
		}
		public void StartTimeForPlayer(PieceColor pieceColor)
		{
			_active = true;
			_currentPlayer = pieceColor;
			if (_currentPlayer == PieceColor.White)
			{
				_whiteTime.Start();
				_blackTime.Stop();
			}
			else if (_currentPlayer == PieceColor.Black)
			{
				_blackTime.Start();
				_whiteTime.Stop();
			}
		}

		public void Stop()
		{
			_active = false;
		}

		public string GetDisplayText(PieceColor color)
		{
			if (color == PieceColor.White)
			{
				return _totalTime.Subtract(_whiteTime.Elapsed).ToString("mm\\:ss\\.ff");
			}
			else if (color == PieceColor.Black)
			{
				return _totalTime.Subtract(_blackTime.Elapsed).ToString("mm\\:ss\\.ff");
			}

			return "?";
		}
	}
}