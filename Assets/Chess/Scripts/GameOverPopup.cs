using TMPro;
using UnityEngine;

namespace Chess
{
    public class GameOverPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private TextMeshProUGUI winnerText = null;

        public void ActivateGameOver(ChessPieceTeam winner)
        {
            canvasGroup.alpha = 0.75f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            if (winner == ChessPieceTeam.White)
            {
                winnerText.text = "White won";
            }
            else
            {
                winnerText.text = "Black won";
            }
        }
    }
}
