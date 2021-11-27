using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] private BaseController greenController;
    [SerializeField] private BaseController redController;

    private Dictionary<BugSide, BaseController> sideToController;

    private BugSide currentActionSide;
    private IEnumerator currentAction;

    void Awake()
    {
        this.sideToController = new Dictionary<BugSide, BaseController>();
        this.sideToController.Add(BugSide.GREEN, greenController);
        this.sideToController.Add(BugSide.RED, redController);
    }

    public void StartLevel(BaseLevel level)
    {
        this.greenController.Init(level, BugSide.GREEN);
        this.redController.Init(level, BugSide.RED);
        StartTurn(level.CurrentSide);
    }

    public void StartTurn(BugSide side)
    {
        if (currentAction != null)
        {
            EndTurn();
        }

        BaseController controller = sideToController[side];
        controller.OnStartTurn();
        currentActionSide = side;
        currentAction = controller.TurnAction();
        StartCoroutine(currentAction);
    }

    public void EndTurn()
    {
        if (currentAction != null)
        {
            StopCoroutine(currentAction);
            sideToController[currentActionSide].OnEndTurn();
            currentAction = null;
        }
    }

    public void EndLevel()
    {
        EndTurn();
        greenController.Reset();
        redController.Reset();
    }
}
