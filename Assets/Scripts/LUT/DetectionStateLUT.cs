using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectionStateLUT
{
    private readonly Dictionary<GameManager.DetectionState, Action> _stateActions;

    public DetectionStateLUT(Action normalActivity, Action fullActivity)
    {
        if (normalActivity == null)
            normalActivity = () => Debug.LogWarning("NormalActivity no está asignada");
        if (fullActivity == null)
            fullActivity = () => Debug.LogWarning("FullActivity no está asignada");
        
        _stateActions = new Dictionary<GameManager.DetectionState, Action>
        {
            { GameManager.DetectionState.Hidden, normalActivity },
            { GameManager.DetectionState.Alerted, fullActivity },
            { GameManager.DetectionState.Detected, fullActivity }
        };
    }

    public void ExecuteAction(GameManager.DetectionState state)
    {
        if (_stateActions.TryGetValue(state, out var action))
        {
            action.Invoke();
        }
        else
        {
            Debug.Log("No se definió acción para el estado: " + state);
        }
    }
}
