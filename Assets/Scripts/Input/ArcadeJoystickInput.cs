using System;
using System.Collections;
using System.Collections.Generic;
using ArcadeCabinet;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chess
{
    public class ArcadeJoystickInput : MonoBehaviour
    {
        private PlayerInputSquare _inputSquare;
        private PlayerInput _playerInput;
        private PlayerInputDeviceSetter _playerInputSetter;
        private Vector2Int previous;
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _inputSquare = GetComponent<PlayerInputSquare>();
            _playerInputSetter = GetComponent<PlayerInputDeviceSetter>();
        }

        public void OnAction(InputValue value)
        {
            //We don't need to wait for Assigned yet, because blue will only ever receive blue.
            _inputSquare.Select();
        }
        public void OnMove(InputValue value)
        {
            //both joysticks get read as the same joystick.
            if (!_playerInputSetter.PlayerHasBeenAssigned)
            {
                return;
            }

            var dir = value.Get<Vector2>();
            //We don't need to wait for Assigned yet, because blue will only ever receive blue.
            if (dir == Vector2.zero)
            {
                previous = Vector2Int.zero;
            }
            else
            {
                var current = AsVec2Int(dir);
                if (current != previous)
                {
                    // //avoid diagonals
                    // if (current.x == 0 || current.y == 0)
                    // {
                    //     _inputSquare.Move(current);
                    // }
                    _inputSquare.Move(current);
                }

                previous = current;
            }
        }

        private Vector2Int AsVec2Int(Vector2 dir)
        {
            int x = dir.x > 0.5 ? 1 : (dir.x < -0.5 ? -1 : 0);
            int y = dir.y > 0.5 ? 1 : (dir.y < -0.5 ? -1 : 0);

            return new Vector2Int(x, y);
        }
    }
}
