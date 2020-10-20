using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class PlayerGroundDetector : MonoBehaviour
{

    public GameObject falseDetector, trueDetector, circle;


    public float yWhenPlayerInAir = 1f, yWhenPlayerBackJump = 0.07f, yNormal = 0f;


    private void FixedUpdate()
    {
        if (MainPlayer._inst.currentMove == currentPlayerMoveStatus.InAir)
        {

            transform.localPosition = new Vector3(transform.localPosition.x, yWhenPlayerInAir, transform.localPosition.z);
        }
        else if(MainPlayer._inst.currentMove == currentPlayerMoveStatus.BackJump)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yWhenPlayerBackJump, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yNormal, transform.localPosition.z);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            trueDetector.SetActive(true);
            circle.SetActive(true);
            falseDetector.SetActive(false);
            Vector3 pos = trueDetector.transform.position;
            circle.transform.position = new Vector3(pos.x, other.transform.position.y + 1f, pos.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            trueDetector.SetActive(false);
            falseDetector.SetActive(true);
            circle.SetActive(false);
        }
    }
   
}
