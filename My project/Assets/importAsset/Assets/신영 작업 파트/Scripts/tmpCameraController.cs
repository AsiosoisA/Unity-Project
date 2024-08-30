using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpCameraController : MonoBehaviour
{
    Transform player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
