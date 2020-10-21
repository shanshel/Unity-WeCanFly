using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager _inst;

    [SerializeField]
    GameObject menuPanel;
    [SerializeField]
    TextMeshProUGUI scoreTextMesh;
    Tweener scoreTween;
    private void Awake()
    {
        _inst = this;
    }
    private void Start()
    {
        
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

    public void showScoreEffect(string textToShow, Color color)
    {
        scoreTextMesh.text = textToShow;
        scoreTextMesh.color = color;
        scoreTextMesh.transform.DOScale(1f, .3f).SetEase(Ease.OutElastic).Play().OnComplete(() => {
            scoreTextMesh.transform.DOScale(0f, .4f).SetDelay(.7f).SetEase(Ease.OutBounce).Play();
        });
    }
}
