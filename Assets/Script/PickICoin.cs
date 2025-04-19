using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ✅ เพิ่มบรรทัดนี้

public class PickCoin : MonoBehaviour
{
    public int coin = 0;
    public TMP_Text coinText; // ✅ เปลี่ยนจาก Text เป็น TMP_Text

    private void OnTriggerEnter(Collider target) {
        if (target.gameObject.tag.Equals("Coin")) {
            Debug.Log("Pick");
            Destroy(target.gameObject);
            coin += 10;
            coinText.text = "Coin : " + coin.ToString();
        }
    }

    public void DefeatBoss() {
        coin += 100;
        coinText.text = "Coin : " + coin.ToString();
        Debug.Log("suss");
    }

    public void DefeatEnemy() {
        coin += 20;
        coinText.text = "Coin : " + coin.ToString();
    }
}
