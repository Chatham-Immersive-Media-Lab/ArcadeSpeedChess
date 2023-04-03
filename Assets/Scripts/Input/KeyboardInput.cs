using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Chess
{
	[RequireComponent(typeof(PlayerInputSquare))]
	public class KeyboardInput : MonoBehaviour
	{
		private PlayerInputSquare _inputSquare;
		
		//dumb fixes for using the same keyboard input for both players, and having them both press select the same frame. lol oops.
		private static bool _selectedThisFrame;
		private static bool _movedThisFrame;
		public KeyCode selectKey;
		private void Awake()
		{
			_inputSquare = GetComponent<PlayerInputSquare>();
		}

		private void Update()
		{
			if (_inputSquare.IsActive)
			{
				InputTick();
			}
		}

		private void InputTick()
		{
			//Select Input.
			if (Input.GetKeyDown(selectKey) && !_selectedThisFrame)
			{
				_selectedThisFrame = true;
				_inputSquare.Select();
			}

			//dont move if we (the other player) already did.
			if (_movedThisFrame)
			{
				return;
			}

			//this is all just for testing anyway, but pressing up and right at the exact same frame is too hard for this code to work.
			bool right = Input.GetKey(KeyCode.RightArrow);
			bool left = Input.GetKey(KeyCode.LeftArrow);
			bool up = Input.GetKey(KeyCode.UpArrow);
			bool down = Input.GetKey(KeyCode.DownArrow);
			bool rightDown = Input.GetKeyDown(KeyCode.RightArrow);
			bool leftDown = Input.GetKeyDown(KeyCode.LeftArrow);
			bool upDown = Input.GetKeyDown(KeyCode.UpArrow);
			bool downDown = Input.GetKeyDown(KeyCode.DownArrow);

			if (rightDown && !left)
			{
				if (up)
				{
					Move(1,1);
					return;
				}else if (down)
				{
					Move(1,-1);
					return;
				}
				Move(1,0);
				return;
			}
			else if (leftDown && !right)
			{
				if (up)
				{
					Move(-1, 1);
					return;
				}
				else if (down)
				{
					Move(-1, -1);
					return;
				}

				Move(-1, 0);
				return;
			}else if (upDown && !down)
			{
				if (right)
				{
					Move(1, 1);
					return;
				}
				else if (left)
				{
					Move(-1, 1);
					return;
				}

				Move(0, 1);
				return;
			}
			else if (downDown && !up)
			{
				if (right)
				{
					Move(1, -1);
					return;
				}
				else if (left)
				{
					Move(-1, -1);
					return;
				}

				Move(0, -1);
				return;
			}
		}

		private void Move(int x, int y)
		{
			_movedThisFrame = true;
			_inputSquare.Move(new Vector2Int(x, y));
		}

		private void LateUpdate()
		{
			_selectedThisFrame = false;
			_movedThisFrame = false;
		}
	}
}