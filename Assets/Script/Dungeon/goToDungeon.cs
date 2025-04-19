using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class goToDungeon : MonoBehaviour
{
    private void OnCollisionEnter(Collision target) {
        if (target.gameObject.tag.Equals("Player")) {
            NextScene();
        }
    }
     void NextScene() {
        SceneManager.LoadScene("Dungeon");
     }
}
