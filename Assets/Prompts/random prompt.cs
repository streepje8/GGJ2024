using System.Collections.Generic;
using UnityEngine;

public class randomprompt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var Prompts = new List<string> { "Falling", "Booming", "Climbing", "Bruh", "Fruit", "Meeting", "Seeing", "Love", "Farts", "Woopsies", "Poopsies", "Schmingus", "Amongus", "What?", "Technology", "Silly", "Accident", "On purpose", "Me", "Someone else", "Sarcasm", "Stinky", "Gobble", "You", "Food", "Together", "Their fault", "My bad", "Loud", "Silent", "Dr. Doctor", "Mr. Stacks", "Bozo Bumpkin", "D. (detective)", "Pablo" };

        if (Input.GetKeyDown("h")) {
            int index = Random.Range(0, Prompts.Count);
            Debug.Log(Prompts[index]);
        }

    }
}
