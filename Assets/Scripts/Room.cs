using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Openings doors;

    [SerializeField]
    private Openings walls;

    public void Setup(HashSet<Vector2Int> config)
    {
        foreach (var wall in walls.GetOpenings())
        {
            bool contains = config.Contains(wall.Pos);
            wall.GameObject.SetActive(!contains);
        }
    }

    void Start()
    {
        
    }
}