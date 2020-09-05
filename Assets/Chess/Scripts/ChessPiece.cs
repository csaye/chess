using UnityEngine;

namespace Chess
{
    enum Piece
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public abstract class ChessPiece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private ChessPieceScriptable scriptable = null;
    }
}
