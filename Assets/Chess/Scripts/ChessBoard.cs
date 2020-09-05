using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        private Piece[,] chessBoard = new Piece[8, 8];

        private Piece GetChessPiece(int x, int y)
        {
            return chessBoard[x, y];
        }

        private void SetChessPiece(int x, int y, Piece chessPiece)
        {
            chessBoard[x, y] = chessPiece;
        }
    }
}
