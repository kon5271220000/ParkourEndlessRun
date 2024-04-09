using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;

    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;
    // Update is called once per frame
    void Update()
    {
        LevelGenerator();
        DeleteGenerator();
    }

    private void LevelGenerator()
    {
        while (Vector2.Distance(player.position, nextPartPosition) < distanceToSpawn)
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];
            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);
            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);
            nextPartPosition = newPart.Find("EndPoint").position;
        }
    }

    private void DeleteGenerator()
    {
        if(transform.childCount > 0)
        {
            Transform deletePart = transform.GetChild(0);

            if(Vector2.Distance(player.transform.position, deletePart.transform.position) > distanceToDelete)
            {
                Destroy(deletePart.gameObject);
            }
        }
    }
}
