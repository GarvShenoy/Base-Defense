using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float slowMultiplier = 0.3f;
    [SerializeField] private float trapDuration = 2f;

    public float GetSlowMultiplier() => slowMultiplier;
    public float GetDuration() => trapDuration;
}