using System;
using UnityEngine;

namespace Chess.UI
{
	public class UIReadyStatus : MonoBehaviour
	{
		public GameObject ReadyTrueObject;
		public GameObject ReadyFalseObject;
		private bool _status;
		[SerializeField] private bool defaultStatus;

		private void Awake()
		{
			SetReadyStatus(defaultStatus);
		}

		public void SetReadyStatus(bool status)
		{
			_status = status;
			ReadyTrueObject.SetActive(status);
			ReadyFalseObject.SetActive(!status);
		}
	}
}