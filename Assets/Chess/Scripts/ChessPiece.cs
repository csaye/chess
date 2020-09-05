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
            chessBoard.InitializeChessPiece(this);
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
                return new Vector2Int[] { Vector2Int.up };
            }
            else
            {
                return new Vector2Int[] { Vector2Int.down };
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0)) OnClick();
        }

        private void OnClick()
        {
            chessBoard.ClickChessPiece(this);
        }
    }
}
