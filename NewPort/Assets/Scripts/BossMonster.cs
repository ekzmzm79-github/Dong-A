using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour
{
    public static bool IsClear;

    public enum Boss_SkillNumber
    {
        Basic = 101,
        Combo = 102,
        Ultimate = 103

    };

    GameObject Player;

    NavMeshAgent navAgent;
    Animator animator;
    Rigidbody rigidbody;

    Monster boss;

    [SerializeField]
    Image hpBar;

    [SerializeField]
    GameObject areabox;

    public struct BossInfo
    {
        public int Index;
        public DataLoader.Monster_kind Kind;
        public string Name;
        public int Level;
        public int Many;

        public float HP; //현재 hp(동적)
        public float MAX_HP; //최대 hp(정적)

        public float Attack;
        public float Defence;
        public float AttackSpeed;
        public float MoveSpeed;
        public float Exp;
        public string Info;
    }

    public BossInfo bossInfo;

    public void Set_BossInfo(int _Index, DataLoader.Monster_kind _Kind, string _Name, int _Level, int _Many, float _HP, float _Attack, float _Defence, float _AttackSpeed, float _MoveSpeed, float _Exp, string _Info)
    {
        bossInfo.Index = _Index;
        bossInfo.Kind = _Kind;
        bossInfo.Name = _Name;
        bossInfo.Level = _Level;
        bossInfo.Many = _Many;

        bossInfo.HP = _HP;
        bossInfo.MAX_HP = _HP;

        bossInfo.Attack = _Attack;
        bossInfo.Defence = _Defence;
        bossInfo.AttackSpeed = _AttackSpeed;
        bossInfo.MoveSpeed = _MoveSpeed;
        bossInfo.Exp = _Exp;
        bossInfo.Info = _Info;

    }

    public BossInfo Get_BossInfo()
    {
        return bossInfo;
    }

    public void Set_HP(float value)
    {
        bossInfo.HP += value;

        float temp = -value;
        Stage_UIManager.setFloating_Dele(gameObject, temp.ToString());

        //hp ui 처리
        hpBar.rectTransform.localScale = new Vector3(bossInfo.HP / bossInfo.MAX_HP, 1f, 1f);

        if (bossInfo.HP <= 0)
        {
            animator.SetBool("Death_Flag", true);
            animator.SetTrigger("Death_Trigger");
        }

    }

    public void Destroy_obj()
    {
        int room_num = Convert.ToInt32(Regex.Replace(transform.parent.transform.name, @"\D", ""));

        RoomManager.decrease_EnemyDele(room_num);

        IsClear = true;
        Destroy(gameObject);
    }


    [SerializeField]
    ParticleSystem basic_effect;
    [SerializeField]
    ParticleSystem skill1_effect;
    [SerializeField]
    GameObject skill2_effect;

    private float delay1, delay2;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();


        Player = PlayerManager.get_playerobj();
    }

    // Start is called before the first frame update
    void Start()
    {
        IsClear = false;

        //보스몬스터 사전에서 찾기
        foreach (var temp in DataLoader.monster_Dic)
        {
            if (temp.Value.Get_Kind() == DataLoader.Monster_kind.Boss)
            {
                boss = temp.Value;
                break;
            }
                
        }

        delay1 = 5f;
        //delay2 = 6f;


    }

    private float distance;
    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);

            if (distance <= 2.3f)
            {

                this.gameObject.transform.LookAt(Player.transform);

                animator.SetBool("Run_Flag", false);
                Choose_Attack();

            }
            else if (distance > 8f)
            {
                animator.SetBool("Run_Flag", false);
            }
            else
            {
                navAgent.SetDestination(Player.transform.position);
                animator.SetBool("Run_Flag", true);
            }
        }

       
    }

    

    private void Choose_Attack()
    {
        /*
         //스킬 데미지의 세팅 : 장비 데미지에 +되어서 데미지 결정됨
            PlayerAnimator.SetFloat("Skill_Damage", DataLoader.skill_Dic[num].Get_Damage() / DataLoader.skill_Dic[num].Get_Count());
        */
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (delay2 <= 0f)
            {
                //궁극기 사용가능
                animator.SetTrigger("Skill2_Trigger");
                delay2 = DataLoader.skill_Dic[(int)Boss_SkillNumber.Ultimate].Get_Delay();

            }
            else if (delay1 <= 0f)
            {
                //콤보 사용가능
                animator.SetTrigger("Skill1_Trigger");
                delay1 = DataLoader.skill_Dic[(int)Boss_SkillNumber.Combo].Get_Delay();

            }
            else
            {
                //기본 공격
                animator.SetTrigger("BasicAttack_Trigger");
                
            }

        }

        delay1 -= Time.deltaTime;
        delay2 -= Time.deltaTime;

    }

    public void Effect_Start(string name)
    {

        if(name == "Basic")
        {
            basic_effect.Play(true);
            animator.SetFloat("Skill_Damage", DataLoader.skill_Dic[(int)Boss_SkillNumber.Basic].Get_Damage() / DataLoader.skill_Dic[(int)Boss_SkillNumber.Basic].Get_Count());

        }
        else if(name == "Skill1")
        {
            skill1_effect.Play(true);
            animator.SetFloat("Skill_Damage", DataLoader.skill_Dic[(int)Boss_SkillNumber.Combo].Get_Damage() / DataLoader.skill_Dic[(int)Boss_SkillNumber.Combo].Get_Count());

        }
        else if(name == "Skill2")
        {
            skill2_effect.SetActive(true);
            animator.SetFloat("Skill_Damage", DataLoader.skill_Dic[(int)Boss_SkillNumber.Ultimate].Get_Damage() / DataLoader.skill_Dic[(int)Boss_SkillNumber.Ultimate].Get_Count());
            

        }
    }

    public void Attack_Start()
    {
        animator.SetBool("Attack_Flag", true);


        if (skill2_effect.activeSelf)
            areabox.GetComponent<BoxCollider>().enabled = true;

    }


    public void Attack_End()
    {
        animator.SetBool("Attack_Flag", false);

        if (skill2_effect.activeSelf)
        {
            areabox.GetComponent<BoxCollider>().enabled = false;
            skill2_effect.SetActive(false);
            
        }
            



    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerWeapon")
        {
            Animator Player_Animator = other.gameObject.transform.root.GetComponent<Animator>();

            if (Player_Animator.GetBool("Attack_Flag"))
            {
                if (animator.GetBool("Death_Flag"))
                    return;

                animator.SetTrigger("Hurt_Trigger");

                Vector3 pos = other.transform.forward.normalized;

                float power = 0.2f;

                rigidbody.AddForce(pos * power, ForceMode.Impulse);

                
                float damage = Player_Animator.GetFloat("Skill_Damage") + PlayerManager.get_playerobj().GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower;
                Set_HP(-damage);
                

            }
            else if (other.gameObject.tag == "Missile")
            {

                animator.SetTrigger("Hurt_Trigger");

                GameObject player = PlayerManager.get_playerobj();

                float damage = player.GetComponent<Animator>().GetFloat("Skill_Damage") + player.GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower;

                Debug.Log(damage);

                Vector3 pos = other.transform.forward.normalized;

                float power = 0.2f;

                rigidbody.AddForce(pos * power, ForceMode.Impulse);


                Set_HP(-damage);

            }


        }
    }


}
