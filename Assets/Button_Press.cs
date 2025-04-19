using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Press : MonoBehaviour
{
    public void ExitButton() {
        Application.Quit();

    }
    public void StartGame() {
        SceneManager.LoadScene("HomeScene"); // ✅ ใช้ SceneManager.LoadScene("HomeScene");
    }
}
