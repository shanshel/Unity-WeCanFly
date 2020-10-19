using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;
using DG.Tweening;
public class Platform : MonoBehaviour
{

    public PlatformerType type = PlatformerType.Normal;
    public MeshRenderer _targetRenderer;


    private void Start()
    {
        float x = Random.Range(-2f, 3f);
        float y = Random.Range(-3f, 4f);
        float z = Random.Range(-2f, 3f);

        Vector3 randomVector = new Vector3(x, y, z);

        //transform.DOShakePosition(3f, 3f, 1, 5f, false, false).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        transform.DOMove(transform.position + randomVector, 2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
   
    }

    public void setTargetMaterial(Material mat)
    {
        _targetRenderer.material = mat;
    }
}
