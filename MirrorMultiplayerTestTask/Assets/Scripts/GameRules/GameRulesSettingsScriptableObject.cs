using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesSettings", menuName = "ScriptableObjects/GameRulesScriptableObject", order = 1)]
public class GameRulesSettingsScriptableObject : ScriptableObject
{
    [SerializeField] private float invincibilityDurationSeconds = 3;

    public float InvincibilityDurationSeconds => invincibilityDurationSeconds;
}
