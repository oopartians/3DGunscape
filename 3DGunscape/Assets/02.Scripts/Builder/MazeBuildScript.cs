using UnityEngine;
using System.Collections;

public class MazeBuildScript : MonoBehaviour
{
    class Point
    {
        public int x;
        public int z;
        public Point(int x,int z)
        {
            this.x = x;
            this.z = z;
        }
    }
    public int mazeXLength = 200;
    public int mazeZLength = 200;
    public int tileXLength = 10;
    public int tileZLength = 10;
    public float wallPosY = 2f;
    public float wallScaleY = 10f;
    public GameObject floor = null;
    public GameObject wall = null;

    int[] dx = { -1, 0, 1, 0 }, dz = { 0, -1, 0, 1 };
    bool[,] map;
    int tilesPerX;
    int tilesPerZ;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool isSafeRange(int x, int z)
    {
        return x >= 0 && x < tilesPerX && z >= 0 && z < tilesPerZ;
    }

    void MakeWalls()
    {
        float startX = (floor.transform.position.x - floor.transform.lossyScale.x / 2);
        float startZ = (floor.transform.position.z - floor.transform.lossyScale.y / 2);
        Debug.Log(startX);
        Debug.Log(startZ);
        float endX = (floor.transform.position.x + floor.transform.lossyScale.x / 2);
        float endZ = (floor.transform.position.z + floor.transform.lossyScale.y / 2);
        /*
        미로벽 만들기 알고리즘
        11101
        10111
        11010
        01011

        1. 먼저 x축을 하나씩 증가시키면서 가로로 벽을 만들기로 함.
        2. x값을 포문돌리면서
            1) 1인 칸을 찾음.
            2) 해당 칸 왼쪽 위 위치를 저장
            3) 해당 칸이 1이고 위칸이 없거나 0이면 for문 계속 진행
            4) 해당칸이 1이 아니거나, 위칸이 1이면 for문을 나와 그 칸의 왼쪽 위 위치까지 벽을 만듬.
                단 벽의 시작과 끝위치가 같다면 생성x
            5) 다음 칸부터 다시 진행
            6) tilesPerZ까지 가면 종료


        */
        /*
        for (int x = 0; x < tilesPerX; x++)
        {
            int wallX = startX + x * tileXLength;
            int z = 0;
            while (z < tilesPerZ) {
                for (; z < tilesPerZ; z++) {
                    if (map[x, z]) break;
                }
                int wallStartZ = startZ + z * tileZLength;
                for (; z < tilesPerZ; z++)
                {
                    if (!map[x, z] || (isSafeRange(x - 1, z) && map[x - 1, z])) break;
                }
                int wallEndZ = startZ + z * tileZLength;

                if (wallStartZ != wallEndZ)
                {
                    Instantiate(wall, wallStartZ);
                }

                z++;
            }
        }*/
        float halfTileX = (float)tileXLength / 2;
        float halfTileZ = (float)tileZLength / 2;
        for (int x = 0; x < tilesPerX; x++)
        {
            for (int z = 0; z < tilesPerZ; z++)
            {
                if (map[x, z])
                {
                    float midX = startX + x * tileXLength + halfTileX;
                    float midZ = startZ + z * tileZLength + halfTileZ;
                    for (int i = 0; i < 4; i++)
                    {
                        if (!isSafeRange(x + dx[i], z + dz[i]) || !map[x + dx[i], z + dz[i]])
                        {
                            GameObject gobj = Instantiate(wall, new Vector3(midX + dx[i] * halfTileX, wallPosY, midZ + dz[i] * halfTileZ), Quaternion.identity) as GameObject;
                            gobj.transform.parent = transform;
                            if (i == 0 || i == 2)
                                gobj.transform.localScale = new Vector3(1f, wallScaleY, tileZLength);
                            else
                                gobj.transform.localScale = new Vector3(tileXLength, wallScaleY, 1f);
                        }
                    }
                }
            }
        }
    }

    void MakeMazeArr()
    {
        tilesPerX = mazeXLength / tileXLength;
        tilesPerZ = mazeZLength / tileZLength;

        bool[,] duplchk = new bool[tilesPerX, tilesPerZ];
        map = new bool[tilesPerX, tilesPerZ];

        /*
        간단한 미로 생성 알고리즘(prim algorithm을 이용 - wiki참조)
        
        - 지나다닐수 있는 길은 1, 없는 길은 0으로 함.

        1. 맵의 가운데를 지나다닐 수 있는 길로 체크하고 duplchk의 해당 칸을 true로 함.
        2. 맵의 가운데 좌표의 4방향의 값을 list에 넣음.
        */
        map[tilesPerX / 2, tilesPerZ / 2] = true;
        duplchk[tilesPerX / 2, tilesPerZ / 2] = true;
        /*
        3. 4방향의 칸들이 이미 list에 있다는 의미로 duplchk의 해당 칸을 true로 함. -- 맵의 크기가 매우 작을 경우 버그 가능성
        */
        ArrayList remainBlock = new ArrayList();
        for (int i = 0; i < 4; i++)
        {
            remainBlock.Add(new Point(tilesPerX / 2 + dx[i], tilesPerZ / 2 + dz[i]));
            duplchk[tilesPerX / 2 + dx[i], tilesPerZ / 2 + dz[i]] = true;
        }

        /*
        4. 루프시작
            1) list에서 임의의 좌표를 가져옴
            2) 길이 만들어 질 수 없는 칸이라면 1)을 반복
                -길이 뚫릴 경우 가능경로가 복수개가 되는 경우
            3) 길을 뚫을 수 있는 칸이라면 1로 설정하고 그 칸의 4방향의 칸중에서 duplchk가 false인 칸을 list에 추가한다.
            4) 사용자의 임의의 기준(추후 만들지도) 이나 , list가 비면 종료
        */
        while (remainBlock.Count > 0)
        {
            Point popped = (Point)remainBlock[Random.Range(0, remainBlock.Count)];
            remainBlock.Remove(popped);
            int cntAdjacented = 0;
            for (int i = 0; i < 4; i++)
            {
                if (isSafeRange(popped.x + dx[i], popped.z + dz[i]) && map[popped.x + dx[i], popped.z + dz[i]])
                    cntAdjacented++;
            }
            if (cntAdjacented >= 2)
            {
                continue;
            }
            map[popped.x, popped.z] = true;
            for (int i = 0; i < 4; i++)
            {
                if (isSafeRange(popped.x + dx[i], popped.z + dz[i]) && !duplchk[popped.x + dx[i], popped.z + dz[i]])
                {
                    duplchk[popped.x + dx[i], popped.z + dz[i]] = true;
                    remainBlock.Add(new Point(popped.x + dx[i], popped.z + dz[i]));
                }
            }
        }
    }

    public void BuildMaze()
    {
        MakeMazeArr();
        MakeWalls();
    }

    public void clearMaze()
    {
        Transform[] arr = GetComponentsInChildren<Transform>();
        foreach(Transform child in arr)
        {
            if (child != transform)
                DestroyImmediate(child.gameObject);
        }
    }
}
