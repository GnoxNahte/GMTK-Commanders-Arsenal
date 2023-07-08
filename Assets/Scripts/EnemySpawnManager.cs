using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] SpriteRenderer enemyPreview;
    [SerializeField] GameObject enemySpawnSpriteUI_Prefab;
    [SerializeField] RectTransform enemySpawnParentUI;

    [SerializeField]
    [ReadOnly]
    private GameObject selectedEnemyPrefab;

    // Cache
    private Camera cam;

    private ObjectPool<GameObject> pool;

    #region Public Methods
    public void ChangeSelectedEnemy(GameObject nextEnemyPrefab)
    {
        selectedEnemyPrefab = nextEnemyPrefab;

        enemyPreview.sprite = GetEnemySprite(selectedEnemyPrefab);
        enemyPreview.transform.localScale = selectedEnemyPrefab.transform.localScale;
    }
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("EnemySpawnManager: enemyPrefabs == null || enemyPrefabs.Length == 0");
        }

        ChangeSelectedEnemy(enemyPrefabs[0]);

        pool = new ObjectPool<GameObject>(
            () => { return Instantiate(selectedEnemyPrefab); }, // create
            enemy => enemy.SetActive(true), // On Get
            enemy => enemy.SetActive(false), // On Release
            enemy => Destroy(enemy.gameObject), // On Destroy
            true, // Collection check
            200 // Default capacity
            );

        // ===== INIT UI =====
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            GameObject imageUI = Instantiate(enemySpawnSpriteUI_Prefab, enemySpawnParentUI);
            imageUI.GetComponent<Image>().sprite = GetEnemySprite(enemyPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyLocation = cam.ScreenToWorldPoint(Input.mousePosition);
        enemyPreview.transform.position = enemyLocation;

        enemyPreview.flipX = HeroAI.instance.transform.position.x < enemyPreview.transform.position.x;
        print("A: " + HeroAI.instance.transform.position.x + "B: " + enemyPreview.transform.position.x);
        // Enemy prefab
        if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(selectedEnemyPrefab, enemyLocation, Quaternion.identity);
            GameObject enemy = pool.Get();
            enemy.transform.position = enemyLocation;
        }

#if UNITY_EDITOR
        print($"total: {pool.CountAll} \nactive: {pool.CountActive}\n inactive: {pool.CountInactive}");
#endif
    }
    #endregion

    #region Private Methods
    Sprite GetEnemySprite(GameObject enemyPrefab)
    {
        return enemyPrefab.GetComponent<SpriteRenderer>().sprite;
    }
    #endregion
}
