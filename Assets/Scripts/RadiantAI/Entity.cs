using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RadiantAI.Tasks;
using UnityEngine.Serialization;

namespace RadiantAI
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Character character;
        [SerializeField] private Animator animator;

        [SerializeField] private bool isBeingInterrupted;

        [SerializeField] private Task currentPrimaryTask;
        [SerializeField] private Task queuedTask;

        private Dictionary<TimeManager.Day, List<Task>> _weeklySchedule;
        private static readonly int Speed = Animator.StringToHash("speed");


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
        }

        private void Update()
        {
            animator.SetFloat(Speed, agent.velocity.normalized.magnitude);
            
            CheckForTasks();

            if (agent.transform.position == agent.destination)
            {
                if (currentPrimaryTask.arrivalTask != null)
                {
                    currentPrimaryTask = currentPrimaryTask.arrivalTask;
                    StartNewTask(currentPrimaryTask);
                }
            }
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
            agent.destination = task.goal;
            currentPrimaryTask = task;
        }

        private void ContinueTask(Task task)
        {
            agent.destination = task.goal;
        }

        private void Interruption(Task task)
        {
            queuedTask = currentPrimaryTask;
            currentPrimaryTask = task;
            isBeingInterrupted = true;
            agent.destination = currentPrimaryTask.goal;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interruption"))
            {
                Task interruptionTask = ScriptableObject.CreateInstance<Task>();
                interruptionTask.goal = other.gameObject.transform.position;
                Interruption(interruptionTask);
                StartCoroutine(debugToInterruptionEnd(other.gameObject));
                Debug.Log("Interrupted");
            }
        }

        IEnumerator debugToInterruptionEnd(GameObject other)
        {
            yield return new WaitForSeconds(20);
            
            Destroy(other);
            isBeingInterrupted = false;
            currentPrimaryTask = queuedTask;
            ContinueTask(currentPrimaryTask);
        }
    }
    
}
