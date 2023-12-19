using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleObject : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
   
    // Update is called once per frame
    void Update()
    {
        if (particleSystem.isStopped) Destroy(this.gameObject);
    }
}
