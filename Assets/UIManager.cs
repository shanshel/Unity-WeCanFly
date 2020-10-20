using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _inst;

    [SerializeField]
    GameObject menuPanel;

    private void Awake()
    {
        _inst = this;
    }
    public void changeCharacter()
    {
        MainPlayer._inst.nextCharacter();
    }

    public void startGame()
    {
        GameManager._inst.startGame();
    }

    public void onGameStarted()
    {
        menuPanel.SetActive(false);
    }
}
