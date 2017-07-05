using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [System.Serializable]
    public struct Bounds
    {
        public Vector3 Minima;
        public Vector3 Maxima;
    }

    public GameObject Prefab;

    public float MinDelaySeconds = 3;
    public float MaxDelaySeconds = 10;
    public float MinSpawnRange = 2;

    public int MaxSpawns = 100;
    public int StartingSpawns = 30;

    public Bounds SpawnArea;
    public Bounds SpawnFacing;

    float nextSpawn;

	// Use this for initialization
	void Start () {

        int loopCheck = 1000;
        while (transform.childCount < StartingSpawns && loopCheck-- > 0)
            SpawnObject();
	}
	
	// Update is called once per frame
	void Update () {
		if ( Time.realtimeSinceStartup >= nextSpawn)
        {
            nextSpawn = Time.realtimeSinceStartup + Random.Range(MinDelaySeconds, MaxDelaySeconds);

            if (transform.childCount < MaxSpawns)
                SpawnObject();
        }
	}

    void SpawnObject()
    {
        var position = new Vector3();
        position.x = Random.Range(SpawnArea.Minima.x, SpawnArea.Maxima.x);
        position.y = Random.Range(SpawnArea.Minima.y, SpawnArea.Maxima.y);
        position.z = Random.Range(SpawnArea.Minima.z, SpawnArea.Maxima.z);

        var eulers = new Vector3();
        eulers.x = Random.Range(SpawnFacing.Minima.x, SpawnFacing.Maxima.x);
        eulers.y = Random.Range(SpawnFacing.Minima.y, SpawnFacing.Maxima.y);
        eulers.z = Random.Range(SpawnFacing.Minima.z, SpawnFacing.Maxima.z);



        bool changed;
        int loopCheck = 1000;
        do {
            changed = false;

            var all = Physics.OverlapSphere(position, MinSpawnRange).Where(m => m.GetType().Name != "TerrainCollider").ToArray();
            var closest = all.OrderBy(m => (position - m.transform.position).sqrMagnitude).FirstOrDefault();
            
            if (closest != null)
            {
                changed = true;

                Vector3 offset = (position - closest.transform.position);
                offset = offset.normalized * (MinSpawnRange - offset.magnitude);

                position += offset;
            }
        }
        while (changed && loopCheck-- > 0);

        if ( loopCheck > 0 )
            Instantiate(Prefab, position, Quaternion.Euler(eulers), transform);
    }
}
