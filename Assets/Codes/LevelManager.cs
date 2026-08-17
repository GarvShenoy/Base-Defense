using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    
    //This code just defines the end goal for mutants.
    public Transform endPoint;

    private void Awake()
    {
        main = this;
    }

}
