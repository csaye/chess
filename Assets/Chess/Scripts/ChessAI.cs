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
                    if (chessBoard.IsPieceAtPosition(move) && chessBoard.GetPiece(move).type != ChessPieceType.King)
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
                foreach (Vector2Int move in RandomMoves(validMoves))
                {
                    if (chessBoard.IsPieceAtPosition(move) && chessBoard.GetPiece(move).type == ChessPieceType.King) continue;
                    chessBoard.MovePiece(piece, move);
                    return;
                }
            }
        }

        private ChessPiece[] RandomChessPieces()
        {
            List<ChessPiece> randomChessPieces = new List<ChessPiece>();
            foreach (ChessPiece piece in chessPieces)
            {
                if (piece != null) randomChessPieces.Add(piece);
            }
            ChessPiece[] randomPieces = randomChessPieces.ToArray();
            for (int i = 0; i < randomPieces.Length; i++)
            {
                int randomIndexA = Random.Range(0, randomPieces.Length);
                int randomIndexB = Random.Range(0, randomPieces.Length);
                randomPieces = Swap(randomIndexA, randomIndexB, randomPieces);
            }
            return randomPieces;
        }

        private Vector2Int[] RandomMoves(Vector2Int[] moves)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                int randomIndexA = Random.Range(0, moves.Length);
                int randomIndexB = Random.Range(0, moves.Length);
                moves = Swap(randomIndexA, randomIndexB, moves);
            }
            return moves;
        }

        private T[] Swap<T>(int indexA, int indexB, T[] array)
        {
            T[] newArray = array;
            T temp = newArray[indexA];
            newArray[indexA] = newArray[indexB];
            newArray[indexB] = temp;
            return newArray;
        }
    }
}
