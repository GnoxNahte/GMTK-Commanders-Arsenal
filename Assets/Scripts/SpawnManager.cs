using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPrefabs;
    [SerializeField] SpriteRenderer spawnPreview;

    [SerializeField]
    [ReadOnly]
    private GameObject selectedSpawnPrefab;

    // Cache
    private Camera cam;

    private ObjectPool<GameObject> pool;

    #region Public Methods
    public void ChangeSelectedSpawn(GameObject nextSpawnPrefab)
    {
        selectedSpawnPrefab = nextSpawnPrefab;
        spawnPreview.sprite = GetSpawnSprite(selectedSpawnPrefab);
    }
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            Debug.LogError("Spawn manager: spawnPrefabs == null || spawnPrefabs.Length == 0");
        }

        ChangeSelectedSpawn(spawnPrefabs[0]);

        pool = new ObjectPool<GameObject>(
            () => { return Instantiate(selectedSpawnPrefab); }, // create
            enemy => enemy.SetActive(true), // On Get
            enemy => enemy.SetActive(false), // On Release
            enemy => Destroy(enemy.gameObject), // On Destroy
            true, // Collection check
            200 // Default capacity
            );
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 spawnLocation = cam.ScreenToWorldPoint(Input.mousePosition);
        spawnPreview.transform.position = spawnLocation;

        // Spawn prefab
        if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(selectedSpawnPrefab, spawnLocation, Quaternion.identity);
            GameObject enemy = pool.Get();
            enemy.transform.position = spawnLocation;
        }

        print($"total: {pool.CountAll} \nactive: {pool.CountActive}\n inactive: {pool.CountInactive}");
    }
    #endregion

    #region Private Methods
    Sprite GetSpawnSprite(GameObject spawnPrefab)
    {
        return spawnPrefab.GetComponent<SpriteRenderer>().sprite;
    }
    #endregion
}
