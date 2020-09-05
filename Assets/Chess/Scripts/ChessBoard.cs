using UnityEngine;

namespace Chess
{
    public class ChessBoard : MonoBehaviour
    {
        private ChessPieceType[,] chessBoard = new ChessPieceType[8, 8];

        private ChessPieceType GetChessPiece(int x, int y)
        {
            return chessBoard[x, y];
        }

        private void SetChessPiece(int x, int y, ChessPieceType chessPiece)
        {
            chessBoard[x, y] = chessPiece;
        }
    }
}
