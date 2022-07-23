using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLooking : MonoBehaviour
{

    private Camera theCam;

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(theCam.transform);
    }
}
