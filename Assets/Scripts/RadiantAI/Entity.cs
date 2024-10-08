using System;
using System.Collections;
using System.Collections.Generic;
using RadiantAI.Scheduling;
using RadiantAI.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace RadiantAI
{
    [RequireComponent(typeof(Inventory), typeof(Stats))]
    public class Entity : MonoBehaviour
    {
        private EventBinding<DoTaskEvent> doTaskEventBinding;

        private Character character;
        private NavMeshAgent agent;
        private Animator animator;
        private Inventory inventory;

        private GameObject currentTarget;

        private bool hasStartedArrivalAction;
        private bool isGathering;

        private Task queuedTask;
        
        [SerializeField] private GameObject torch;
        [SerializeField] private Item torchItem;
        private bool torchBool;
        
        [SerializeField] private GameObject pickAxe;
        [SerializeField] private Item pickAxeItem;
        
        private GameObject activeObject;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            animator.SetFloat("speed", agent.velocity.normalized.magnitude);
        }

        public Character GetCharacter()
        {
            return character;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return agent;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public GameObject GetCurrentTarget()
        {
            return currentTarget;
        }

        public bool GetHasStartedArrivalAction()
        {
            return hasStartedArrivalAction;
        }

        public void SetHasStartedArrivalAction(bool set)
        {
            hasStartedArrivalAction = set;
        }
        
        public InteractableObject nearestInteractableObjects(HashSet<InteractableObject> objectHashSet)
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
            
            return targ.gameObject.GetComponent<InteractableObject>();
        }
        
        public void startGatherResource(Resource _resource, int _yield, float baseGatherTime)
        {
            if (!isGathering)
            {
                isGathering = true;
                StartCoroutine(gatherResource(_resource, _yield, baseGatherTime));
                if (itemInInventory(itemToEnable(_resource)))
                {
                    toolToEnable(_resource).SetActive(true);
                    torch.SetActive(false);
                    torchBool = false;
                }
            }
        }
        
        bool itemInInventory(Item _item)
        {
            bool set = false;
            for (int i = 0; i < inventory.inventory.Count; i++)
            {
                if (inventory.inventory[i].item == _item)
                {
                    set = true;
                }
            }

            return set;
        }
        
        // Turn the following two methods into an event, so that
        // 1. I don't need both methods
        // 2. I can get this stuff out of here and have it activated elsewhere
        private GameObject toolToEnable(Resource _resource)
        {
            Resource.ResourceType type = _resource.type;
            GameObject returnObject = null;
            
            switch (type)
            {
                case Resource.ResourceType.Ore:
                    returnObject = pickAxe;
                    activeObject = returnObject;
                    break;
                default:
                    break;
            }

            return returnObject;
        }
        
        private Item itemToEnable(Resource _resource)
        {
            Resource.ResourceType type = _resource.type;
            Item returnItem = null;
            
            switch (type)
            {
                case Resource.ResourceType.Ore:
                    returnItem = pickAxeItem;
                    break;
                default:
                    break;
            }

            return returnItem;
        }
        
        IEnumerator gatherResource(Resource _resource, int _yield, float baseGatherTime)
        {
            while (inventory.currentInventoryWeight() + (_resource.itemWeight * _yield) <= inventory.maxWeight)
            {
                yield return new WaitForSeconds(baseGatherTime);
            
                inventory.addItemToInventory(_resource, _yield);
            }
            
            animator.SetTrigger("_returnIdle");
            isGathering = false;
        }
        
        
        public IEnumerator CheckArrival(Task task)
        {
            while (agent.pathPending)
            {
                yield return null;
            }

            while (agent.remainingDistance > agent.stoppingDistance || agent.velocity.sqrMagnitude > 0f)
            {
                yield return null;
            }
            
            if (!hasStartedArrivalAction)
            {
                animator.SetTrigger(task.npcAction.actionAnimationTriggerString);
                currentTarget.GetComponent<InteractableObject>().beginInteraction(this);
                SetHasStartedArrivalAction(true);
            }
        }

        
    }

}
