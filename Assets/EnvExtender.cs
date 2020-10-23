using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvExtender : MonoBehaviour
{
    Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.layer == 8)
        {
            //Debug.Log(other.gameObject.name);
            //_rigidBody.detectCollisions = false;
            //Invoke("resetRigid", 1f);
            LevelManager._inst.extendEnv();
        }
    }

    private void resetRigid()
    {
        _rigidBody.ResetCenterOfMass();
        _rigidBody.ResetInertiaTensor();
        _rigidBody.detectCollisions = true;
    }

}
