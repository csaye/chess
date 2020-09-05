using UnityEngine;

namespace Chess
{
    enum ChessPieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    enum ChessPieceTeam
    {
        White,
        Black
    }

    [CreateAssetMenuAttribute(fileName = "ChessPiece", menuName = "Scriptables/ChessPiece")]
    public class ChessPieceScriptable : ScriptableObject
    {
        [Header("Chess Piece Attributes")]
        [SerializeField] private ChessPieceType type;
        [SerializeField] private ChessPieceTeam team;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2Int[] possibleMoves;
    }
}
