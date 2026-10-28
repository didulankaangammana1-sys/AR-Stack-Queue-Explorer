
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StackStepController : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform blockAnchor;
    public int capacity = 5;

    private readonly List<GameObject> blocks =
        new List<GameObject>();

    private readonly List<int> values =
        new List<int>();

    private enum Operation { None, Push, Pop }

    private Operation operation = Operation.None;
    private int step = 0;
    private int top = -1;
    private int pendingValue;

    public string CodeDisplay { get; private set; } =
        "Select an operation.";

    public string Status { get; private set; } =
        "Stack is empty.";

    private readonly string[] pushCode =
    {
        "if (top == capacity - 1) stop;",
        "top = top + 1;",
        "stack[top] = value;"
    };

    private readonly string[] popCode =
    {
        "if (top == -1) stop;",
        "value = stack[top];",
        "top = top - 1;"
    };

    public void BeginPush(int value)
    {
        if (operation != Operation.None)
            return;

        pendingValue = value;
        operation = Operation.Push;
        step = 0;
        Status = "Push ready. Press Next Step.";
        CodeDisplay = string.Join("\n", pushCode);
    }

    public void BeginPop()
    {
        if (operation != Operation.None)
            return;

        operation = Operation.Pop;
        step = 0;
        Status = "Pop ready. Press Next Step.";
        CodeDisplay = string.Join("\n", popCode);
    }

    public void NextStep()
    {
        if (operation == Operation.Push)
            ExecutePushStep();
        else if (operation == Operation.Pop)
            ExecutePopStep();
    }

    private void ExecutePushStep()
    {
        if (step == 0)
        {
            CodeDisplay = Highlight(pushCode, 0);

            if (top >= capacity - 1)
            {
                Status = "Stack Overflow!";
                operation = Operation.None;
                return;
            }

            Status = "Stack has available space.";
            step = 1;
        }
        else if (step == 1)
        {
            top++;
            CodeDisplay = Highlight(pushCode, 1);
            Status = "Top moved to " + top;
            step = 2;
        }
        else
        {
            GameObject block = Instantiate(
                blockPrefab, blockAnchor);

            block.transform.localPosition =
                new Vector3(0, 0.025f + top * 0.055f, 0);

            block.transform.localRotation = Quaternion.identity;

            TMP_Text label =
                block.GetComponentInChildren<TMP_Text>();

            if (label != null)
                label.text = pendingValue.ToString();

            blocks.Add(block);
            values.Add(pendingValue);

            CodeDisplay = Highlight(pushCode, 2);
            Status = "Pushed " + pendingValue;

            operation = Operation.None;
            step = 0;
        }
    }

    private void ExecutePopStep()
    {
        if (step == 0)
        {
            CodeDisplay = Highlight(popCode, 0);

            if (top < 0)
            {
                Status = "Stack Underflow!";
                operation = Operation.None;
                return;
            }

            Status = "Stack is not empty.";
            step = 1;
        }
        else if (step == 1)
        {
            int value = values[top];

            CodeDisplay = Highlight(popCode, 1);
            Status = "Read top value: " + value;
            step = 2;
        }
        else
        {
            int value = values[top];

            Destroy(blocks[top]);
            blocks.RemoveAt(top);
            values.RemoveAt(top);
            top--;

            CodeDisplay = Highlight(popCode, 2);
            Status = "Popped " + value;

            operation = Operation.None;
            step = 0;
        }
    }

    private string Highlight(string[] lines, int active)
    {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == active)
            {
                result.AppendLine(
                    "<color=#FFD166><b>▶ " +
                    lines[i] + "</b></color>");
            }
            else
            {
                result.AppendLine("  " + lines[i]);
            }
        }

        return result.ToString();
    }
}
