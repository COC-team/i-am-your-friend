using System;
using UnityEngine;

namespace Scenes.MainScene.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // Объект, за которым будет следить камера
        public Vector3 offset;   // Смещение относительно целевого объекта
        public float smoothSpeed = 0.1f; // Скорость сглаживания движения камеры
        public RectTransform canvasRectTransform; // Ссылка на RectTransform канваса

        private Camera cam;

        private void Start()
        {
            cam = Camera.main; // Получаем ссылку на камеру
        }

        private void FixedUpdate()
        {
            // Желаемая позиция камеры
            Vector3 desiredPosition = target.position + offset;

            // Получаем размеры канваса в мировых координатах
            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);
            float minX = canvasCorners[0].x;
            float maxX = canvasCorners[2].x;
            float minY = canvasCorners[0].y;
            float maxY = canvasCorners[2].y;

            // Учитываем ширину и высоту камеры (для 2D-режима)
            float cameraHeight = cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;

            // Ограничиваем позицию камеры, чтобы она не выходила за пределы канваса
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX + cameraWidth, maxX - cameraWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY + cameraHeight, maxY - cameraHeight);

            // Сглаживаем движение камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Применяем сглаженную позицию
            transform.position = smoothedPosition;

            // Устанавливаем фиксированную позицию Z для 2D (например, -10)
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}