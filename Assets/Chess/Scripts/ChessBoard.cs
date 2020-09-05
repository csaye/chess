using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject moveHighlight = null;

        private ChessPieceType[,] chessBoard = new ChessPieceType[8, 8];

        private ChessPiece clickedPiece;

        private bool pieceClicked;

        private Transform moveHighlightTransform;

        private void Start()
        {
            moveHighlightTransform = transform.GetChild(1).transform;
        }

        private ChessPieceType GetChessPiece(Vector2Int position)
        {
            return chessBoard[position.x, position.y];
        }

        public void ClickChessPiece(ChessPiece piece)
        {
            if (pieceClicked) return;
            pieceClicked = true;
            clickedPiece = piece;
            HighlightMovableSquares(piece);
        }

        private void HighlightMovableSquares(ChessPiece piece)
        {
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsEmpty(movePosition)) HighlightSquare(movePosition);
            }
        }

        private void HighlightSquare(Vector2Int position)
        {
            Instantiate(moveHighlight, PieceToHighlightPosition(position), Quaternion.identity, moveHighlightTransform);
        }

        public void ChooseMovePosition(Vector2Int position)
        {
            if (!pieceClicked) return;
            pieceClicked = false;
            ClearAllHighlight();
            clickedPiece.position = position;
        }

        private void ClearAllHighlight()
        {
            foreach (Transform transform in moveHighlightTransform) Destroy(transform.gameObject);
        }

        private Vector3 PieceToHighlightPosition(Vector2Int position)
        {
            return new Vector3(position.x - 3.5f, position.y - 3.5f, 0);
        }

        private bool IsEmpty(Vector2Int position)
        {
            if (IsOutOfBounds(position)) return false;
            return GetChessPiece(position) == ChessPieceType.Empty;
        }

        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7;
        }
    }
}
