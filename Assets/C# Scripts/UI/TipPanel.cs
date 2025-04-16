using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TipPanel : MonoBehaviour
{
    [SerializeField] Canvas m_Canvas;
    [SerializeField] GameObject m_Panel;
    [SerializeField] GameObject m_PageHolder;
    [SerializeField] List<GameObject> m_Pages=new();
    int m_PageIndex;
    int m_PageCount;
    Vector2 showPos;
    Vector2 hidePos;
    RectTransform m_Panel_Transform;

    private void Awake() {
        m_Canvas = transform.GetChild(0).GetComponent<Canvas>();
        m_Panel = m_Canvas.transform.GetChild(0).gameObject;
        m_PageHolder = m_Panel.transform.Find("PageHolder").gameObject;
        m_PageIndex = 0;
        m_PageCount = m_PageHolder.transform.childCount;
        m_Canvas.enabled = false;
        m_Panel_Transform = m_Panel.GetComponent<RectTransform>();
        showPos = m_Panel_Transform.position;
        hidePos = new Vector2(showPos.x, showPos.y-Screen.height);
        m_Panel_Transform.position = hidePos;

        for(int i=0;i < m_PageHolder.transform.childCount;i++){
            m_Pages.Add(m_PageHolder.transform.GetChild(i).gameObject);
            m_Pages[i].SetActive(false);    
        }
    }

    public void ShowTip(){ 
        m_Canvas.enabled=true;
        m_Panel_Transform.DOMoveY(showPos.y,0.5f,true).SetEase(Ease.OutBack);
        StartTip();
    }

    public void StartTip() {
        m_PageIndex = 0;
        m_Pages[0].SetActive(true);
    }

    public void NextPage(){
        Debug.Log("111");

        if (m_PageIndex < m_PageCount-1){
            m_Pages[m_PageIndex].SetActive(false);
            m_Pages[++m_PageIndex].SetActive(true);
        } else {
            HideTip();
        }
    }

    public void GoLastPage(){
        //if (m_PageIndex == m_PageCount - 1) return;
        //m_Pages[m_PageIndex].SetActive(false);
        //m_PageIndex = m_Pages.Count-1;
        //m_Pages[m_PageIndex].SetActive(true);

        if (m_PageIndex == 0) return;
        m_Pages[m_PageIndex].SetActive(false);
        m_PageIndex--;
        m_Pages[m_PageIndex].SetActive(true);
    }

    public void TipOver(){
        m_Pages[m_PageIndex].SetActive(false);
        m_PageIndex = 0;
    }

    public void HideTip(){
        m_Panel_Transform.DOMoveY(hidePos.y,0.5f,true).SetEase(Ease.InBack).onComplete = () => TipOver(); 
    }
}
