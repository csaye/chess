using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessPiece : MonoBehaviour
    {
        [Header("Attributes")]
        public ChessPieceScriptable scriptable;

        private ChessBoard chessBoard;

        public ChessPieceType type { get { return scriptable.type; } }
        public ChessPieceTeam team { get { return scriptable.team; } }

        public bool hasMoved {get; set;} = false;

        public Vector2Int position
        {
            get
            {
                int x = Mathf.RoundToInt(transform.localPosition.x);
                int y = Mathf.RoundToInt(transform.localPosition.y);
                return new Vector2Int(x, y);
            }
            set
            {
                transform.localPosition = (Vector3Int)value;
            }
        }

        private void Start()
        {
            chessBoard = FindObjectOfType<ChessBoard>();
            chessBoard.InitializePiece(this);
        }

        public Vector2Int[] GetMoves()
        {
            if (type == ChessPieceType.Pawn) return PawnMoves();
            return scriptable.moveScriptable.moves;
        }

        private Vector2Int[] PawnMoves()
        {
            if (team == ChessPieceTeam.White)
            {
                List<Vector2Int> movesList = new List<Vector2Int>();
                if (chessBoard.IsOpposingPieceAtPosition(this, position + new Vector2Int(1, 1))) movesList.Add(new Vector2Int(1, 1));
                if (chessBoard.IsOpposingPieceAtPosition(this, position + new Vector2Int(-1, 1))) movesList.Add(new Vector2Int(-1, 1));
                if (!chessBoard.IsPieceAtPosition(position + new Vector2Int(0, 1))) movesList.Add(new Vector2Int(0, 1));
                if (!hasMoved && !chessBoard.IsPieceAtPosition(position + new Vector2Int(0, 2))) movesList.Add(new Vector2Int(0, 2));
                return movesList.ToArray();
            }
            else
            {
                List<Vector2Int> movesList = new List<Vector2Int>();
                if (chessBoard.IsOpposingPieceAtPosition(this, position + new Vector2Int(1, -1))) movesList.Add(new Vector2Int(1, -1));
                if (chessBoard.IsOpposingPieceAtPosition(this, position + new Vector2Int(-1, -1))) movesList.Add(new Vector2Int(-1, -1));
                if (!chessBoard.IsPieceAtPosition(position + new Vector2Int(0, -1))) movesList.Add(new Vector2Int(0, -1));
                if (!hasMoved && !chessBoard.IsPieceAtPosition(position + new Vector2Int(0, -2))) movesList.Add(new Vector2Int(0, -2));
                return movesList.ToArray();
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0)) OnClick();
        }

        private void OnClick()
        {
            if (team != ChessPieceTeam.White) return;
            chessBoard.ClickPiece(this);
        }
    }
}
