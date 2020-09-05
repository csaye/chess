using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject moveHighlight = null;
        [SerializeField] private ChessAI chessAI = null;
        [SerializeField] private GameOverPopup gameOverPopup = null;

        public bool isGameOver {get; private set;} = false;

        private ChessPiece[,] chessBoard = new ChessPiece[8, 8];

        private ChessPiece clickedPiece;

        private bool pieceClicked;

        private Transform moveHighlightTransform;

        private void Start()
        {
            moveHighlightTransform = transform.GetChild(1).transform;
        }

        public void InitializePiece(ChessPiece piece)
        {
            SetPiece(piece.position, piece);
        }

        public void ClickPiece(ChessPiece piece)
        {
            if (pieceClicked) return;
            if (isGameOver) return;
            if (GetValidMoves(piece).Length == 0) return;
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

        private void SetPiece(Vector2Int position, ChessPiece piece)
        {
            chessBoard[position.x, position.y] = piece;
        }

        private void HighlightMovableSquares(ChessPiece piece)
        {
            foreach (Vector2Int move in GetValidMoves(piece)) HighlightSquare(move);
        }

        public Vector2Int[] GetValidMoves(ChessPiece piece)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsValidMove(piece, movePosition))
                {
                    validMoves.Add(movePosition);
                }
            }
            return validMoves.ToArray();
        }

        private bool IsValidMove(ChessPiece piece, Vector2Int movePosition)
        {
            if (!IsEmpty(movePosition) && !IsOpposingPieceAtPosition(piece, movePosition)) return false;
            if (MoveResultsInCheck(piece, movePosition)) return false;
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

        private bool MoveResultsInCheck(ChessPiece piece, Vector2Int position)
        {
            ChessPiece previousPiece = GetPiece(position);
            SetPiece(position, piece);
            bool resultsInCheck = IsInCheck(piece.team);
            SetPiece(position, previousPiece);
            return resultsInCheck;
        }

        public bool IsOpposingPieceAtPosition(ChessPiece piece, Vector2Int position)
        {
            if (GetPiece(position) == null) return false;
            return GetPiece(position).team != piece.team;
        }

        public bool IsPieceAtPosition(Vector2Int position)
        {
            return GetPiece(position) != null;
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
            MovePiece(clickedPiece, position);
            CheckCheckmateOfTeam(ChessPieceTeam.Black);
            if (isGameOver) return;
            chessAI.TakeTurn();
            CheckCheckmateOfTeam(ChessPieceTeam.White);
        }

        public void CheckCheckmateOfTeam(ChessPieceTeam team)
        {
            if (!IsInCheck(team)) return;
            // check for checkmate not check
            isGameOver = true;
            gameOverPopup.ActivateGameOver(team);
        }

        private bool IsInCheck(ChessPieceTeam team)
        {
            Vector2Int kingPosition = GetKingPosition(team);
            ChessPiece[] opposingPieces;
            if (team == ChessPieceTeam.White)
            {
                opposingPieces = GetAllPiecesOfTeam(ChessPieceTeam.Black);
            }
            else
            {
                opposingPieces = GetAllPiecesOfTeam(ChessPieceTeam.White);
            }
            foreach (ChessPiece opposingPiece in opposingPieces)
            {
                if (GetValidMoves(opposingPiece).Contains(kingPosition)) return true;
            }
            return false;
        }

        private Vector2Int GetKingPosition(ChessPieceTeam team)
        {
            foreach (ChessPiece piece in chessBoard)
            {
                if (piece == null) continue;
                if (piece.type == ChessPieceType.King && piece.team == team) return piece.position;
            }
            return new Vector2Int(-1, -1);
        }

        private ChessPiece[] GetAllPiecesOfTeam(ChessPieceTeam team)
        {
            List<ChessPiece> pieces = new List<ChessPiece>();
            foreach (ChessPiece piece in chessBoard)
            {
                if (piece != null && piece.team == team) pieces.Add(piece);
            }
            return pieces.ToArray();
        }

        public void MovePiece(ChessPiece piece, Vector2Int position)
        {
            RemovePiece(piece);
            if (GetPiece(position) != null) Destroy(GetPiece(position).gameObject);
            piece.position = position;
            piece.hasMoved = true;
            SetPiece(position, piece);
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
