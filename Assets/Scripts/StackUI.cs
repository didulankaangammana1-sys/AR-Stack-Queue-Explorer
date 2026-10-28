
using TMPro;
using UnityEngine;

public class StackUI : MonoBehaviour
{
    public ARPlacement placement;
    public TMP_Text codeText;
    public TMP_Text statusText;

    private StackStepController GetStack()
    {
        if (placement.PlacedObject == null)
            return null;

        return placement.PlacedObject
            .GetComponent<StackStepController>();
    }

    public void Push10()
    {
        StackStepController stack = GetStack();

        if (stack == null)
        {
            statusText.text = "Place the stack first.";
            return;
        }

        stack.BeginPush(10);
        Refresh(stack);
    }

    public void Push20()
    {
        StackStepController stack = GetStack();

        if (stack == null)
        {
            statusText.text = "Place the stack first.";
            return;
        }

        stack.BeginPush(20);
        Refresh(stack);
    }

    public void Pop()
    {
        StackStepController stack = GetStack();

        if (stack == null)
        {
            statusText.text = "Place the stack first.";
            return;
        }

        stack.BeginPop();
        Refresh(stack);
    }

    public void NextStep()
    {
        StackStepController stack = GetStack();

        if (stack == null)
        {
            statusText.text = "Place the stack first.";
            return;
        }

        stack.NextStep();
        Refresh(stack);
    }

    private void Refresh(StackStepController stack)
    {
        codeText.text = stack.CodeDisplay;
        statusText.text = stack.Status;
    }
}
