using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CourtScene : Event
{
    private TestimonyManager theTM;
    protected int Count => theTM.GetCount(); //어떤 장면인가?
    [SerializeField]
    protected Testimony testimony;
    private TestimonyManager.InputState GetState
    {
        get { return theTM.GetState(); }
    }
    private bool isTestimony()
    {
        return GetState != TestimonyManager.InputState.testimony;
    }
    [SerializeField]
    protected Dialog backToZero;

    protected override IEnumerator EventCoroutine()
    {
        FadeIn();

        StartTestimony(testimony);//할당, 심문개시 글자가 전개되고 진입

        while (flag)
        {
            ShowTestimony(Count);

            yield return new WaitUntil(() => !isTestimony()); //입력이 있어 상태가 전이될 때까지 대기.

            switch (GetState)
            {
                case TestimonyManager.InputState.interrogate:
                    StartCoroutine(InterrogationCoroutine(Count));
                    break;
                case TestimonyManager.InputState.objection:
                    //StartCoroutine 이의제기. 이건 코루틴보다는 조건에 맞는지 확인 후 반환하는 함수가 좋겠다.
                    break;
                case TestimonyManager.InputState.back_to_zero:
                    StartDialogue(backToZero);
                    break;
                default:
                    break;
            }
            theTM.NextCount(); //return to testimony. 스택이 필요할까 - 인벤토리(법정기록)을 열었다 닫을 때 필요.
        }

        //다음으로 넘어가기
    }

    protected abstract IEnumerator InterrogationCoroutine(int _i);
    protected abstract IEnumerator ObjectionCoroutine(int _i, int _ID);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        theTM = FindObjectOfType<TestimonyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
