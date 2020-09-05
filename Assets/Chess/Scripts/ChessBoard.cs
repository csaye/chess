using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject highlight = null;

        private ChessPieceType[,] chessBoard = new ChessPieceType[8, 8];

        private bool pieceClicked;

        private ChessPieceType GetChessPiece(Vector2Int position)
        {
            return chessBoard[position.x, position.y];
        }

        public void ClickChessPiece(ChessPiece piece)
        {
            if (pieceClicked) return;
            pieceClicked = true;
            HighlightMovableSquares(piece);
        }

        private void HighlightMovableSquares(ChessPiece piece)
        {
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsEmpty(movePosition))
                {
                    HighlightSquare(movePosition);
                }
            }
        }

        private void HighlightSquare(Vector2Int position)
        {
            Instantiate(highlight, PieceToHighlightPosition(position), Quaternion.identity, transform);
        }

        private Vector3 PieceToHighlightPosition(Vector2Int position)
        {
            return new Vector3(position.x + 3.5f, position.y + 3.5f, 0);
        }

        private bool IsEmpty(Vector2Int position)
        {
            if (IsOutOfBounds(position)) return false;
            return GetChessPiece(position) == ChessPieceType.Empty;
        }

        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x > 8 || position.y < 0 || position.y > 8;
        }
    }
}
