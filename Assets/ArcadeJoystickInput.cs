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

        private bool _moved;
        private Vector2 _move;
        private PlayerInputDeviceSetter _playerInputSetter;

        private void Awake()
        {
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
            if (!_playerInputSetter.PlayerHasBeenAssigned)
            {
                //leave if we don't have the controller yet, because it could be invalid joystick data from other input.
                return;
            }
            
            _move = value.Get<Vector2>();
        }


        private void Update()
        {
            if (_moved)
            {
                if (_move == Vector2.zero)
                {
                    _moved = false;
                    return;
                }
            }
            else
            {
                if (_move != Vector2.zero)
                {
                    var dir = new Vector2Int((int)_move.x, (int)_move.y);
                    _inputSquare.Move(dir);
                    _moved = true;
                }
            }
        }
    }
}
