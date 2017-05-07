using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {

    // SerializeField
    [SerializeField]
    private LabyrinthGenerator labyrinthGenerator;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject target;

    private void Start()
    {
        var width = 51;
        var height = 51;
        labyrinthGenerator.CreateLabyrinth(width, height);
        player.transform.position = new Vector3(2, 0.5f, (height * 2 - 4));
        target.transform.position = new Vector3((width * 2 - 4), 0.5f, 2);

        player.GetComponent<NavMeshAgent>().destination = target.transform.position;
    }
}
