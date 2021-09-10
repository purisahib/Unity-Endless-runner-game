using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_TileManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float spawnZ = -6.0f;
    [SerializeField]
    private float tileLength = 27.0f;//Length of tile
    [SerializeField]
    private float safeZone = 40.0f;
    [SerializeField]
    private int amnTilesOnScreen = 7;
    [SerializeField]
    private int lastPrefabIndex = 0;

    [SerializeField]
    private List<GameObject> activeTiles;
    
    private void Start() {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for(int i = 0; i< amnTilesOnScreen; i++){
            if(i < 2)
                SpawnTile(0);
            else
                SpawnTile();
        }
    }
    private void Update() {
        if(playerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLength)){
            SpawnTile();
            DeletTile();
        }
    }
    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        if(prefabIndex == -1){
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        }else{
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }
    private void DeletTile(){
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
    private int RandomPrefabIndex(){
        if(tilePrefabs.Length <= 1){
            return 0;
        }

        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex){
            randomIndex = Random.Range(0, tilePrefabs.Length);
        } 
        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
