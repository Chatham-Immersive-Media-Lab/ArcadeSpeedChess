using System;
using TMPro;
using UnityEngine;

namespace Chess.UI
{
	public class UIDisplayTime : MonoBehaviour
	{
		[SerializeField] PieceColor myColor;
		private GameManager _gameManager;

		private TMP_Text _text;

		private void Awake()
		{
			//dont yell at me, it's FINE.
			_text = GetComponent<TMP_Text>();
			_gameManager = GameObject.FindObjectOfType<GameManager>();
		}

		void Update()
		{
			_text.text = _gameManager.Timer.GetDisplayText(myColor);
		}

	}
}