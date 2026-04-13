using UnityEngine;
using SuperTiled2Unity;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float pixelsPerUnit = 16f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minOrthographicSize = 2f;
    [SerializeField] private float maxOrthographicSize = 15f;

    private Camera targetCamera;
    private float minWorldX, maxWorldX, minWorldY, maxWorldY;
    private bool boundsInitialized = false;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        CalculateFullWorldBounds();
    }

    public void CalculateFullWorldBounds()
    {
        SuperMap[] allMaps = FindObjectsByType<SuperMap>(FindObjectsSortMode.None);
        if (allMaps.Length == 0) return;

        minWorldX = float.MaxValue;
        maxWorldX = float.MinValue;
        minWorldY = float.MaxValue;
        maxWorldY = float.MinValue;

        foreach (SuperMap map in allMaps)
        {
            float width = map.m_Width * map.m_TileWidth / pixelsPerUnit;
            float height = map.m_Height * map.m_TileHeight / pixelsPerUnit;
            Vector3 pos = map.transform.position;

            minWorldX = Mathf.Min(minWorldX, pos.x);
            maxWorldX = Mathf.Max(maxWorldX, pos.x + width);
            minWorldY = Mathf.Min(minWorldY, pos.y - height);
            maxWorldY = Mathf.Max(maxWorldY, pos.y);
        }

        boundsInitialized = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void LateUpdate()
    {
        if (boundsInitialized)
        {
            ClampCamera();
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            float newSize = targetCamera.orthographicSize - (scrollInput * zoomSpeed);
            targetCamera.orthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
        }
    }

    private void ClampCamera()
    {
        float camHeight = targetCamera.orthographicSize;
        float camWidth = camHeight * targetCamera.aspect;

        float minX = minWorldX + camWidth;
        float maxX = maxWorldX - camWidth;
        float minY = minWorldY + camHeight;
        float maxY = maxWorldY - camHeight;

        if (maxWorldX - minWorldX < camWidth * 2)
        {
            minX = maxX = (minWorldX + maxWorldX) / 2f;
        }

        if (maxWorldY - minWorldY < camHeight * 2)
        {
            minY = maxY = (minWorldY + maxWorldY) / 2f;
        }

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}