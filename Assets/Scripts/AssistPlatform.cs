using UnityEngine;

public class AssistPlatform : Surface
{
    private int _stepsToDisband;
    
    public AssistPlatform(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        Confirm();
        _stepsToDisband = Tower.GetAssistPlatformSteps();
    }

    protected override void OnExit()
    {
        StepOnPlatform();
    }

    public void StepOnPlatform()
    {
        _stepsToDisband -= 1;
        if (_stepsToDisband <= 0) Disband();
    }
}