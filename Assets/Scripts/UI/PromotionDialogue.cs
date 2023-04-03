using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Chess
{
    public class PromotionDialogue : MonoBehaviour
    {
        [SerializeField] private PromotionOption[] _promotionOptions;
        [SerializeField] private TMP_Text _text;
        private int currentIndex = 0;

        void Start()
        {
            gameObject.SetActive(false);
        }
        public void DisplayPromotionPanel(bool display)
        {
            if (display)
            {
                //reset when turning on.
                currentIndex = 0;
                RefreshUI();
            }

            gameObject.SetActive(display);
        }

        private void RefreshUI()
        {
            _text.text = _promotionOptions[currentIndex].name;
        }

        public void CycleRight()
        {
            currentIndex++;
            if (currentIndex >= _promotionOptions.Length)
            {
                currentIndex = 0;
            }
            RefreshUI();
        }

        public void CycleLeft()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = _promotionOptions.Length - 1;
            }
            RefreshUI();
        }

        public Piece GetChosenPiecePrefab()
        {
            return _promotionOptions[currentIndex].prefab;
        }

        void SelectPiece()
        {
            gameObject.SetActive(false);
            return ;
        }
    }

    [Serializable]
    public class PromotionOption
    {
        public string name;
        public Piece prefab;
    }
}
