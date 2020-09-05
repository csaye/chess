using System.Collections;
using TMPro;
using UnityEngine;

namespace Chess
{
    public class GameOverPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private TextMeshProUGUI winnerText = null;
        [SerializeField] private GameObject playAgainButton = null;

        public void ActivateGameOver(ChessPieceTeam loser)
        {
            canvasGroup.alpha = 0.75f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            if (loser == ChessPieceTeam.White)
            {
                winnerText.text = "Black won";
            }
            else
            {
                winnerText.text = "White won";
            }
            playAgainButton.SetActive(true);
        }
    }
}
