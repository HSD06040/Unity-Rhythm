using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{    
    public float startY => GameManager.Instance.spawnLine.position.y;
    public float endY => GameManager.Instance.judgeLine.position.y;
    public int gridCount = 10;
    public float endRatio => (startY - endY) / gridCount; // ÇÑ Ä­ÀÇ Y

    public GameObject linePrefab;
    public GameObject notePrefab;
    public GameObject longPrefab;
    public GameObject curNote;

    [SerializeField] private Transform gridParent;
    [SerializeField] private float[] offset;
    private bool isLongNote;
    private float spawnY;
    private Camera cam;
    Vector3 worldPos;

    private void Awake()
    {
        cam = Camera.main;

        for (int i = 1; i < 3000; i++)
        {
            spawnY = endY + (endRatio * i);
            GameObject line = Instantiate(linePrefab, new Vector3(GameManager.Instance.judgeLine.position.x, spawnY,0), Quaternion.identity, gridParent);

            if(i % 10 == 0)
                line.GetComponent<SpriteRenderer>().color = Color.red;
            else if (i % 5 == 0)
                line.GetComponent<SpriteRenderer>().color = Color.yellow;

            line.GetComponent<GridLine>().Init(i);
        }
    }

    private void Update()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = -cam.transform.position.z;
        worldPos = cam.ScreenToWorldPoint(mPos);

        Debug.DrawRay(worldPos, cam.transform.forward * 2, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, cam.transform.forward, 2f);

        if (hit.transform == null)
            return;

        if (hit.transform.CompareTag("Grid"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GridLine line = hit.transform.GetComponent<GridLine>();

                float minDist = float.MaxValue;
                int idx = 0;

                for (int i = 0; i < offset.Length; i++)
                {
                    float offsetX = line.transform.position.x + offset[i];
                    float dist = Mathf.Abs(worldPos.x - offsetX);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        idx = i;
                    }
                }

                CreateNote(line, idx);
            }
        }

    }

    private void CreateNote(GridLine gridLine, int idx)
    {
        Vector3 spawnPos = new Vector3(gridLine.transform.position.x + offset[idx], gridLine.transform.position.y);
        Instantiate(notePrefab, spawnPos, Quaternion.identity);
    }
}
