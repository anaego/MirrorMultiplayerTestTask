using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/MovementSettingsScriptableObject", order = 1)]
public class MovementSettingsScriptableObject : ScriptableObject
{
    [SerializeField] private float verticalTurnSpeed = 100;
    [SerializeField] private float horizontalTurnSpeed = 100;
    [SerializeField] private float moveSpeedMultiplier = 8;
    [SerializeField] private float surgeSpeedMultiplier = 25;
    [SerializeField] private float surgeDistance = 10;

    public float VerticalTurnSpeed => verticalTurnSpeed;
    public float HorizontalTurnSpeed => horizontalTurnSpeed;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float SurgeSpeedMultiplier => surgeSpeedMultiplier;
    public float SurgeDistance => surgeDistance;
}
