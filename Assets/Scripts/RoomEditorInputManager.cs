
using System;
using UnityEngine;

namespace Assets.Scripts
{
    class RoomEditorInputManager : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject indicatorSquare;

        RoomEditorManager manager;

        int lastX, lastY;

        private void Start()
        {
            manager = GetComponent<RoomEditorManager>();
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;

            var point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

            var placementX = Mathf.FloorToInt(point.x);
            var placementY = Mathf.FloorToInt(point.y);

            if (lastX != placementX || lastY != placementY) {
                indicatorSquare.transform.position = new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, -1);

                lastX = placementX;
                lastY = placementY;
            }

            var gridX = placementX/10;
            var gridY = placementY/10;

            if (!Input.GetButton("Fire1") || !IsInbounds(gridX, gridY))
                return;

            manager.SetGridValue(gridX, gridY);
        }

        private bool IsInbounds(int x, int y){
            return x >= 0 && x < 10 && y >= 0 && y < 10;
        }
    }
}
