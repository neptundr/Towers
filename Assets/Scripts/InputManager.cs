using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private KeyCode _moveRightTower1 = KeyCode.D;
    private KeyCode _moveLeftTower1 = KeyCode.A;
    private KeyCode _moveUpTower1 = KeyCode.W;
    private KeyCode _moveDownTower1 = KeyCode.S;
    private KeyCode _placeTower1 = KeyCode.LeftShift;
    private KeyCode _changeBlockTower1 = KeyCode.LeftAlt;
    private KeyCode _maximizeCameraTower1 = KeyCode.X;
    private KeyCode _minimizeCameraTower1 = KeyCode.C;
    private KeyCode _placeOrderTower1 = KeyCode.LeftControl;

    private KeyCode _moveRightTower2 = KeyCode.L;
    private KeyCode _moveLeftTower2 = KeyCode.J;
    private KeyCode _moveUpTower2 = KeyCode.I;
    private KeyCode _moveDownTower2 = KeyCode.K;
    private KeyCode _placeTower2 = KeyCode.RightShift;
    private KeyCode _changeBlockTower2 = KeyCode.Space;
    private KeyCode _maximizeCameraTower2 = KeyCode.M;
    private KeyCode _minimizeCameraTower2 = KeyCode.N;
    private KeyCode _placeOrderTower2 = KeyCode.RightControl;

    private void Update()
    {
        if (!GameManager.BuyPaused())
        {
            if (Input.GetKeyDown(_moveRightTower1)) Tower.GetFirstPlacer().Move(new Vector2Int(1, 0));
            else if (Input.GetKeyDown(_moveLeftTower1)) Tower.GetFirstPlacer().Move(new Vector2Int(-1, 0));
            else if (Input.GetKeyDown(_moveUpTower1)) Tower.GetFirstPlacer().Move(new Vector2Int(0, 1));
            else if (Input.GetKeyDown(_moveDownTower1)) Tower.GetFirstPlacer().Move(new Vector2Int(0, -1));

            if (Input.GetKeyDown(_changeBlockTower1)) Tower.GetFirstPlacer().ChangeBlock();
            if (Input.GetKeyDown(_placeTower1)) Tower.GetFirstPlacer().TryPlace();

            if (Input.GetKey(_maximizeCameraTower1)) Tower.GetFirstPlacer().MaximizeCamera();
            if (Input.GetKey(_minimizeCameraTower1)) Tower.GetFirstPlacer().MinimizeCamera();

            if (Input.GetKeyDown(_placeOrderTower1)) Tower.GetFirstPlacer().PlaceOrder();
            // if (Input.GetKey(KeyCode.LeftControl)) _tower1.GetPlacer().DeleteAssistPlatform();


            if (Input.GetKeyDown(_moveRightTower2)) Tower.GetSecondPlacer().Move(new Vector2Int(1, 0));
            else if (Input.GetKeyDown(_moveLeftTower2)) Tower.GetSecondPlacer().Move(new Vector2Int(-1, 0));
            else if (Input.GetKeyDown(_moveUpTower2)) Tower.GetSecondPlacer().Move(new Vector2Int(0, 1));
            else if (Input.GetKeyDown(_moveDownTower2)) Tower.GetSecondPlacer().Move(new Vector2Int(0, -1));

            if (Input.GetKeyDown(_changeBlockTower2)) Tower.GetSecondPlacer().ChangeBlock();
            if (Input.GetKeyDown(_placeTower2)) Tower.GetSecondPlacer().TryPlace();

            if (Input.GetKey(_maximizeCameraTower2)) Tower.GetSecondPlacer().MaximizeCamera();
            if (Input.GetKey(_minimizeCameraTower2)) Tower.GetSecondPlacer().MinimizeCamera();
            
            if (Input.GetKeyDown(_placeOrderTower2)) Tower.GetSecondPlacer().PlaceOrder();
            // if (Input.GetKey(KeyCode.RightControl)) _tower2.GetPlacer().DeleteAssistPlatform();
        }
        else
        {
            if (Input.GetKeyDown(_moveRightTower1)) BuyMenu.SetShower(Direction.Right, true);
            else if (Input.GetKeyDown(_moveLeftTower1)) BuyMenu.SetShower(Direction.Left, true);
            
            if (Input.GetKeyDown(_moveRightTower2)) BuyMenu.SetShower(Direction.Right, false);
            else if (Input.GetKeyDown(_moveLeftTower2)) BuyMenu.SetShower(Direction.Left, false);
            
            if (Input.GetKeyDown(_changeBlockTower1)) GameManager.ConfirmBuy(true);
            if (Input.GetKeyDown(_changeBlockTower2)) GameManager.ConfirmBuy(false);
            
            if (Input.GetKeyDown(_placeTower1)) BuyMenu.Buy(true);
            if (Input.GetKeyDown(_placeTower2)) BuyMenu.Buy(false);
        }
    }
}
