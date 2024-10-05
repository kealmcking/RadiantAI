using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RadiantAI.Tasks;
using UnityEngine.Serialization;

namespace RadiantAI
{
    [RequireComponent(typeof(Inventory), typeof(Stats))]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Character character;
        [SerializeField] private Animator animator;

        [SerializeField] private Inventory inventory;
        [SerializeField] private Stats stats;

        [SerializeField] private bool isBeingInterrupted;
        [SerializeField] private bool isGathering = false;

        [SerializeField] private Task currentPrimaryTask;
        [SerializeField] private GameObject currentTarget;
        [SerializeField] private GameObject queuedTarget;
        [SerializeField] private Task queuedTask;

        private Dictionary<TimeManager.Day, List<Task>> _weeklySchedule;
        private static readonly int Speed = Animator.StringToHash("speed");

        [SerializeField] private GameObject torch;
        [SerializeField] private Item torchItem;
        private bool torchBool;

        [SerializeField] private float inventoryWeight;


        private void Awake()
        {
            //StartNewTask(character.schedule);
            
            _weeklySchedule = new Dictionary<TimeManager.Day, List<Task>>()
            {
                { TimeManager.Day.Monday, character.schedule.scheduledTasksMonday },
                { TimeManager.Day.Tuesday, character.schedule.scheduledTasksTuesday },
                { TimeManager.Day.Wednesday, character.schedule.scheduledTasksWednesday },
                { TimeManager.Day.Thursday, character.schedule.scheduledTasksThursday },
                { TimeManager.Day.Friday, character.schedule.scheduledTasksFriday },
                { TimeManager.Day.Saturday, character.schedule.scheduledTasksSaturday },
                { TimeManager.Day.Sunday, character.schedule.scheduledTasksSunday },
            };

            inventory.addItemToInventory(torchItem, 1);
        }

        private void Update()
        {
            inventoryWeight = inventory.currentInventoryWeight();
            animator.SetFloat(Speed, agent.velocity.normalized.magnitude);
            CheckForTasks();
            TimeOfDayMethods();

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        if (currentPrimaryTask == null) return;
                        StartArrivalAction(currentPrimaryTask.taskAction);
                    }
                }
            }
        }

        public Inventory getInventory()
        {
            return inventory;
        }

        public Character getCharacter()
        {
            return character;
        }

        private void StartArrivalAction(Action currentTaskAction)
        {
            animator.SetTrigger(currentTaskAction.actionAnimationTriggerString);
            currentTarget.GetComponent<InteractableObject>().beginInteraction(this);
            
        }

        public void startGatherResource(Resource _resource, int _yield, float baseGatherTime)
        {
            if (!isGathering)
            {
                isGathering = true;
                StartCoroutine(gatherResource(_resource, _yield, baseGatherTime));
            }
        }
        
        IEnumerator gatherResource(Resource _resource, int _yield, float baseGatherTime)
        {
            while (getInventory().currentInventoryWeight() + (_resource.itemWeight * _yield) <= getInventory().maxWeight)
            {
            
                yield return new WaitForSeconds(baseGatherTime);
            
                getInventory().addItemToInventory(_resource, _yield);
            }

            isGathering = false;
        }
        
        private void CheckForTasks()
        {
            TimeManager.Day currentDay = TimeManager.instance.getCurrentDay();

            if (_weeklySchedule.TryGetValue(currentDay, out List<Task> scheduledTasks))
            {
                foreach (Task task in scheduledTasks)
                {
                    if (task.startHour == TimeManager.instance.getCurrentHour() && task.startMinute == TimeManager.instance.getCurrentMinute())
                    {
                        queuedTask = task;
                        if (!isBeingInterrupted)
                        {
                            StartNewTask(task);
                        }
                    }
                }
            }
        }

        private void StartNewTask(Task task)
        {
            currentPrimaryTask = task;
            Action.ActionType action = task.taskAction.type;
            
            agent.destination = nearestInteractableObjects(returnHashSet(action)).gameObject.transform.position;
        }

        private HashSet<InteractableObject> returnHashSet(Action.ActionType action)
        {
            switch (action)
            {
                case Action.ActionType.Mine:
                    return InteractableObject.MineableObjects;
                case Action.ActionType.Chop:
                    return InteractableObject.ChoppableObjects;
                default:
                    return null;
            }
        }

        private InteractableObject nearestInteractableObjects(HashSet<InteractableObject> objectHashSet)
        {
            Vector3 pos = this.transform.position;
            float dist = float.PositiveInfinity;
            InteractableObject targ = null;
            
            foreach (var obj in objectHashSet)
            {
                var d = (pos - obj.transform.position).sqrMagnitude;

                if (d < dist)
                {
                    targ = obj;
                    dist = d;
                }
            }

            currentTarget = targ.gameObject;
            return targ.gameObject.GetComponent<InteractableObject>();
        }

        private void ContinueTask(Task task)
        {
            currentTarget = queuedTarget;
            agent.destination = currentTarget.gameObject.transform.position;
        }

        // COMMENTING THIS OUT BC IM TOO STUPID TO FIGURE IT OUT RIGHT NOW TBH - 10/05/2024 : Keagon
        
        // private void Interruption(Task task)
        // {
        //     queuedTask = currentPrimaryTask;
        //     queuedTarget = currentTarget;
        //     currentPrimaryTask = task;
        //     
        //     isBeingInterrupted = true;
        //     agent.destination = currentPrimaryTask.goal;
        // }
        //
        // public void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Interruption"))
        //     {
        //         Task interruptionTask = ScriptableObject.CreateInstance<Task>();
        //         interruptionTask.goal = other.gameObject.transform.position;
        //         Interruption(interruptionTask);
        //         StartCoroutine(debugToInterruptionEnd(other.gameObject));
        //         Debug.Log("Interrupted");
        //     }
        // }

        IEnumerator debugToInterruptionEnd(GameObject other)
        {
            yield return new WaitForSeconds(20);
            
            Destroy(other);
            isBeingInterrupted = false;
            currentPrimaryTask = queuedTask;
            ContinueTask(currentPrimaryTask);
        }

        int GetMainTerrainTexture(float x, float z, TerrainData terrainData)
        {
            float[,,] alphamap = terrainData.GetAlphamaps(
                (int)(x * terrainData.alphamapWidth), 
                (int)(z * terrainData.alphamapHeight), 
                1, 
                1);

            float maxWeight = 0;
            int maxIndex = 0;

            for (int i = 0; i < alphamap.GetLength(2); i++)
            {
                if (alphamap[0, 0, i] > maxWeight)
                {
                    maxWeight = alphamap[0, 0, i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        void TimeOfDayMethods()
        {
            if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Dawn)
            {
                if (!torchBool)
                {
                    EquipTorch(false);
                }
            } else if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Day)
            {
                torchBool = false;
            }
            else if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Dusk)
            {
                if (!torchBool)
                {
                    EquipTorch(true);
                }
            } else if (TimeManager.instance.getTimeOfDay() == TimeManager.TimeOfDay.Night)
            {
                torchBool = false;
            }
        }

        void EquipTorch(bool set)
        {
            for (int i = 0; i < inventory.inventory.Count; i++)
            {
                if (inventory.inventory[i].item == torchItem)
                {
                    torch.SetActive(set);
                    torchBool = true;
                    return;
                }
            }

            if (!set)
            {
                torch.SetActive(false);
                torchBool = false;
            }
        }
    }
    
}
