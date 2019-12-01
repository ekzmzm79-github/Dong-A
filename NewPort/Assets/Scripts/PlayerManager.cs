using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate GameObject Player_EventHandler();
    public static Player_EventHandler get_playerobj;

    public delegate void Trans_bodyDele(bool trigger);
    public static Trans_bodyDele trans_BodyDele;

    Rigidbody PlayerRgb;
    Animator PlayerAnimator;

    [SerializeField]
    GameObject body;


    public struct PlayerInfo
    {
        public float HP;
        public float MAX_HP;
        public float MP;
        public float MAX_MP;

        public int Level;
        public float AttackPower;
        public float AttackPower_Added;
    };

    private static PlayerInfo playerInfo;
    private float bonus_hp = 100f, bonus_mp = 50f, bonus_power = 30f;

    void Awake()
    {
        get_playerobj = Get_PlayerObject;
        
        trans_BodyDele = Trans_body;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerRgb = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();

        SaveDataIO.SaveTake();

        playerInfo.Level = SaveDataIO.save.Level;

        playerInfo.HP = SaveDataIO.save.HP;
        playerInfo.MAX_HP = playerInfo.HP + (bonus_hp * (playerInfo.Level - 1f));
        playerInfo.MP = SaveDataIO.save.MP;
        playerInfo.MAX_MP = playerInfo.MP + (bonus_mp * (playerInfo.Level - 1f));

        playerInfo.AttackPower = DataLoader.item_Dic[SaveDataIO.save.Equipment[DataLoader.Item_kind.Weapon]].Get_Stat() + (SaveDataIO.save.Level - 1) * bonus_power;
        

    }

    public PlayerInfo Get_PlayerInfo()
    {
        return playerInfo;
    }

    public void Set_HP(float value)
    {
        playerInfo.HP += value;
        CharacterFrame.setTrigger_Dele();
    }
    public void Set_MP(float value)
    {
        playerInfo.MP += value;
        CharacterFrame.setTrigger_Dele();

        //Debug.Log("남은 마나: " + playerInfo.MP);

    }
    public void Set_AttackPoint(float value)
    {
        playerInfo.AttackPower_Added = value;
        playerInfo.AttackPower += value;
    }

    // Update is called once per frame
    void Update()
    {

        


    }

    GameObject Get_PlayerObject()
    {
        return this.gameObject;
    }

    void Trans_body(bool trigger)
    {
        
        if(trigger)
        {
            body.SetActive(false);
        }
        else
        {
            body.SetActive(true);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "EnemyWeapon")
        {

            Debug.Log(other.name);

            GameObject Enemy = other.gameObject.GetComponent<EnemyWeapon>().Root;

            if (Enemy.transform.GetComponent<Animator>().GetBool("Attack_Flag"))
            {
                //힛리커버리 체크
                if (PlayerAnimator.GetBool("Hurt_Flag"))
                    return;

                

                PlayerAnimator.SetBool("Attack_Flag", false);
                PlayerAnimator.SetTrigger("Hurt_Trigger");

                Vector3 pos = Enemy.transform.forward.normalized;

                float power = 5f;

                PlayerRgb.AddForce(pos * power, ForceMode.Impulse);


                //레벨에 따라 데미지 감소 및 방어력 공식 설정은 현재 보류
                float damage;

                if (Enemy.name == "BossMonster")
                    damage = Enemy.GetComponent<BossMonster>().Get_BossInfo().Attack + Enemy.GetComponent<Animator>().GetFloat("Skill_Damage");
                else
                    damage = Enemy.GetComponent<Enemy>().Get_EnemyInfo().Attack;

                //디버그용 임시
                damage *= 0.5f;

                Set_HP(-damage);
                
                

                Stage_UIManager.setFloating_Dele(gameObject, damage.ToString());




            }

        }
    }

    private void Hurt_Start()
    {
        PlayerAnimator.SetBool("Hurt_Flag", true);
    }

    private void Hurt_End()
    {
        PlayerAnimator.SetBool("Hurt_Flag", false);
    }
    

}
