using UnityEngine;

public class AssistPlatform : Surface
{
    private float _clicksToDisband;
    private float _onStandClickStrength = 1;
    private float _onDoingSthClickStrength = 0.1f;
    
    public AssistPlatform(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        Confirm();
        _clicksToDisband = _info.health;
    }

    protected override void DoSth()
    {
        Click(_onDoingSthClickStrength);
    }

    protected override void OnExit()
    {
        StepOnPlatform();
    }

    public void StepOnPlatform()
    {
        Click(_onStandClickStrength);
    }

    private void Click(float strength)
    {
        _clicksToDisband -= strength;
        if (_clicksToDisband <= 0) Disband();
        _picture.color = new Color(_picture.material.color.r, _picture.material.color.g,
            _picture.material.color.b, _clicksToDisband / _info.health);
    }
}