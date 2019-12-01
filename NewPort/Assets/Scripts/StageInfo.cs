using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{

    public Text Name;
    public Text Level;
    public Text Reward;
    public Text Experience;

    private static int StageNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_StageInfo(int num)
    {
        Name.text = "스테이지: Stage. " + num;
        Level.text = "레벨: Level: " + num;

        float tmin = num * 10.0f, tmax = num * 150.0f;

        Reward.text ="기대 보상: " + tmin + " ~ " + tmax + "Gold";

        tmin = num * 100.0f;
        tmax = num * 200.0f;

        Experience.text ="기대 경험치: " + tmin + " ~ " + tmax + "Exp";

    }

}
