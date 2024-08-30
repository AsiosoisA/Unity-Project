using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class FlameField : MonoBehaviour
    {
        BoxCollider2D box2D;

        private void Start()
        {
            box2D = GetComponent<BoxCollider2D>();
        }
        void EnableColider()
        {
            box2D.enabled = true;
        }

        void AnimationFinish()
        {
            Destroy(gameObject);
        }
    }
}
