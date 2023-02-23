using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Liutenant Property", fileName = "New Liutenant Property")]
public class LiutenantPropertySO : ScriptableObject
{
    [SerializeField][Range(0, 1)] float Chance = 0.25f;
    [field: SerializeField] public LPType LPType { get; set;}
    [field: SerializeField] public float Value { get; set;}
    [field: SerializeField] public Color Color { get; set; }

    // public Color GetColor()
    // {
    //     return Color;
    // }
}



public enum LPType
{
    ExtraHealth = 1,
    HasTripleShoot = 2,
    HasBigShoot = 3,
    HasShield = 4,
    ExtraSpeed = 5
}
