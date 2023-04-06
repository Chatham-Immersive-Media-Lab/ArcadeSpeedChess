using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Chess.UI
{
    public class UIGameOverPanel : MonoBehaviour
    {
        public GameObject blackWinsObject;
        [FormerlySerializedAs("whiteWInsObject")] public GameObject whiteWinsObject;
        private Image _image;
        public void Start()
        {
            _image = GetComponent<Image>();
            if (_image != null)
            {
                _image.enabled = false;
            }
            blackWinsObject.SetActive(false);
            whiteWinsObject.SetActive(false);
        }

        public void SetVictory(PieceColor winner)
        {
            if (_image != null)
            {
                _image.enabled = true;
            }
            gameObject.SetActive(true);
            if (winner == PieceColor.Black)
            {
                blackWinsObject.SetActive(true);
            }else if (winner == PieceColor.White)
            {
                whiteWinsObject.SetActive(true);
            }
        }
    }
}