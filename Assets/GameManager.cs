using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class GameManager : MonoBehaviour
{
    public static GameManager _inst;
    [HideInInspector]
    public GameStatus gameStatus = GameStatus.MainMenu;
    public bool isAboutToFinish = false;
    public float gameTime, playingTime, timeScaleTime;

    [HideInInspector]
    public int score;
    [SerializeField]
    Color ColorGoodJump, ColorLongJump, ColorPerfectJump, ColorPerfectLongJump;
    private void Awake()
    {
        _inst = this;
        
        if (Screen.currentResolution.refreshRate < 40)
        {
            Application.targetFrameRate = 30;
        }
        else if (Screen.currentResolution.refreshRate < 75)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 120;
        }
    }

    private void Start()
    {
        SoundManager._inst.playMusic(SoundEnum.MainMenuMusic);
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
        SoundManager._inst.playMusic(SoundEnum.GameplayMusic);
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

    public void killPlayer(float showScreenAfter = 1f)
    {
        gameStatus = GameStatus.Die;
        Invoke("sendShowDeathScreenToUI", showScreenAfter);
       
    }
    void sendShowDeathScreenToUI()
    {
        UIManager._inst.showDeathScreen();
    }
   

    public void finishLevel(float finishAfter = 2.5f)
    {
        isAboutToFinish = true;
        SharedData._inst.currentLevel = SharedData._inst.currentLevel + 1;
        Invoke("sendShowWinScreenToUI", finishAfter);
    }
    void sendShowWinScreenToUI()
    {
        gameStatus = GameStatus.Finished;
        
        MainPlayer._inst._rigid.isKinematic = true;
        MainCamera._inst.onGameFinished();
        SoundManager._inst.playSoundOnce(SoundEnum.WinSFX2);
        UIManager._inst.showFinishScreen();
  
    }

}
