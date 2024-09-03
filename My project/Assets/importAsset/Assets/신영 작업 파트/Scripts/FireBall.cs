using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{

    public class FireBall : MonoBehaviour
    {
        GameObject player;
        Rigidbody2D playerRigid2D;
        Rigidbody2D rd;

        float lifeTime = 3.0f;
        float startTime;

        private void Start()
        {
            startTime = Time.time;

            player = transform.parent.parent.gameObject;
            playerRigid2D = player.GetComponentInParent<Rigidbody2D>();
            rd = GetComponent<Rigidbody2D>();
            transform.SetParent(null);

            if (player.transform.eulerAngles.y >= 150)
                rd.velocity = new Vector2(-10f, 0);
            else
                rd.velocity = new Vector2(10f, 0);

        }
        void FixedUpdate()
        {

            if (Time.time - startTime > lifeTime)
                Destroy(gameObject);
            Debug.Log(playerRigid2D.velocity.y);
        }
    }
}
