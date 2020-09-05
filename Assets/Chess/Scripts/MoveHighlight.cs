using UnityEngine;

namespace Chess
{
    public class MoveHighlight : MonoBehaviour
    {
        private ChessBoard _chessBoard;

        private Vector2Int position
        {
            get
            {
                int x = Mathf.RoundToInt(transform.position.x + 3.5f);
                int y = Mathf.RoundToInt(transform.position.y + 3.5f);
                return new Vector2Int(x, y);
            }
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
            chessBoard.ChooseMovePosition(position);
        }
    }
}
