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
        public void DisplayPromotionPanel()
        {
            currentIndex = 0;
            gameObject.SetActive(true);
            RefreshUI();
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
        }

        public void CycleLeft()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = _promotionOptions.Length - 1;
            }
        }

        void SelectPiece()
        {
            gameObject.SetActive(false);
            return ;
        }

        public Piece GetChosenPiece()
        {
            return _promotionOptions[currentIndex].prefab;
        }
    }

    [Serializable]
    public class PromotionOption
    {
        public string name;
        public Piece prefab;
    }
}
