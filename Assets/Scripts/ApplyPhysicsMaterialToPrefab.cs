using UnityEngine;

public class ApplyPhysicsMaterialToPrefab : MonoBehaviour
{
    public GameObject prefabRoot;
    public PhysicsMaterial2D physicsMaterial;

    void Start()
    {
        if (prefabRoot == null || physicsMaterial == null)
        {
            Debug.LogWarning("Assign both the prefab root and the material.");
            return;
        }

        Collider2D[] colliders = prefabRoot.GetComponentsInChildren<Collider2D>(true);
        foreach (var col in colliders)
        {
            col.sharedMaterial = physicsMaterial;
        }

        Debug.Log($"Applied material to {colliders.Length} colliders.");
    }
}
