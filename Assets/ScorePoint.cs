using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class ScorePoint : MonoBehaviour
{
    public TextMeshPro text;

 
    public void setup(int scoreNumber, string scoreText = "")
    {
        text.text = scoreText;
        GameManager._inst.score += scoreNumber;

        transform.DOLocalMoveY(10f, .5f).Play().OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
