using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] private BaseController bottomController;
    [SerializeField] private BaseController topController;

    private Dictionary<BugSide, BaseController> sideToController;

    private BugSide currentActionSide;
    private IEnumerator currentAction;

    void Awake()
    {
        this.sideToController = new Dictionary<BugSide, BaseController>();
        this.sideToController.Add(BugSide.BOTTOM, bottomController);
        this.sideToController.Add(BugSide.TOP, topController);
    }

    public void StartLevel(BaseLevel level)
    {
        this.bottomController.Init(level, BugSide.BOTTOM);
        this.topController.Init(level, BugSide.TOP);
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
        bottomController.Clear();
        topController.Clear();
    }
}
