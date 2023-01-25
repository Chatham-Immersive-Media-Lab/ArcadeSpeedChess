using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Color baseColor, offsetColor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject highLight;
        private GridManager _gridManager;
        private Piece _currentPiece;
        public Vector2Int Position => _position;
        private Vector2Int _position;

        public void Init(Vector2Int pos, GridManager gridManager)
        {
            _gridManager = gridManager;
            _position = pos;
            var isOffset = (pos.x + pos.y) % 2 == 1;
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }


        private void OnMouseEnter()
        {
            SetHighlight(true);
        }

        private void OnMouseExit()
        {
            SetHighlight(false);
        }

        public void SetHighlight(bool highlight)
        {
            highLight.SetActive(highlight);
        }

        public GridManager GetGridManager()
        {
            return _gridManager;
        }

        public bool IsEmpty()
        {
            return _currentPiece == null;
        }

        public void SetPiece(Piece piece)
        {
            if (_currentPiece != null)
            {
                Debug.LogError("Tile not empty");
            }

            _currentPiece = piece;
        }

        public Piece GetPieceHere()
        {
            return _currentPiece;
        }

        public void ClearPiece()
        {
            _currentPiece = null;
        }


    }
}