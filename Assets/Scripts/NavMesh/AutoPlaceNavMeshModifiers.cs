using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[ExecuteInEditMode]
public class AutoPlaceNavMeshModifiers : MonoBehaviour
{
    private Terrain terrain;
    
    private void OnValidate()
    {
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
            PlaceNavMeshModifiers(terrain, 1,-200,200);
        }
    }

    void PlaceNavMeshModifiers(Terrain terrain, int terrainLayerIndex, float minHeight, float maxHeight)
    {
        GameObject parentObject = new GameObject("NavMeshModifierParent");
        TerrainData terrainData = terrain.terrainData;

        // Loop through the terrain's alphamap
        for (int x = 0; x < terrainData.alphamapWidth; x++)
        {
            for (int z = 0; z < terrainData.alphamapHeight; z++)
            {
                float[,,] alphamaps = terrainData.GetAlphamaps(x, z, 1, 1);
                float influence = alphamaps[0, 0, terrainLayerIndex];

                // Look for strong influences indicating path center
                if (influence > 0.7f) // Targeting higher influence areas
                {
                    // Calculate the position in the world based on the alphamap coordinates
                    float normalizedX = x / (float)terrainData.alphamapWidth;
                    float normalizedZ = z / (float)terrainData.alphamapHeight;

                    // Convert normalized coordinates into world space
                    Vector3 terrainPosition = new Vector3(
                        normalizedX * terrainData.size.x,
                        0, // Temporary height
                        normalizedZ * terrainData.size.z
                    );

                    // Use SampleHeight to find the correct height at this position
                    float height = terrain.SampleHeight(terrain.transform.position + terrainPosition);

                    // Check if height is within specified range
                    if (height >= minHeight && height <= maxHeight)
                    {
                        // Adjust the position to be relative to the terrain's position in the world
                        Vector3 worldPosition = terrain.transform.position + new Vector3(terrainPosition.x, height, terrainPosition.z);

                        // Create a NavMeshModifierVolume at this world position
                        GameObject modifierObj = new GameObject("NavMeshModifierVolume");
                        modifierObj.transform.position = worldPosition;

                        NavMeshModifierVolume volume = modifierObj.AddComponent<NavMeshModifierVolume>();
                        volume.area = NavMesh.GetAreaFromName("Dirt"); // Ensure 'Dirt' area exists
                        volume.size = new Vector3(2, 10, 2); // Adjust the size as needed
                        
                        // Parent to the modifier parent object
                        modifierObj.transform.parent = parentObject.transform;
                    }
                }
            }
        }
    }
}

