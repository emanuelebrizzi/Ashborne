using UnityEngine;

public class Player : MonoBehaviour
{
    public SPUM_Prefabs spumPrefabs;

    private void Awake()
    {
        if (spumPrefabs == null)
        {
            spumPrefabs = GetComponentInChildren<SPUM_Prefabs>();
        }

        if (spumPrefabs == null)
        {
            Debug.LogError("SPUM_Prefabs component not found on Hero or its children!");
        }
    }

    private void Start()
    {
        if (spumPrefabs != null)
        {
            if (!spumPrefabs.allListsHaveItemsExist())
            {
                spumPrefabs.PopulateAnimationLists();
            }

            spumPrefabs.OverrideControllerInit();
        }
    }

    public void PlayAnimation(PlayerState state, int index = 0)
    {
        if (spumPrefabs != null)
        {
            spumPrefabs.PlayAnimation(state, index);
        }
    }
}
