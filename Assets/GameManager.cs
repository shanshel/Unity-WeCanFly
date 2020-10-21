using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class GameManager : MonoBehaviour
{
    public static GameManager _inst;
    [HideInInspector]
    public GameStatus gameStatus = GameStatus.MainMenu;

    public float gameTime, playingTime, timeScaleTime;

    [HideInInspector]
    public int score;
    [SerializeField]
    Color ColorGoodJump, ColorLongJump, ColorPerfectJump, ColorPerfectLongJump;
    private void Awake()
    {
        _inst = this;
    }
    private void Update()
    {
        if (gameStatus == GameStatus.GameStarted)
        {
            playingTime += Time.deltaTime;

            if (timeScaleTime <= 0f)
            {
                Time.timeScale = 1f;
            }
        }

        timeScaleTime -= Time.deltaTime;
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

    public void applyTimeScaleEffect(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        timeScaleTime = duration;

    }

    public void addScore(int _score, string textToShow, bool isSilent = false)
    {
        if (!isSilent)
        {
            Color color = ColorGoodJump;

            if (_score == 1)
                color = ColorGoodJump;
            else if (_score == 2)
                color = ColorLongJump;
            else if (_score == 3)
                color = ColorPerfectJump;
            else if (_score == 4)
                color = ColorPerfectLongJump;
            UIManager._inst.showScoreEffect(textToShow, color);
        }
      
        score += _score;
    }

   
}
