using System;
using UnityEngine;

namespace Scenes.MainScene.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public Animator animator;
        public float moveSpeed = 5f;
        private Rigidbody2D rb;
        private Vector2 movement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // A (-1) and D (1)
            float vertical = Input.GetAxisRaw("Vertical");     // W (1) and S (-1)
            
            // animator.SetFloat("Horizontal", horizontal);
            // animator.SetFloat("Vertical", vertical);

            // Create a movement vector
            movement = new Vector2(horizontal, vertical);

            // Normalize only when moving diagonally
            if (movement.sqrMagnitude > 1)
            {
                movement = movement.normalized;
            }
        }

        private void FixedUpdate()
        {
            Vector2 newPosition = rb.position + moveSpeed * Time.fixedDeltaTime * movement;
            rb.MovePosition(newPosition);
        }
    }
}

