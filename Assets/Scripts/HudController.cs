using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    public GameObject hudPanel;

    public GameObject gameOverPanel;

    public TextMeshProUGUI playerHpValueText;

    public TextMeshProUGUI killCountText;

    private int playerHP;

    private int _killCount;

    private void Start()
    {
        hudPanel.SetActive(true);

        hudPanel.SetActive(false);

        gameOverPanel.SetActive(false);

        playerHP = 125;

        _killCount = 0;

        playerHpValueText.text = playerHP.ToString();

        killCountText.text = _killCount.ToString();
    }

    // Update is called once per frame
    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("keypressed");

            if (!hudPanel.activeInHierarchy)
            {
                hudPanel.SetActive(true);
            }
            else
            {
                hudPanel.SetActive(false);
            }
        }
    }

    public void UpdatePlayerHp(int hpValue)
    {
        playerHP -= hpValue;

        playerHpValueText.text = playerHP.ToString();

        if (playerHP <= 0)
        {
            gameOverPanel.SetActive(true);

            Debug.Log("GameOver");
        }
    }

    //updating the zombie kill count, when bullet hits zombie it will die
    public void EnemyDamage()
    {
        _killCount += 1;

        killCountText.text = _killCount.ToString();
    }
}
