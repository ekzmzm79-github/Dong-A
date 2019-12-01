using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    Transform PlayerTr;

    Transform EnemyTr;
    NavMeshAgent navAgent;

    Animator EnemyAnimator;
    Rigidbody EnemyRgb;

    [SerializeField]
    Image hpBar;

    public struct EnemyInfo
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

    };

    public EnemyInfo enemyInfo;

    public void Set_EnemyInfo(int _Index, DataLoader.Monster_kind _Kind, string _Name, int _Level, int _Many, float _HP, float _Attack, float _Defence, float _AttackSpeed, float _MoveSpeed, float _Exp, string _Info)
    {
        enemyInfo.Index = _Index;
        enemyInfo.Kind = _Kind;
        enemyInfo.Name = _Name;
        enemyInfo.Level = _Level;
        enemyInfo.Many = _Many;

        enemyInfo.HP = _HP;
        enemyInfo.MAX_HP = _HP;

        enemyInfo.Attack = _Attack;
        enemyInfo.Defence = _Defence;
        enemyInfo.AttackSpeed = _AttackSpeed;
        enemyInfo.MoveSpeed = _MoveSpeed;
        enemyInfo.Exp = _Exp;
        enemyInfo.Info = _Info;

    }

    public EnemyInfo Get_EnemyInfo()
    {
        return enemyInfo;
    }

    public void Set_HP(float value)
    {
        enemyInfo.HP += value;

        float temp = -value;
        Stage_UIManager.setFloating_Dele(gameObject, temp.ToString());

        hpBar.rectTransform.localScale = new Vector3(enemyInfo.HP / enemyInfo.MAX_HP, 1f, 1f);


        if (enemyInfo.HP <= 0)
        {
            hpBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);

            EnemyAnimator.SetBool("Death_Flag", true);
            EnemyAnimator.SetTrigger("Death_Trigger");
        }
            

    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerTr = PlayerManager.get_playerobj().GetComponent<Transform>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyRgb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        EnemyTr = GetComponent<Transform>();

    }

    private float distance;
    // Update is called once per frame
    void Update()
    {
        if (EnemyAnimator.GetBool("Death_Flag") || EnemyAnimator.GetBool("Attack_Flag"))
            return;

        if (BossMonster.IsClear)
        {
            

            hpBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);

            EnemyAnimator.SetBool("Death_Flag", true);
            EnemyAnimator.SetTrigger("Death_Trigger");

            return;
        }


        

        distance = Vector3.Distance(EnemyTr.position, PlayerTr.position);

        if (distance <= 1.0f)
        {
            this.gameObject.transform.LookAt(PlayerTr);
            
            EnemyAnimator.SetBool("Run_Flag", false);

            //어택 실행
            EnemyAnimator.SetTrigger("BasicAttack_Trigger");

        }
        else if(distance > 8f) //범위 벗어남
        {
            EnemyAnimator.SetBool("Run_Flag", false);
        }
        else
        {
            
            //추적 실행하려하는데, 현재 공격 애니메이션 중이다.
            if (EnemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"))
            {
                EnemyAnimator.ResetTrigger("BasicAttack_Trigger");
                return;
            }

            //추척 실행
            
            navAgent.SetDestination(PlayerTr.position);
            EnemyAnimator.SetBool("Run_Flag", true);

        }
        

    }

    public void Destroy_obj()
    {
        int room_num = Convert.ToInt32(Regex.Replace(transform.parent.transform.name, @"\D", ""));  

        RoomManager.decrease_EnemyDele(room_num);

        Destroy(gameObject);
    }

    


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "PlayerWeapon")
        {
            Animator Player_Animator = other.gameObject.transform.root.GetComponent<Animator>();

            if (Player_Animator.GetBool("Attack_Flag"))
            {
                if (EnemyAnimator.GetBool("Death_Flag"))
                    return;

                EnemyAnimator.SetTrigger("Hurt_Trigger");

                Vector3 pos = other.transform.forward.normalized;

                float power = 15f;

                EnemyRgb.AddForce(pos * power, ForceMode.Impulse);

                
                float damage = Player_Animator.GetFloat("Skill_Damage") + PlayerManager.get_playerobj().GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower;
                Set_HP(-damage);


            }

        }
        else if(other.gameObject.tag == "Missile")
        {
            
            EnemyAnimator.SetTrigger("Hurt_Trigger");

            GameObject player = PlayerManager.get_playerobj();

            float damage = player.GetComponent<Animator>().GetFloat("Skill_Damage") + player.GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower;

            Debug.Log(damage);

            Vector3 pos = other.transform.forward.normalized;

            float power = 0.2f;

            EnemyRgb.AddForce(pos * power, ForceMode.Impulse);


            Set_HP(-damage);

        }

    }


    

    public void Attack_Start()
    {
        EnemyAnimator.SetBool("Attack_Flag", true);
    }

    public void Attack_End()
    {
        EnemyAnimator.SetBool("Attack_Flag", false);
    }

}
