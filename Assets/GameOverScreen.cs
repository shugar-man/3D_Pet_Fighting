using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void Setup() {
        Debug.Log("gamee");
        gameObject.SetActive(true);
    }

    public void RestartButton() {
        SceneManager.LoadScene("Dungeon");
    }
    public void ExitButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
