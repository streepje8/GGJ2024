using UnityEngine;

public class FunnyPopulator : MonoBehaviour
{
    public GameObject funnyButtonPrefab;
    private void Awake()
    {
        if (!PersistentGameState.TryFindObject("Data", out PersistentGameData data))
        {
            enabled = false;
            return;
        }

        foreach (var funnySetFunny in data.FunnySet.Funnies)
        {
            FunnyButton funnySetFunnyButton = Instantiate(funnyButtonPrefab, transform).GetComponent<FunnyButton>();
            funnySetFunnyButton.SetFunny(funnySetFunny);
        }
    }
}
