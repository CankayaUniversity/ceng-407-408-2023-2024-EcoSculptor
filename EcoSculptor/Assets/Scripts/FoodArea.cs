using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a collection of flower plants and attached flowers
/// </summary>
public class FoodArea : MonoBehaviour
{
    // The diameter of the area where the agent and flowers can be
    // used for observing relative distance from agent to flower
    public const float AreaDiameter = 4f;

    // The list of all flower plants in this flower area (flower plants have multiple flowers)
    private List<GameObject> foodClusters;

    // A lookup dictionary for looking up a flower from a nectar collider
    private Dictionary<Collider, Food> foodDictionary;

    /// <summary>
    /// The list of all flowers in the flower area
    /// </summary>
    public List<Food> Foods { get; private set; }

    /// <summary>
    /// Reset the flowers and flower plants
    /// </summary>
    public void ResetFoods()
    {
        // Rotate each flower plant around the Y axis and subtly around X and Z
        foreach (GameObject foodCluster in foodClusters)
        {
            float xRotation = UnityEngine.Random.Range(-5f, 5f);
            float yRotation = UnityEngine.Random.Range(-180f, 180f);
            float zRotation = UnityEngine.Random.Range(-5f, 5f);
            foodCluster.transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        // Reset each flower
        foreach (Food food in Foods)
        {
            food.ResetFood();
        }
    }

    /// <summary>
    /// Gets the <see cref="Flower"/> that a nectar collider belongs to
    /// </summary>
    /// <param name="collider">The nectar collider</param>
    /// <returns>The matching flower</returns>
    public Food GetFood(Collider collider)
    {
        return foodDictionary[collider];
    }

    /// <summary>
    /// Called when the area wakes up
    /// </summary>
    private void Awake()
    {
        // Initialize variables
        foodClusters = new List<GameObject>();
        foodDictionary = new Dictionary<Collider, Food>();
        Foods = new List<Food>();
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        // Find all flowers that are children of this GameObject/Transform
        FindChildFoods(transform);
    }

    /// <summary>
    /// Recursively finds all flowers and flower plants that are children of a parent transform
    /// </summary>
    /// <param name="parent">The parent of the children to check</param>
    private void FindChildFoods(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.CompareTag("flower_plant"))
            {
                // Found a flower plant, add it to the flowerPlants list
                foodClusters.Add(child.gameObject);

                // Look for flowers within the flower plant
                FindChildFoods(child);
            }
            else
            {
                // Not a flower plant, look for a Flower component
                Food food = child.GetComponent<Food>();
                if (food != null)
                {
                    // Found a flower, add it to the Flowers list
                    Foods.Add(food);

                    // Add the nectar collider to the lookup dictionary
                    foodDictionary.Add(food.foodCollider, food);

                    // Note: there are no flowers that are children of other flowers
                }
                else
                {
                    // Flower component not found, so check children
                    FindChildFoods(child);
                }
            }
        }
    }
}
