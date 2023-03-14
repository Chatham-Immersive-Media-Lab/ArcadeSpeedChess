using System;
using System.Net.NetworkInformation;
using Chess.UI;
using UnityEngine;

namespace Chess
{
	public class GameSplash : MonoBehaviour
	{
		public static Action OnStartGame;
		
		private bool _isBlackReady = false;
		private bool _isWhiteReady = false;

		[SerializeField] private GameObject _splashPanel;
		[SerializeField] private UIReadyStatus _whiteReadyStatus;
		[SerializeField] private UIReadyStatus _blackReadyStatus;
		
		public void Update()
		{
			if (_isBlackReady && _isWhiteReady)
			{
				StartGame();
			}
		}

		private void StartGame()
		{
			//turn off the ui panel for it
			OnStartGame?.Invoke();
		}

		public void SetPlayerReady(PieceColor color)
		{
			if (color == PieceColor.Black)
			{
				_isBlackReady = true;
			}else if (color == PieceColor.White)
			{
				_isWhiteReady = true;
			}
		}

		//not needed in current setup but shh
		public void Display()
		{
			_splashPanel.SetActive(true);
		}
	}
}