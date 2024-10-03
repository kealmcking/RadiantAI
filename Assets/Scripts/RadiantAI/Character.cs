using UnityEngine;
using RadiantAI.Scheduling;

namespace RadiantAI
{
    [CreateAssetMenu(fileName = "Character", menuName = "RadiantAI/CharacterObject")]
    public class Character : ScriptableObject
    {
        public string name;
        public int hp;

        public int responsibility;

        public Schedule schedule;


    }
    
}
