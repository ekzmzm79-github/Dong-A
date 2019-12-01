using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public enum Skill_Number
    {
        Basic = 0,
        JumpShot = 1,
        Combo = 2,
        Buff = 3,
        Missile = 4
    };


    [SerializeField]
    ParticleSystem BA_Effect;

    [SerializeField]
    ParticleSystem SK1_Effect;

    [SerializeField]
    ParticleSystem[] SK2_Effect;

    [SerializeField]
    GameObject SK3_Effect1;
    [SerializeField]
    GameObject SK3_Effect_Weapon;
    [SerializeField]
    ParticleSystem SK3_Effect2;


    [SerializeField]
    Transform SK4_FirstPostion;
    [SerializeField]
    GameObject SK4_MissilePrefab;


    Animator PlayerAnimator;
    bool BA_ClickOn;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (BA_ClickOn)
        {
            PlayerAnimator.SetTrigger("BasicAttack_Trigger");
        }


    }

    public void Attack_Start(int num)
    {
        //버튼 누름으로 실행됨
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayerAnimator.SetBool("Attack_Flag", true);

            //디버그 편의를 위해서 10%의 마나만 사용
            float for_debug = 0.1f;

            float player_mp = PlayerManager.get_playerobj().GetComponent<PlayerManager>().Get_PlayerInfo().MP;
            float cost = DataLoader.skill_Dic[num].Get_Cost() * for_debug;

            

            //마나 체크
            if (player_mp < cost)
                return;

            
            PlayerManager.get_playerobj().GetComponent<PlayerManager>().Set_MP(-cost);

            //스킬 데미지의 세팅 : 장비 데미지에 +되어서 데미지 결정됨
            PlayerAnimator.SetFloat("Skill_Damage", DataLoader.skill_Dic[num].Get_Damage() / DataLoader.skill_Dic[num].Get_Count());

            switch (num)
            {
                case (int)Skill_Number.Basic:
                    PlayerAnimator.SetTrigger("BasicAttack_Trigger");
                    BA_ClickOn = true;
                    break;
                case (int)Skill_Number.JumpShot:
                    PlayerAnimator.SetTrigger("Skill1_Trigger");
                    break;
                case (int)Skill_Number.Combo:
                    PlayerAnimator.SetTrigger("Skill2_Trigger");
                    break;
                case (int)Skill_Number.Buff:

                    if (PlayerAnimator.GetBool("Buff_Flag"))
                        return;

                    PlayerAnimator.SetTrigger("Skill3_Trigger");

                    //버프 스킬이기 때문에 어택 플래그 다시 내림
                    PlayerAnimator.SetBool("Attack_Flag", false);
                    PlayerAnimator.SetBool("Buff_Flag", true);

                    //플레이어 장비 능력치 +10% 처리 해야함.
                    float damage = gameObject.GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower;
                    gameObject.GetComponent<PlayerManager>().Set_AttackPoint(damage * PlayerAnimator.GetFloat("Skill_Damage"));



                    Stage_UIManager.buffOn_Dele();


                    break;
                case (int)Skill_Number.Missile:

                    PlayerAnimator.SetTrigger("Skill4_Trigger");

                    

                    break;

            }


        }
            
    }

    public void Attack_End()
    {
        //에니메이션 이벤트로 호출.
        PlayerAnimator.SetBool("Attack_Flag", false);
        PlayerAnimator.SetFloat("Skill_Damage", 0f);
    }

    public void BasicAttack_ClickOff()
    {
        BA_ClickOn = false;
        PlayerAnimator.ResetTrigger("BasicAttack_Trigger");
        PlayerAnimator.SetFloat("Skill_Damage", 0f);

    }


    public void Effect_Start(string name)
    {

        //애니메이션 이벤트로 호출됨
        if (name == "Basic")
        {
            BA_Effect.Play(true);
        }
        else if (name == "Skill1")
        {
            SK1_Effect.Play(true);
        }
        else if (name == "Skill2_1")
        {
            SK2_Effect[0].Play(true);
        }
        else if (name == "Skill2_2")
        {
            SK2_Effect[1].Play(true);
        }
        else if (name == "Skill2_3")
        {
            SK2_Effect[2].Play(true);
        }
        else if (name == "Skill2_4")
        {
            SK2_Effect[3].Play(true);
        }
        else if (name == "Skill3")
        {
            SK3_Effect1.SetActive(true);
            SK3_Effect2.Play(true);
            SK3_Effect_Weapon.SetActive(true);

        }
        else if (name == "Skill4")
        {
            //미사일 소환 처리
            //SK4_Effect.Play(true);
            GameObject missile = Instantiate(SK4_MissilePrefab, SK4_FirstPostion.position, SK4_FirstPostion.transform.rotation) as GameObject;
            missile.GetComponent<Missile>().Damage = PlayerAnimator.GetFloat("Skill_Damage");
            missile.SetActive(true);


        }

    }

    public void Buff_Off()
    {
        SK3_Effect1.SetActive(false);
        SK3_Effect2.Play(false);
        SK3_Effect_Weapon.SetActive(false);
        PlayerAnimator.SetBool("Buff_Flag", false);

        float tdamage = PlayerManager.get_playerobj().GetComponent<PlayerManager>().Get_PlayerInfo().AttackPower_Added;
        PlayerManager.get_playerobj().GetComponent<PlayerManager>().Set_AttackPoint(-tdamage);

    }



}
