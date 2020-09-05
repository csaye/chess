﻿using UnityEngine;

namespace Chess
{
    public class ChessPiece : MonoBehaviour
    {
        [Header("Attributes")]
        public ChessPieceScriptable scriptable;

        private ChessBoard _chessBoard;

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

        public Vector2Int[] GetMoves()
        {
            return scriptable.moveScriptable.moves;
        }

        private ChessBoard chessBoard
        {
            get
            {
                if (_chessBoard == null) _chessBoard = FindObjectOfType<ChessBoard>();
                return _chessBoard;
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
