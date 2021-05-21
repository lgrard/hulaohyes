﻿using UnityEngine;
using System.Collections;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Enemy
{
    [CreateAssetMenu(fileName = "WalkerData_0", menuName = "HulaOhYes/Data/Enemy", order = 0)]
    public class EnemyDatas : ScriptableObject
    {
        [Header("HP values")]
        [SerializeField] private int _hp = 3;

        [Header("Enemy type")]
        [SerializeField] private EnemyType type = EnemyType.walker;

        [Header("Detection values")]
        [SerializeField] private float _detectionRadius = 3;

        [Header("Attack values")]
        [SerializeField] private float _startUpDuration = 1;
        [SerializeField] private float _activeDuration = 1;
        [SerializeField] private float _recoveryDuration = 1;

        [Header("If walker")]
        [SerializeField] private float _rushSpeed = 3;
        [SerializeField] private float _accelerationAmount = 1;
        [SerializeField] private float _brakingAmount = 1;


        //Getters
        public int hp => _hp;
        public float startUpDuration => _startUpDuration;
        public float activeDuration => _activeDuration;
        public float detectionRadius => _detectionRadius;
        public float recoveryDuration => _recoveryDuration;
        public float rushSpeed => _rushSpeed;
        public float acceleration => _accelerationAmount;
        public float braking => _brakingAmount;

    }

    public enum EnemyType
    {
        walker,
        turret,
    }
}