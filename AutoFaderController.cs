using UnityEngine;

public class AutoFaderController : MonoBehaviour
{
    public AutoFaderController Parent;
    public AutoFaderExecution Execution;
    int childIndex = -1;

    public void MakeKids()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            this.enabled = false;
            AutoFaderController afd = child.AddComponent<AutoFaderController>();
            AutoFaderExecution afe = child.AddComponent<AutoFaderExecution>();
            afd.Execution = afe;
            afd.Parent = this;
            afd.Parent = this;
            afd.Parent = this;
            afe.Controller = afd;
            afd.MakeKids();
            afe.Initialize();
            afd.enabled = false;
            afe.enabled = false;
        }
    }

    public void Begin()
    {
        childIndex++;
        Execution.enabled = true;
        enabled = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.childCount > childIndex)
        {
            this.enabled = false;
            GameObject child = this.transform.GetChild(childIndex).gameObject;
            child.GetComponent<AutoFaderController>().Begin();
        }
        else
        {
            this.enabled = false;
            if (Parent == null)
            {
                Debug.Log("End Recording");

            }
        }
    }

    public bool HasMoreToExecute()
    {
        return this.transform.childCount > childIndex + 1;
    }
}
