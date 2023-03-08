using System;
using System.Diagnostics;
using UnityEngine;

namespace Chess
{
	public class ChessTimer
	{
		private readonly Stopwatch _whiteTime;
		private TimeSpan _whiteBonusTime;
		private readonly Stopwatch _blackTime;
		private TimeSpan _blackBonusTime;
		
		private TimeSpan _totalTime;
		
		private PieceColor _currentPlayer = PieceColor.White;
		private bool _active;

		public ChessTimer(TimeSpan total)
		{
			_totalTime = total;
			_whiteTime = new Stopwatch();
			_whiteBonusTime = TimeSpan.Zero;
			_blackTime = new Stopwatch();
			_blackBonusTime = TimeSpan.Zero;
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

		public void AddTimeToPlayer(PieceColor pieceColor, int secondsToAdd)
		{
			if (pieceColor == PieceColor.White)
			{
				_whiteBonusTime = _whiteBonusTime.Add(new TimeSpan(0,0,0, secondsToAdd));
			}
			else if (pieceColor == PieceColor.Black)
			{
				_blackBonusTime = _blackBonusTime.Add(new TimeSpan(0, 0, 0, secondsToAdd));
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
				return _totalTime.Subtract(_whiteTime.Elapsed).Add(_whiteBonusTime).ToString("mm\\:ss\\.ff");
			}
			else if (color == PieceColor.Black)
			{
				return _totalTime.Subtract(_blackTime.Elapsed).Add(_blackBonusTime).ToString("mm\\:ss\\.ff");
			}

			return "?";
		}

		public bool IsPlayerOutOfTime(PieceColor color)
		{
			if (color == PieceColor.White)
			{
				return _totalTime.Subtract(_whiteTime.Elapsed).Add(_whiteBonusTime).TotalMilliseconds <= 0;
			}
			else if (color == PieceColor.Black)
			{
				return _totalTime.Subtract(_blackTime.Elapsed).Add(_blackBonusTime).TotalMilliseconds <= 0;
			}

			return false;
		}
	}
}