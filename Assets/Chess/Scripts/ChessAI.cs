using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChessBoard chessBoard = null;
        [SerializeField] private ChessPiece[] chessPieces = new ChessPiece[16];

        public void TakeTurn()
        {
            foreach (ChessPiece piece in RandomChessPieces())
            {
                Vector2Int[] validMoves = chessBoard.GetValidMoves(piece, true);
                if (validMoves.Length == 0) continue;
                foreach (Vector2Int move in validMoves)
                {
                    if (chessBoard.IsPieceAtPosition(move))
                    {
                        chessBoard.MovePiece(piece, move);
                        return;
                    }
                }
            }
            foreach (ChessPiece piece in RandomChessPieces())
            {
                Vector2Int[] validMoves = chessBoard.GetValidMoves(piece, true);
                if (validMoves.Length == 0) continue;
                chessBoard.MovePiece(piece, validMoves[Random.Range(0, validMoves.Length)]);
                return;
            }
        }

        private ChessPiece[] RandomChessPieces()
        {
            List<ChessPiece> randomChessPieces = new List<ChessPiece>();
            foreach (ChessPiece piece in chessPieces)
            {
                if (piece != null) randomChessPieces.Add(piece);
            }
            for (int i = 0; i < chessPieces.Length; i++)
            {
                int randomIndexA = Random.Range(0, randomChessPieces.Count);
                int randomIndexB = Random.Range(0, randomChessPieces.Count);
                Swap(randomIndexA, randomIndexB, randomChessPieces);
            }
            return randomChessPieces.ToArray();
        }

        private void Swap<T>(int indexA, int indexB, List<T> list)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }
    }
}
