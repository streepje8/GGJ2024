using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2,fileName = "New Funny Prompts", menuName = "hihi/funnyprompts")]
public class FunnyPrompts : ScriptableObject
{
    [field: SerializeField]
    public List<string> Prompts { get; private set; } = new List<string> { "Falling", "Booming", "Climbing", "Bruh", "Fruit", "Meeting", "Seeing", "Love", "Farts", "Woopsies", "Poopsies", "Schmingus", "Amongus", "What?", "Technology", "Silly", "Accident", "On purpose", "Me", "Someone else", "Sarcasm", "Stinky", "Gobble", "You", "Food", "Together", "Their fault", "My bad", "Loud", "Silent", "Dr. Doctor", "Mr. Stacks", "Bozo Bumpkin", "D. (detective)", "Pablo" };
}
