using System.Collections;
using System.Collections.Generic;
using RadiantAI.Tasks;
using UnityEngine;

namespace RadiantAI
{
    [CreateAssetMenu(fileName = "Task", menuName = "RadiantAI/Tasks/RetrieveTask")]
    public class Task_Retrieve : Task
    {
        public string goalName;

    }
}
