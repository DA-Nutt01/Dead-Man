using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-50)]
public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance {get; private set;}

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase fruitTile;
    private Vector3Int fruitPos; 
    private int fruitLifetime = 10;

    public int pelletsEaten {get; private set;} = 0;
    public int fruitSpawnCount {get; private set;} = 0;


    private void OnEnable(){
        Pellet.OnPelletEaten += Pellet_OnPelletEaten;
        GameManager.OnLevelStart += GameManager_OnLevelStart;
    }

    private void OnDisable(){
        Pellet.OnPelletEaten -= Pellet_OnPelletEaten;
        GameManager.OnLevelStart -= GameManager_OnLevelStart;
    }

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Pellet_OnPelletEaten(Pellet pellet){
        //Subscribing function to OnPelletEatenEvent from Pellet
        pelletsEaten++;
        //Debug.Log($"Pellets: {pelletsEaten}");
        if ((pelletsEaten == 70 || pelletsEaten == 170) && fruitSpawnCount < 2)
        {
            SpawnFruit();
        }
    }

    private void GameManager_OnLevelStart(){
        // Subscribing function to OnLevelStart from GameManager
        // Reset cookies
        pelletsEaten = 0;
        fruitSpawnCount = 0;
        RemoveFruit();
    }

    private void SpawnFruit(){
        // Spawn the fruit based on its corresponding level
        fruitPos = new Vector3Int(-1,-4,0);
        //Debug.Log("Spawning Fruit");
        fruitSpawnCount++;
        tileMap.SetTile(fruitPos, fruitTile);

        // Start the despawn countdown
        CancelInvoke(nameof(RemoveFruit));
        Invoke(nameof(RemoveFruit), fruitLifetime);
    }

    public void RemoveFruit()
    {
        tileMap.SetTile(fruitPos, null);
    }
}
