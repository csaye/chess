using TMPro;
using System.Collections;
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
        [SerializeField] private TextMeshProUGUI checkIndicator = null;

        public bool isGameOver {get; private set;} = false;

        public bool isPieceMoving {get; private set;} = false;

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
            if (isGameOver) return;
            if (isPieceMoving) return;
            ClearAllHighlight();
            if (pieceClicked && clickedPiece == piece)
            {
                pieceClicked = false;
                return;
            }
            pieceClicked = true;
            clickedPiece = piece;
            HighlightMovableSquares(piece);
        }

        public ChessPiece GetPiece(Vector2Int position)
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
            Vector2Int[] validMoves = GetValidMoves(piece, true);
            if (validMoves.Length == 0) pieceClicked = false;
            foreach (Vector2Int move in validMoves) HighlightSquare(move);
        }

        public Vector2Int[] GetValidMoves(ChessPiece piece, bool checkPrevention)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            foreach (Vector2Int move in piece.GetMoves())
            {
                Vector2Int movePosition = piece.position + move;
                if (IsValidMove(piece, movePosition, checkPrevention))
                {
                    validMoves.Add(movePosition);
                }
            }
            return validMoves.ToArray();
        }

        private bool IsValidMove(ChessPiece piece, Vector2Int movePosition, bool checkPrevention)
        {
            if (!IsEmpty(movePosition) && !IsOpposingPieceAtPosition(piece, movePosition)) return false;
            if (checkPrevention && MoveResultsInCheck(piece, movePosition)) return false;
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
            Vector2Int previousPosition = piece.position;
            SetPiece(position, piece);
            SetPiece(previousPosition, null);
            bool resultsInCheck = IsInCheck(piece.team);
            SetPiece(previousPosition, piece);
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
            if (GetPiece(position) != null && GetPiece(position).type == ChessPieceType.King) return;
            Instantiate(moveHighlight, PieceToHighlightPosition(position), Quaternion.identity, moveHighlightTransform);
        }

        public void ChooseMovePosition(Vector2Int position)
        {
            if (!pieceClicked) return;
            pieceClicked = false;
            ClearAllHighlight();
            MovePiece(clickedPiece, position);
            StartCoroutine(MakeAIMove());
        }

        private IEnumerator MakeAIMove()
        {
            while (isPieceMoving) yield return null;
            yield return null;
            CheckCheckmate();
            if (!isGameOver)
            {
                chessAI.TakeTurn();
                while (isPieceMoving) yield return null;
                yield return null;
                CheckCheckmate();
            }
        }

        private void CheckCheckmate()
        {
            if (IsInCheckmate(ChessPieceTeam.White))
            {
                isGameOver = true;
                gameOverPopup.ActivateGameOver(ChessPieceTeam.White);
            }
            if (IsInCheckmate(ChessPieceTeam.Black))
            {
                isGameOver = true;
                gameOverPopup.ActivateGameOver(ChessPieceTeam.Black);
            }
        }

        private bool IsInCheckmate(ChessPieceTeam team)
        {
            if (!IsInCheck(team))
            {
                if (team != ChessPieceTeam.Black || checkIndicator.text != "White is in check") checkIndicator.text = "";
            }
            else
            {
                checkIndicator.text = (team == ChessPieceTeam.White) ? "White is in check" : "Black is in check";
            }
            foreach (ChessPiece piece in GetAllPiecesOfTeam(team))
            {
                if (GetValidMoves(piece, true).Length > 0) return false;
            }
            return true;
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
                if (GetValidMoves(opposingPiece, false).Contains(kingPosition)) return true;
            }
            return false;
        }

        private Vector2Int GetKingPosition(ChessPieceTeam team)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    ChessPiece piece = GetPiece(position);
                    if (piece == null) continue;
                    if (piece.type == ChessPieceType.King && piece.team == team) return position;
                }
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
            isPieceMoving = true;
            piece.SetSortingLayer(1);
            RemovePiece(piece);
            piece.hasMoved = true;
            StartCoroutine(LerpPiece(piece, position));
        }

        private IEnumerator LerpPiece(ChessPiece piece, Vector2Int goal)
        {
            Vector2 init = piece.position;
            for (float i = 0; i < 1; i += 0.01f)
            {
                piece.transform.localPosition = ((1 - i) * init) + (i * (Vector2)goal);
                yield return null;
            }
            piece.position = goal;
            if (GetPiece(goal) != null) Destroy(GetPiece(goal).gameObject);
            SetPiece(goal, piece);
            piece.SetSortingLayer(0);
            isPieceMoving = false;
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
