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
            if (IsOutOfBounds(position)) return null;
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
            bool highlighted = false;
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsValidMove(piece, movePosition))
                {
                    highlighted = true;
                    HighlightSquare(movePosition);
                }
            }
            if (!highlighted) pieceClicked = false;
        }

        private bool IsValidMove(ChessPiece piece, Vector2Int movePosition)
        {
            if (!IsEmpty(movePosition) && !IsOpposingPieceAtPosition(piece, movePosition)) return false;
            if (piece.type == ChessPieceType.Knight) return true;
            if (piece.position.x == movePosition.x || piece.position.y == movePosition.y)
            {
                return IsValidStraightMove(piece.position, movePosition);
            }
            else
            {
                return IsValidDiagonalMove(piece.position, movePosition);
            }
        }

        private bool IsOpposingPieceAtPosition(ChessPiece piece, Vector2Int position)
        {
            if (GetPiece(position) == null) return false;
            return GetPiece(position).team != piece.team;
        }

        private bool IsValidStraightMove(Vector2Int init, Vector2Int goal)
        {
            Vector2Int direction;
            if (init.x == goal.x)
            {
                direction = init.y < goal.y ? Vector2Int.up : Vector2Int.down;
            }
            else
            {
                direction = init.x < goal.x ? Vector2Int.right : Vector2Int.left;
            }
            Vector2Int pointer = init + direction;
            while (pointer != goal)
            {
                if (!IsEmpty(pointer)) return false;
                pointer += direction;
            }
            return true;
        }

        private bool IsValidDiagonalMove(Vector2Int init, Vector2Int goal)
        {
            Vector2Int direction;
            if (init.x < goal.x)
            {
                direction = init.y < goal.y ? new Vector2Int(1, 1) : new Vector2Int(1, -1);
            }
            else
            {
                direction = init.y < goal.y ? new Vector2Int(-1, 1) : new Vector2Int(-1, -1);
            }
            Vector2Int pointer = init + direction;
            while (pointer != goal)
            {
                if (!IsEmpty(pointer)) return false;
                pointer += direction;
            }
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
            if (GetPiece(position) != null) Destroy(GetPiece(position).gameObject);
            clickedPiece.position = position;
            clickedPiece.hasMoved = true;
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
