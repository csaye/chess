using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess
{
    public class MenuButton : MonoBehaviour
    {
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
