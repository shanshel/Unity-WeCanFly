using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;
using DG.Tweening;
using TMPro;

public class Platform : MonoBehaviour
{

    public PlatformerType type = PlatformerType.Normal;
    public MeshRenderer _targetRenderer;
    public GameObject springPiece;
    public SpriteRenderer _centerPointHover;
    public TextMeshPro PlatNumberText;
    [HideInInspector]
    public BoxCollider platCollider;


    
 
    Tweener floatingTween, pushDownTween, centerHoverTween;
    Vector3 randomVector;
    public int platNumber;
    [HideInInspector]
    public bool isJumped = false;

    [HideInInspector]
    public float angleToPrevPlat;
    Vector3 prevPlatPos;
    private void Start()
    {
        if (type == PlatformerType.Normal)
        {
            centerHoverTween = _centerPointHover.DOFade(0f, .4f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
            InvokeRepeating("SlowUpdate", 2f, .1f);
        }

        
    }
    private void Awake()
    {
        platCollider = GetComponent<BoxCollider>();
    }

  
    public void applyFloatingAnimtion()
    {
        float x = Random.Range(-2f, 3f);
        float y = Random.Range(-3f, 4f);
        float z = Random.Range(-2f, 3f);

        randomVector = new Vector3(x, y, z);
        if (springPiece != null)
            pushDownTween = springPiece.transform.DOPunchPosition(Vector3.down * 0.5f, .4f, 2, 3f, false).SetDelay(.05f).SetEase(Ease.InOutExpo).SetAutoKill(false);

   
        floatingTween = transform.DOMove(transform.position + randomVector, 2f).SetAutoKill(false).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).Play();

    }
    public void setTargetMaterial(Material mat)
    {
        _targetRenderer.material = mat;
    }


    public Vector3 testLookAt = Vector3.zero;
    public Vector3 testUp = Vector3.up;
    void SlowUpdate()
    {
     
        if (isJumped) return;
        if (Vector3.Distance(transform.position, MainPlayer._inst.transform.position) < 50f)
        {
            whenCloseToThePlayer();
        }
        else
        {
            whenFarFromthePlayer();
        }
        
    }


    void whenCloseToThePlayer()
    {
        if (type == PlatformerType.Normal)
        {
            _centerPointHover.enabled = true;
            if (!centerHoverTween.IsPlaying())
            {
                centerHoverTween.Play();
            }
        }
      
            
    }

    void whenFarFromthePlayer()
    {
        if (type == PlatformerType.Normal)
        {
            _centerPointHover.enabled = false;
            if (centerHoverTween.IsPlaying())
            {
                centerHoverTween.Pause();
            }
        }
            
    }
    public void onHitPlayer()
    {
        if (springPiece != null)
            pushDownTween.Restart();

        if (!isJumped)
        {
            isJumped = true;
            if (type == PlatformerType.Normal)
            {
                _centerPointHover.enabled = false;
                centerHoverTween.Pause();
            }
        }
     
       
       


    }

    public void setNumber(int pnumber)
    {
        platNumber = pnumber;
        if (platNumber == 0) return;

        PlatNumberText.text = ( ( (SharedData._inst.currentLevel - 1) * LevelManager._inst.platformPerLevel) +  platNumber).ToString() ;

    }

    public void setAngle(float angle)
    {

        angleToPrevPlat = angle;
    }

    public void informLastPlatformPosition(Vector3 pos)
    {
        
        PlatNumberText.transform.parent.transform.LookAt(pos);
    }

    void OnDestroy()
    {
        floatingTween.Kill();
        pushDownTween.Kill();
        centerHoverTween.Kill();
    }
}
