using System;
using System.Net.NetworkInformation;
using Chess.UI;
using UnityEngine;

namespace Chess
{
	public class GameSplash : MonoBehaviour
	{
		public static Action OnStartGame;

		private bool _splashOver;
		private bool _isBlackReady = false;
		private bool _isWhiteReady = false;

		[SerializeField] private GameObject _splashPanel;
		[SerializeField] private UIReadyStatus _whiteReadyStatus;
		[SerializeField] private UIReadyStatus _blackReadyStatus;

		private void Start()
		{
			_splashOver = false;
			_whiteReadyStatus.SetReadyStatus(false);
			_blackReadyStatus.SetReadyStatus(false);
		}

		public void Update()
		{
			if (!_splashOver && _isBlackReady && _isWhiteReady)
			{
				StartGame();
			}
		}

		private void StartGame()
		{
			_splashOver = true;
			//turn off the ui panel for it
			OnStartGame?.Invoke();
			
		}

		public void SetPlayerReady(PieceColor color)
		{
			if (color == PieceColor.Black)
			{
				Debug.Log("Black Player Ready");
				_whiteReadyStatus.SetReadyStatus(true);
				_isBlackReady = true;
			}else if (color == PieceColor.White)
			{
				Debug.Log("White Player Ready");
				_isWhiteReady = true;
				_blackReadyStatus.SetReadyStatus(true);
			}
		}

		//not needed in current setup but shh
		public void Display()
		{
			_splashPanel.SetActive(true);
		}
	}
}