using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    [SerializeField]
    public Room[] roomPrefab;

    private void Generate()
    {
        Graph graph = new Graph();
        var infos = graph.Generate1(15);

        foreach (var pos in infos.Keys)
        {

            Room prefabToInsantiate = roomPrefab[Random.Range(0, roomPrefab.Length)];
            
            var room = Instantiate(prefabToInsantiate, new Vector3(pos.x, pos.y) * 23f, Quaternion.identity);
            room.Setup(infos[pos]);
        }
    }

    void Start()
    {
        Generate();
    }

}