using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSettlement{
    public LevelSettlementController LSC { get;}

    public int Score { get; set; }
    
    //结算当前关卡，给出星级
    public void Settle();
}

public class LevelSettlementController : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] List<GameObject> Stars = new();
    [SerializeField] List<GameObject> Buttons = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finished(ILevelSettlement levelSettlementHandler) {
        UI.SetActive(true);
        LevelChooseManager.Instance.OnCheckPointPassed(levelSettlementHandler);
        //根据分数处理UI
        StartCoroutine(finished(levelSettlementHandler));
    }

    IEnumerator finished(ILevelSettlement levelSettlementHandler) {
        RectTransform backRect = UI.transform.GetChild(0).GetComponent<RectTransform>();
        float y = backRect.anchoredPosition.y;
        backRect.anchoredPosition = new Vector2(backRect.anchoredPosition.x, y - Screen.height);
        backRect.gameObject.SetActive(true);

        bool block = false;
        backRect.DOAnchorPosY(y,0.8f).OnComplete(() => { block = true; });
        yield return new WaitUntil(() => block);

        //yield return sq.WaitForCompletion();

        for (int i = 0; i < 3 && i < levelSettlementHandler.Score; i++) {
            Stars[i].SetActive(true);
            yield return new WaitForSeconds(1);
        }

        RectTransform rect;
        foreach (var button in Buttons) {
            rect = button.GetComponent<RectTransform>();
            float pos_y = rect.position.y;
            rect.position = new(rect.position.x,0);
            button.SetActive(true);

            button.transform.DOMoveY(pos_y,0.8f);
        }
    }

    public void Next() {
        //切换场景
        LevelManager.Instance.FinishLevel();
    }
}
