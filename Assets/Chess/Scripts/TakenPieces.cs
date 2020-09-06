using UnityEngine;

namespace Chess
{
    public class TakenPieceDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject[] displayChessPieces = new GameObject[12];

        private Transform whitePiecesTransform;
        private Transform blackPiecesTransform;

        private int whitePieces = 0;
        private int blackPieces = 0;

        private void Start()
        {
            whitePiecesTransform = transform.GetChild(0).transform;
            blackPiecesTransform = transform.GetChild(1).transform;
        }

        public void AddDisplayPiece(ChessPiece piece)
        {
            if (piece.team == ChessPieceTeam.White)
            {
                AddWhitePiece((int)piece.type);
            }
            else
            {
                AddBlackPiece((int)piece.type);
            }
        }

        private void AddWhitePiece(int type)
        {
            GameObject piece = displayChessPieces[type];
            whitePieces++;
        }

        private void AddBlackPiece(int type)
        {
            GameObject piece = displayChessPieces[6 + type];
            blackPieces++;
        }
    }
}
