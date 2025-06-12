using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SPUM_Prefabs spumPrefabs;

    private void Start()
    {
        spumPrefabs = GetComponent<SPUM_Prefabs>();
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
