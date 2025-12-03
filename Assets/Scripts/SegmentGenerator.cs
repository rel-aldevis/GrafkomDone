using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segments;
    public Transform player;

    private float zPos = 0f;
    public float segmentLength = 50f;
    public int initialSegments = 3;

    void Start()
    {
        // Spawn beberapa segment diawal
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Jika player sudah mendekati segment terakhir, spawn segment berikutnya
        if (player.position.z + 60f > zPos)
        {
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        int segmentNum = Random.Range(0, segments.Length);
        Instantiate(segments[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += segmentLength;
    }
}
