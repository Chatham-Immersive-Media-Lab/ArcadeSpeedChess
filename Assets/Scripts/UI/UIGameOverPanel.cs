using UnityEngine;
using UnityEngine.Serialization;

namespace Chess.UI
{
    public class UIGameOverPanel : MonoBehaviour
    {
        public GameObject blackWinsObject;
        [FormerlySerializedAs("whiteWInsObject")] public GameObject whiteWinsObject;
        public void Start()
        {
            blackWinsObject.SetActive(false);
            whiteWinsObject.SetActive(false);
        }

        public void SetVictory(PieceColor winner)
        {
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