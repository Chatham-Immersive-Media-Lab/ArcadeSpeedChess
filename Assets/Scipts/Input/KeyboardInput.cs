using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Chess
{
	[RequireComponent(typeof(PlayerInputSquare))]
	public class KeyboardInput : MonoBehaviour
	{
		private PlayerInputSquare _inputSquare;
		
		//dumb fixes for using the same keyboard input for both players, and having them both press select the same frame. lol oops.
		private static bool _selectedThisFrame;
		private static bool _movedThisFrame;
		private void Awake()
		{
			_inputSquare = GetComponent<PlayerInputSquare>();
		}

		private void Update()
		{
			if (_inputSquare.IsActive)
			{
				if (Input.GetKeyDown(KeyCode.Space) && !_selectedThisFrame)
				{
					_selectedThisFrame = true;
					_inputSquare.Select();
				}

				//this is all just for testing anyway, but pressing up and right at the exact same frame is too hard for this code to work.

				bool right = Input.GetKeyDown(KeyCode.RightArrow);
				bool left = Input.GetKeyDown(KeyCode.LeftArrow);
				bool up = Input.GetKeyDown(KeyCode.UpArrow);
				bool down = Input.GetKeyDown(KeyCode.DownArrow);

				int horizontal = 0;
				if (right && !left)
				{
					horizontal = 1;
				}
				else if (left && !right)
				{
					horizontal = -1;
				}

				int vertical = 0;
				if (up && !down)
				{
					vertical = 1;
				}
				else if (down && !up)
				{
					vertical = -1;
				}

				if (horizontal != 0 || vertical != 0 && !_movedThisFrame)
				{
					_movedThisFrame = true;
					_inputSquare.Move(new Vector2Int(horizontal, vertical));
				}
			}
		}

		private void LateUpdate()
		{
			_selectedThisFrame = false;
			_movedThisFrame = false;
		}
	}
}