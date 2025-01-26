using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.KillRoaches.Scripts
{
    public class RoachMovement : MonoBehaviour
    {
        public Animator animator;
        public float moveSpeed = 5f;
        public float maxMovementDistance = 10f;
        public SpriteRenderer background;
        private RectTransform _canvasRect;
        private Rigidbody2D _rb;
        private BoxCollider2D _collider;
        private Vector2 _roachSize;
        private Vector2 _movement;
        
        private float _backgroundMinX;
        private float _backgroundMaxX;
        private float _backgroundMinY;
        private float _backgroundMaxY;
        
        private Vector2 _startPosition = new Vector2(0, 0);
        private Vector2 _endPosition = new Vector2(0, 0);
        private Vector2 _direction = new Vector2(0, 0);

        private enum Direction
        {
            up, right, left, down, upRight, downRight, downLeft, upLeft
        }
        
        private Direction _currentDirection = Direction.left;

        public float spriteMaxWidth = 31f;
        public float spriteMaxHeight = 30f;

        private SpriteRenderer _spriteRenderer;
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = _rb.GetComponent<BoxCollider2D>();
            _canvasRect = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<RectTransform>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Sprite sprite = _spriteRenderer.sprite;
            
            Vector3 worldSize = Vector3.Scale(sprite.bounds.size, gameObject.transform.lossyScale);
            _roachSize = new Vector2(worldSize.x, worldSize.y);
            
            Vector3[] canvasCorners = new Vector3[4];
            _canvasRect.GetWorldCorners(canvasCorners);

            var bounds = background.bounds;
            _backgroundMinX = bounds.min.x;
            _backgroundMaxX = bounds.max.x;
            _backgroundMinY = bounds.min.y;
            _backgroundMaxY = bounds.max.y;
            float roachWidthHalf = _roachSize.x / 2;
            float roachHeightHalf = _roachSize.y / 2;
            
            // _rb.position = new Vector2(Random.Range(_canvasMinX + roachWidthHalf, _canvasMaxX - roachWidthHalf),
            //     Random.Range(_canvasMinY + roachHeightHalf , _canvasMaxY - roachHeightHalf));
            GetRandomMovement();
        }

        public void Death()
        {
            Debug.Log("Death");
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Objects"))
            {
                GetRandomMovement();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Objects"))
            {
                GetRandomMovement();
            }
        }

        void Update()
        {
            float threshold = moveSpeed * 1f;
            float distanceFromEndPositionToRb = Mathf.Abs(_endPosition.x - _rb.position.x) + Mathf.Abs(_endPosition.y - _rb.position.y);
            if (distanceFromEndPositionToRb <= threshold)
            {
                GetRandomMovement();
            }
            
            _direction = _endPosition - _rb.position;

            float horizontal = 0;
            float vertical = 0;
            switch (_currentDirection)
            {
                case Direction.up:
                    vertical = 1;
                    break;
                case Direction.right:
                    horizontal = 1;
                    break;
                case Direction.left:
                    horizontal = -1;
                    break;
                case Direction.down:
                    vertical = -1;
                    break;
                case Direction.upRight:
                    vertical = 1;
                    horizontal = 1;
                    break;
                case Direction.downRight:
                    vertical = -1;
                    horizontal = 1;
                    break;
                case Direction.downLeft:
                    vertical = -1;
                    horizontal = -1;
                    break;
                case Direction.upLeft:
                    vertical = 1;
                    horizontal = -1;
                    break;
            }
            
            animator.SetFloat("MoveX", horizontal);
            animator.SetFloat("MoveY", vertical);
    
            // Create a movement vector
            _movement = new Vector2(horizontal, vertical);

            // Normalize only when moving diagonally
            if (_movement.sqrMagnitude > 1)
            {
                _movement = _movement.normalized;
            }
        }

        private void GetRandomMovement()
        {
            _startPosition = new Vector2(_rb.position.x, _rb.position.y);
            _currentDirection = (Direction) Random.Range(0, 7);
            Vector2 vectorDirection = Vector2.zero;
            switch (_currentDirection)
            {
                case Direction.up:
                    vectorDirection = Vector2.up;
                    break;
                case Direction.right:
                    vectorDirection = Vector2.right;
                    break;
                case Direction.left:
                    vectorDirection = Vector2.left;
                    break;
                case Direction.down:
                    vectorDirection = Vector2.down;
                    break;
                case Direction.upRight:
                    vectorDirection = Vector2.right + Vector2.up;
                    break;
                case Direction.downRight:
                    vectorDirection = Vector2.right + Vector2.down;
                    break;
                case Direction.downLeft:
                    vectorDirection = Vector2.left + Vector2.down;
                    break;
                case Direction.upLeft:
                    vectorDirection = Vector2.left + Vector2.up;
                    break;
            }
            _endPosition = _startPosition + vectorDirection * maxMovementDistance;
            _direction = _endPosition - _startPosition;
        }
        
        private void FixedUpdate()
        {
            float halfSpriteSizeX = spriteMaxWidth / 2;
            float halfSpriteSizeY = spriteMaxHeight / 2;
            Vector2 newPosition = _rb.position + moveSpeed * Time.fixedDeltaTime * _movement;

            if (newPosition.x > _backgroundMaxX - halfSpriteSizeX)
            {
                var intersection = GetIntersectionX(_backgroundMaxX);
                newPosition.x = intersection.x - halfSpriteSizeX;
                GetRandomMovement();
            } else if (newPosition.x < _backgroundMinX + halfSpriteSizeX)
            {
                var intersection = GetIntersectionX(_backgroundMinX);
                newPosition.x = intersection.x + halfSpriteSizeX;
                GetRandomMovement();
            }
            
            if (newPosition.y > _backgroundMaxY - halfSpriteSizeY)
            {
                var intersection = GetIntersectionY(_backgroundMaxY);
                newPosition.y = intersection.y - halfSpriteSizeY;
                GetRandomMovement();
            } else if (newPosition.y < _backgroundMinY + halfSpriteSizeY)
            {
                var intersection = GetIntersectionY(_backgroundMinY);
                newPosition.y = intersection.y + halfSpriteSizeY;
                GetRandomMovement();
            }
            _rb.MovePosition(newPosition);
            // Debug.Log($"Roach position - {_rb.position}");
        }
        
        Vector2 GetIntersectionY(float y)
        {
            Vector2 dir = new Vector2((_startPosition.x + _direction.x) / 2, (_startPosition.y + _direction.y) / 2);
            // Calculate the parameter t where the intersection occurs
            float t = (y - _startPosition.y) / dir.y;

            // Calculate the intersection point
            float x = _startPosition.x + dir.x * t;

            // Return the intersection point (x, y)
            return new Vector2(x, y);
        }
        
        Vector2 GetIntersectionX(float x)
        {
            Vector2 dir = new Vector2((_startPosition.x + _direction.x) / 2, (_startPosition.y + _direction.y) / 2);
            // Calculate the parameter t where the intersection occurs
            float t = (x - _startPosition.x) / dir.x;

            // Calculate the intersection point
            float y = _startPosition.y + dir.y * t;

            // Return the intersection point (x, y)
            return new Vector2(x, y);
        }
    }    
}
