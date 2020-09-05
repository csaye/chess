using UnityEngine;

namespace Chess
{
    public enum ChessPieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum ChessPieceTeam
    {
        White,
        Black
    }

    [CreateAssetMenuAttribute(fileName = "ChessPiece", menuName = "Scriptables/ChessPiece")]
    public class ChessPieceScriptable : ScriptableObject
    {
        [Header("Attributes")]
        public ChessPieceType type;
        public ChessPieceTeam team;
        public ChessPieceMovesScriptable moveScriptable;
    }
}
