using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_UIManager : MonoBehaviour
{
    public delegate void SetFloating_Dele(GameObject target, string damage);
    public static SetFloating_Dele setFloating_Dele;

    public delegate void BuffOn_Dele();
    public static BuffOn_Dele buffOn_Dele;

    [SerializeField]
    GameObject FloatingTextPrefab;

    [SerializeField]
    Image redScreenImage;

    [SerializeField]
    Image buffFillImage;
    [SerializeField]
    GameObject buffIcon;

    [SerializeField]
    GameObject ClearPanel;

    private float speed = 2.5f;
    private bool redScreenTrigger;
    private float timer = 2.0f;

    private void Awake()
    {
        setFloating_Dele = SetFloating;
        buffOn_Dele = Buff_IconOn;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClearPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(RoomManager.IsClear && ClearPanel.activeSelf == false)
            ClearPanel.SetActive(true);

        


        if(redScreenTrigger)
        {
            Color pcolor = redScreenImage.color;

            if (pcolor.a <= 0)
            {
                redScreenTrigger = false;
                redScreenImage.gameObject.SetActive(false);
                redScreenImage.color = new Color(pcolor.r, pcolor.g, pcolor.b, 1.0f);
            }
            else
                redScreenImage.color = new Color(pcolor.r, pcolor.g, pcolor.b, pcolor.a - Time.deltaTime * speed);

        }



    }

    public void SetFloating(GameObject target, string damage)
    {

        if (FloatingTextPrefab == null)
            Debug.Log("Text프리팹이 널 값입니다.");

        if(target.tag == "Player")
        {
            //레드 스크린 처리
            redScreenImage.gameObject.SetActive(true);
            redScreenTrigger = true;
        }


        GameObject TXT = Instantiate(FloatingTextPrefab);

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        TXT.transform.localPosition = uiPosition;
        TXT.transform.SetParent(gameObject.transform.GetComponent<Canvas>().transform);
        TXT.transform.GetComponent<FloatingText>().print(damage);
    }

    public void Buff_IconOn()
    {
        //현재 실행되는 버프는 하나로 고정 -> 추후에 버프가 추가된다면 매개변수 이용
        buffIcon.SetActive(true);
        StartCoroutine("BuffTimer");
    }


    IEnumerator BuffTimer()
    {
        float durationTime = 30f, leftTime = 30f;
        float speed = 10.0f;

        while (leftTime > 0)
        {

            leftTime -= Time.deltaTime * speed;

            buffFillImage.fillAmount = 1.0f - leftTime / durationTime;

            yield return new WaitForSeconds(0.1f);
        }


        buffFillImage.fillAmount = 0.0f;
        buffIcon.SetActive(false);

        PlayerManager.get_playerobj().GetComponent<SkillManager>().Buff_Off();
    }

    public void Goto_Retry()
    {
        ClearPanel.SetActive(false);
        BossMonster.IsClear = false;

        //스테이지 씬의 리로드
    }

    public void Goto_Village()
    {
        ClearPanel.SetActive(false);
        BossMonster.IsClear = false;

        //빌리지 씬으로 복귀

        LoadingSceneManager.LoadScene("Village");
    }

}
