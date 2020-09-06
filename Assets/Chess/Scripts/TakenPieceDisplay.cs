using UnityEngine;

namespace Chess
{
    public class TakenPieceDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject[] displayChessPieces = new GameObject[12];

        private Transform whitePiecesTransform;
        private Transform blackPiecesTransform;

        private int whitePiecesCount = 0;
        private int blackPiecesCount = 0;

        private void Start()
        {
            whitePiecesTransform = transform.GetChild(0).transform;
            blackPiecesTransform = transform.GetChild(1).transform;
        }

        public void AddDisplayPiece(ChessPiece piece)
        {
            if (piece.team == ChessPieceTeam.White)
            {
                AddWhitePiece((int)piece.type);
            }
            else
            {
                AddBlackPiece((int)piece.type);
            }
        }

        private void AddWhitePiece(int type)
        {
            GameObject piece = displayChessPieces[type];
            Vector3 position = (Vector3Int)GetPosition(whitePiecesCount, true);
            GameObject obj = Instantiate(piece, Vector3.zero, Quaternion.identity, whitePiecesTransform);
            obj.transform.localPosition = position;
            whitePiecesCount++;
        }

        private void AddBlackPiece(int type)
        {
            GameObject piece = displayChessPieces[6 + type];
            Vector3 position = (Vector3Int)GetPosition(blackPiecesCount, false);
            GameObject obj = Instantiate(piece, Vector3.zero, Quaternion.identity, blackPiecesTransform);
            obj.transform.localPosition = position;
            blackPiecesCount++;
        }

        private Vector2Int GetPosition(int count, bool white)
        {
            int x = 0;
            if (count > 7) x = white ? x - 1 : x + 1;
            int y = 0;
            y += count % 8;
            return new Vector2Int(x, y);
        }
    }
}
