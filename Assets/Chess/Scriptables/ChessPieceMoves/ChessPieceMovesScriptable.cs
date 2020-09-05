using UnityEngine;

namespace Chess
{
    [CreateAssetMenuAttribute(fileName = "ChessPieceMoves", menuName = "Scriptables/ChessPieceMoves")]
    public class ChessPieceMovesScriptable : ScriptableObject
    {
        [Header("Attributes")]
        public Vector2Int[] moves;
    }
}
