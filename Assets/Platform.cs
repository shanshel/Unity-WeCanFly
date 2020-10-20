using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;
using DG.Tweening;
public class Platform : MonoBehaviour
{

    public PlatformerType type = PlatformerType.Normal;
    public MeshRenderer _targetRenderer;
    public GameObject springPiece;
    [HideInInspector]
    public BoxCollider platCollider;

    
 
    Tweener floatingTween, pushDownTween;
    Vector3 randomVector;

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

   
    public void onHitPlayer()
    {
        if (springPiece != null)
            pushDownTween.Restart();
    }


}
