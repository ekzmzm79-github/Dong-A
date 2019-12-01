using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    public delegate void Decrease_enemyDele(int room_num);
    public static Decrease_enemyDele decrease_EnemyDele;

    enum WeatherIndex { Rain = 0, Snow = 1, None = 2 };
    enum RoomChild { Color = 0, Light = 1, Weather = 2 };

    private static int P_Num = 3;

    [SerializeField]
    Transform First_Position;

    [SerializeField]
    GameObject RoomPrefab;

    [SerializeField]
    Material[] Room_color = new Material[P_Num];
    [SerializeField]
    Light[] Room_light = new Light[P_Num];

    GameObject Player;

    public struct RoomProperty
    {
        public Material color;
        public Light light;
        public int weather_ind;
    }

    private static RoomProperty[] RoomModels = new RoomProperty[P_Num * P_Num * P_Num];
    private static int[] Rand_ind = new int[P_Num * P_Num];





    public struct RoomInfo
    {
        public GameObject room;
        public int enemy_many;
    }
    private RoomInfo[] roomInfos = new RoomInfo[P_Num * P_Num * P_Num];

    static int ClearConunt = 1;
    public static bool IsClear;

    void Decrease_enemy(int room_num)
    {
        roomInfos[room_num].enemy_many--;

        if (roomInfos[room_num].enemy_many <= 0)
        {

            StageMinimap.active_BlinderDele(room_num);

            ClearConunt++;

            Debug.Log(ClearConunt);

            if(ClearConunt == P_Num * P_Num)
            {
                //모든 방이 클리어 되었다는 의미
                IsClear = true;
            }
        }
            

    }


    private void Awake()
    {
        Player = PlayerManager.get_playerobj();
        decrease_EnemyDele = Decrease_enemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        //스테이지 매니저를 통해서 발동되도록 수정할것.
        Stage_Generator();
        Set_StageRooms();
        Set_Monsters();

        IsClear = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Room_Factory()
    {
        int index = 0;

        for (int i = 0; i < P_Num; i++)
        {
            for (int j = 0; j < P_Num; j++)
            {
                for (int k = 0; k < P_Num; k++)
                {
                    RoomModels[index].color = Room_color[i];
                    RoomModels[index].light = Room_light[j];
                    RoomModels[index].weather_ind = k;

                    index++;
                }
            }
        }

    }
    void Stage_Generator()
    {

        //룸 인덱스 배열 생성
        Room_Factory();

        bool[] check = new bool[P_Num * P_Num * P_Num + 1];

        for (int i = 0; i < P_Num * P_Num; i++)
        {
            int num = Random.Range(0, P_Num * P_Num * P_Num);

            if (check[num])
                i--;
            else
            {
                Rand_ind[i] = num;
                check[num] = true;
            }

        }

    }


    void Set_StageRooms()
    {
        //생성된 룸의 세팅
        float Weight_x = 16, Weight_z = 16;

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                GameObject troom =  Instantiate(RoomPrefab) as GameObject;

                troom.transform.SetParent(gameObject.transform, true);
                troom.transform.localPosition = new Vector3(First_Position.position.x + (i * Weight_x), First_Position.position.y, First_Position.position.z + (j * Weight_z));

                troom.transform.GetChild((int)RoomChild.Color).transform.GetComponent<MeshRenderer>().material = RoomModels[Rand_ind[i * 3 + j]].color;
                troom.transform.GetChild((int)RoomChild.Light).transform.GetComponent<Light>().type = RoomModels[Rand_ind[i * 3 + j]].light.type;
                troom.transform.GetChild((int)RoomChild.Light).transform.GetComponent<Light>().intensity = RoomModels[Rand_ind[i * 3 + j]].light.intensity;

                int w_sel = RoomModels[Rand_ind[i * 3 + j]].weather_ind;

                switch (w_sel)
                {
                    case (int)WeatherIndex.Rain:
                        troom.transform.GetChild((int)RoomChild.Weather).GetChild((int)WeatherIndex.Rain).gameObject.SetActive(true);
                        break;

                    case (int)WeatherIndex.Snow:
                        troom.transform.GetChild((int)RoomChild.Weather).GetChild((int)WeatherIndex.Snow).gameObject.SetActive(true);
                        break;
                    case (int)WeatherIndex.None:

                        break;
                }

                troom.transform.name = "Room" + (i * 3 + j);
                roomInfos[i * 3 + j].room = troom;

            }
        }
    }


    void Set_Monsters()
    {
        int P_ind = Random.Range(0, P_Num * P_Num), B_ind = Random.Range(0, P_Num * P_Num);

        while(P_ind == B_ind)
            B_ind = Random.Range(0, P_Num * P_Num);

        int[] M_inds = new int[P_Num * P_Num];

        for (int i = 0; i < P_Num * P_Num; i++)
        {
            //몬스터 종류가 3개임
            int num = Random.Range(0, P_Num);

            if (i == P_ind)
            {
                Debug.Log("플레이어 룸의 번호: " + i);

                Player.transform.position = new Vector3(roomInfos[i].room.transform.position.x -16f, 1.015f, roomInfos[i].room.transform.position.z - 16f);

                MainCamera mainCamera = new MainCamera();
                Camera.main.transform.position = Player.transform.position - (1 * Vector3.forward * mainCamera.dist) + (Vector3.up * mainCamera.height);
                Camera.main.transform.LookAt(Player.transform);

                //Debug.Log("플레이어 포지션 :" + Player.transform.position);
                //Debug.Log("방의 포지션 :" + ((GameObject)RoomArray[i]).transform.position);

                roomInfos[i].enemy_many = 0;

                StageMinimap.active_BlinderDele(i);

            }
            else if(i == B_ind)
            {
                //보스 룸에 대한 세팅
                //보스 소환 및 추가처리
                int tmany = 0, tlevel = -1, tindex = 0;

                tindex = (int)DataLoader.Monster_kind.Boss;
                tlevel = DataLoader.monster_Dic[tindex].Get_Level();
                tmany = DataLoader.monster_Dic[tindex].Get_Many();


                GameObject boss = Instantiate(DataLoader.monster_Dic[tindex].Get_Prefab()) as GameObject;

                //능력치 세팅
                boss.transform.GetComponent<BossMonster>().Set_BossInfo(DataLoader.monster_Dic[tindex].Get_Index(), DataLoader.monster_Dic[tindex].Get_Kind(), DataLoader.monster_Dic[tindex].Get_Name(), DataLoader.monster_Dic[tindex].Get_Level(), DataLoader.monster_Dic[tindex].Get_Many(), DataLoader.monster_Dic[tindex].Get_HP(), DataLoader.monster_Dic[tindex].Get_Attack(), DataLoader.monster_Dic[tindex].Get_Defence(), DataLoader.monster_Dic[tindex].Get_AttackSpeed(), DataLoader.monster_Dic[tindex].Get_MoveSpeed(), DataLoader.monster_Dic[tindex].Get_Exp(), DataLoader.monster_Dic[tindex].Get_Info());
                boss.transform.SetParent(roomInfos[i].room.transform);

                //09.24
                //상수 사용 (범위에 대한 변수 조정 필요)
                float tx = Random.Range(-17f, -12f), tz = Random.Range(-17f, -12f);

                boss.transform.localPosition = new Vector3(tx, 1.0f, tz);

                boss.transform.GetComponent<NavMeshAgent>().enabled = true;
                boss.transform.GetComponent<BossMonster>().enabled = true;

                boss.name = "BossMonster";

                roomInfos[i].enemy_many = 1;
            }
            else
            {
                Spawn_Monsters(num, i);
                
            }
            


        }

    }

    void Spawn_Monsters(int monster_index, int room_num)
    {

        int tmany = 0, tlevel = -1;

        //09.24
        //몬스터 인덱스 및 해당 몬스터 스폰 정보를 json에서 불러올 수 있도록 수정할것 --> 수정완료

        tlevel = DataLoader.monster_Dic[monster_index].Get_Level();
        tmany = DataLoader.monster_Dic[monster_index].Get_Many();
        roomInfos[room_num].enemy_many = tmany;

        for (int i = 0; i < tmany; i++)
        {

            GameObject tmob = Instantiate(DataLoader.monster_Dic[monster_index].Get_Prefab()) as GameObject;

            //능력치 세팅
            tmob.transform.GetComponent<Enemy>().Set_EnemyInfo(DataLoader.monster_Dic[monster_index].Get_Index(), DataLoader.monster_Dic[monster_index].Get_Kind(), DataLoader.monster_Dic[monster_index].Get_Name(), DataLoader.monster_Dic[monster_index].Get_Level(), DataLoader.monster_Dic[monster_index].Get_Many(), DataLoader.monster_Dic[monster_index].Get_HP(), DataLoader.monster_Dic[monster_index].Get_Attack(), DataLoader.monster_Dic[monster_index].Get_Defence(), DataLoader.monster_Dic[monster_index].Get_AttackSpeed(), DataLoader.monster_Dic[monster_index].Get_MoveSpeed(), DataLoader.monster_Dic[monster_index].Get_Exp(), DataLoader.monster_Dic[monster_index].Get_Info());
            tmob.transform.SetParent(roomInfos[room_num].room.transform);

            //09.24
            //상수 사용 (범위에 대한 변수 조정 필요)
            float tx = Random.Range(-20f, -9f), tz = Random.Range(-20f, -9f);

            tmob.transform.localPosition = new Vector3(tx, 1.0f, tz);
            
            tmob.transform.GetComponent<NavMeshAgent>().enabled = true;
            tmob.transform.GetComponent<Enemy>().enabled = true;
        }






    }


}
