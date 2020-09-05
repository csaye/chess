using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject moveHighlight = null;

        private ChessPiece[,] chessBoard = new ChessPiece[8, 8];

        private ChessPiece clickedPiece;

        private bool pieceClicked;

        private Transform moveHighlightTransform;

        private void Start()
        {
            moveHighlightTransform = transform.GetChild(1).transform;
        }

        public void InitializeChessPiece(ChessPiece piece)
        {
            SetPiece(piece);
        }

        public void ClickChessPiece(ChessPiece piece)
        {
            if (pieceClicked) return;
            pieceClicked = true;
            clickedPiece = piece;
            HighlightMovableSquares(piece);
        }

        private ChessPiece GetPiece(Vector2Int position)
        {
            return chessBoard[position.x, position.y];
        }

        private void RemovePiece(ChessPiece piece)
        {
            chessBoard[piece.position.x, piece.position.y] = null;
        }

        private void SetPiece(ChessPiece piece)
        {
            chessBoard[piece.position.x, piece.position.y] = piece;
        }

        private void HighlightMovableSquares(ChessPiece piece)
        {
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsValidMove(piece, movePosition)) HighlightSquare(movePosition);
            }
        }

        private bool IsValidMove(ChessPiece piece, Vector2Int movePosition)
        {
            if (!IsEmpty(movePosition)) return false;
            if (piece.type == ChessPieceType.Knight) return true;
            Debug.Log("TODO IsValidMove");
            return true;
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
            RemovePiece(clickedPiece);
            clickedPiece.position = position;
            SetPiece(clickedPiece);
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
            return GetPiece(position) == null;
        }

        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7;
        }
    }
}
