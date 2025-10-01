/*****************************************************************************
// File Name : Obstacle.cs
// Author : Brandon Koederitz
// Creation Date : 10/1/2025
// Last Modified : 10/1/2025
//
// Brief Description : Holds data 
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle")]
    public class Obstacle : ScriptableObject
    {
        [SerializeField] private float obstacleSize;
        [SerializeField] private Sprite obstacleSprite;
        [SerializeField] private int baseScore;

        [Header("Hitbox")]
        [SerializeField]

    }
}
