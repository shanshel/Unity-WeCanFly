using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class GameManager : MonoBehaviour
{
    public static GameManager _inst;
    [HideInInspector]
    public GameStatus gameStatus = GameStatus.MainMenu;

    public float gameTime, playingTime;

    private void Awake()
    {
        _inst = this;
    }
    private void Update()
    {
        if (gameStatus == GameStatus.GameStarted)
        {
            playingTime += Time.deltaTime;
        }

        gameTime += Time.deltaTime;
    }


    public void startGame()
    {
        if (gameStatus == GameStatus.GameStarted) return;
        gameStatus = GameStatus.GameStarted;
        UIManager._inst.onGameStarted();
        MainPlayer._inst.onGameStarted();
        MainCamera._inst.onGameStarted();
    }
}
