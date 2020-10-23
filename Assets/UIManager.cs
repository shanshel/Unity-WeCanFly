using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static Assets.Scripts.EnumData;

public class UIManager : MonoBehaviour
{
    public static UIManager _inst;

    [SerializeField]
    GameObject menuPanel, deathPanel, finishedPanel;
    [SerializeField]
    TextMeshProUGUI scoreTextMesh, FinishedTextMesh;
    Tweener scoreTween;

    
    bool isPlayAgain = false;
    private void Awake()
    {
        _inst = this;
    }
    private void Start()
    {
        
    }
    public void changeCharacter()
    {
        SoundManager._inst.playSoundOnce(SoundEnum.UIClickSFX);
        MainPlayer._inst.nextCharacter();
    }

    public void startGame()
    {

        SoundManager._inst.playSoundOnce(SoundEnum.UIHeavyClickSFX);
        GameManager._inst.startGame();
      
    }

    public void onGameStarted()
    {
        menuPanel.SetActive(false);
    }

    [HideInInspector]
    public void showScoreEffect(string textToShow, Color color)
    {
        scoreTextMesh.text = textToShow;
        scoreTextMesh.color = color;
        scoreTextMesh.transform.DOScale(1f, .3f).SetEase(Ease.OutElastic).Play().OnComplete(() => {
            scoreTextMesh.transform.DOScale(0f, .4f).SetDelay(.7f).SetEase(Ease.OutBounce).Play();
        });
    }
    
    public void playAgain()
    {
        if (isPlayAgain) return;
        isPlayAgain = true;
        SoundManager._inst.playSoundOnce(SoundEnum.UIClickSFX);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void showDeathScreen()
    {
        deathPanel.SetActive(true);
    }

    public void showFinishScreen()
    {
        FinishedTextMesh.text =  "Go Next Level (" +  SharedData._inst.currentLevel.ToString() + ")";
        finishedPanel.SetActive(true);
    }

}
