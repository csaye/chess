using UnityEngine;

namespace Chess
{
    public class ChessPiece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private ChessPieceScriptable scriptable = null;

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }

        private void OnClick()
        {
            Debug.Log(scriptable.type + " was clicked");
        }
    }
}
