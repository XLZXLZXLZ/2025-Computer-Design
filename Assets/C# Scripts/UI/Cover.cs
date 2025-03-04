using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 轮椅场景过渡类，不用任何预配置，单例调用直接耍！(项目里要有DOTWEEN)
/// </summary>
public class Cover : Singleton<Cover>
{
    protected override bool IsDonDestroyOnLoad => true;

    private Canvas canvas;

    //初始化一个Canvas，添加相应组件并设置为Scale With Screen Size
    protected override void Awake()
    {
        base.Awake();
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var canvasScaler = gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }

    public void ChangeScene(string sceneName)
    {
        CellCover(() => SceneManager.LoadScene(sceneName), Color.black);
    }

    /// <summary>
    /// 单元格遮蔽式的转场
    /// </summary>
    /// <param name="cellSize">每个单元格的大小</param>
    /// <param name="MidPointAction">在场景被覆盖时执行的函数</param>
    /// <param name="cellColor">单元格颜色</param>
    /// <param name="leftToRight">单元格是否从左到右生成</param>
    /// <param name="cellAppearDuration">单个单元格出现和消失的时间，默认0.3秒</param>
    /// <param name="coveredDuration">单个单元格出现后等待多久后开始消失，默认1.5秒</param>
    /// <param name="biasBetweenCells">每组出现的单元格之间的时间差，默认0.1秒</param>
    public void CellCover(Action MidPointAction, Color cellColor , float cellSize = 80f  , bool leftToRight = true , float cellAppearDuration = 0.2f, float coveredDuration = 1.5f, float coverDisappeaarDuration = 1.5f , float biasBetweenCells = 0.1f)
    {
        // 计算屏幕尺寸
        Vector2 referenceResolution = GetComponent<CanvasScaler>().referenceResolution;
        var rt = canvas.GetComponent<RectTransform>();
        float screenWidth = referenceResolution.x;
        float screenHeight = referenceResolution.y;

        // 计算每行和每列可以放置的单元格数
        int columns = Mathf.CeilToInt(screenWidth / cellSize);
        int rows = Mathf.CeilToInt(screenHeight / cellSize);

        List<RectTransform> cells = new List<RectTransform>();

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // 创建一个新的 Image
                GameObject cell = new GameObject("Cell_" + i + "_" + j);
                Image image = cell.AddComponent<Image>();
                image.color = cellColor;

                // 设置 RectTransform
                RectTransform rectTransform = cell.GetComponent<RectTransform>();
                rectTransform.SetParent(canvas.transform, false);
                rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                rectTransform.anchoredPosition = new Vector2(i * cellSize + cellSize / 2, -j * cellSize - cellSize / 2);
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);

                cells.Add(rectTransform);
            }
        }

        // 按给定顺序重新排序
        if (leftToRight)
        {
            cells.Sort((a, b) => a.anchoredPosition.x.CompareTo(b.anchoredPosition.x));
        }
        else
        {
            cells.Sort((a, b) => b.anchoredPosition.x.CompareTo(a.anchoredPosition.x));
        }

        int completedCells = 0;
        // 执行放大的DOTween
        for (int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i];
            cells[i].localScale = Vector2.zero;

            // 计算偏移时间
            float bias = (i / rows) * biasBetweenCells; // 每隔rows个cell添加0.1秒的偏移

            // 执行放大动画
            DOTween.Sequence()
                .AppendInterval(bias)
                .Append(cell.DOScale(1f, cellAppearDuration))
                .Join(cell.DORotate(Vector3.forward * 90, cellAppearDuration))
                .AppendCallback(() =>
                {
                    completedCells++;
                    // 检查是否所有cell都完成了动画
                    if (completedCells == cells.Count)
                    {
                        MidPointAction?.Invoke();
                        MidPointAction = null;
                    }
                })
                .AppendInterval(coveredDuration)
                .Append(cell.DOScale(0f, cellAppearDuration))
                .Join(cell.DORotate(Vector3.zero, cellAppearDuration))
                .OnComplete(() => { Destroy(cell.gameObject); });
        }
    }
}
