using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton() {
        Debug.Log("Quit");
        Application.Quit();

    }
    public void StartGame() {
        SceneManager.LoadScene("HomeScene"); // ✅ ใช้ SceneManager.LoadScene("HomeScene");
    }
}
