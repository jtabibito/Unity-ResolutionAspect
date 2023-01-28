using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AutoScaleTextureEx : MonoBehaviour
{
    public enum EM_ParentType
    {
        InWindow,//保持比例
        InPanel,//不保持比例
    }
    public EM_ParentType m_emParentType;
    public EM_ParentType ParentType
    {
        get
        {
            return m_emParentType;
        }
        set
        {
            m_emParentType = value;
            Resize();
        }
    }
    
    public float m_iWDesign;
    public float m_iHDesign;
    public float DesignWidth
    {
        get
        {
            return m_iWDesign;
        }
        set
        {
            m_iWDesign = value;
            Resize();
        }
    }
    public float DesignHeight
    {
        get
        {
            return m_iHDesign;
        }
        set
        {
            m_iHDesign = value;
            Resize();
        }
    }
    private void Start()
    {
        m_iWDesign = 2400;
        m_iHDesign = 1080;
    }
    private void OnEnable()
    {
        Resize();
    }
    private void Update()
    {
        
    }
    private void OnDisable()
    {
        
    }
    public void Resize()
    {
        RawImage pRawImageMe = transform.GetComponent<RawImage>();
        if (pRawImageMe == null)
        {
            return;
        }
        Texture pTexture = pRawImageMe.mainTexture;
        if (pTexture == null)
        {
            return;
        }
        RectTransform rtMe = pRawImageMe.GetComponent<RectTransform>();
        Transform tfParent = transform.parent.transform;
        RectTransform rtParent = tfParent.GetComponent<RectTransform>();
        
        rtMe.pivot = new Vector2(0.5f, 0.5f);
        rtMe.anchorMin = new Vector2(0.5f, 0.5f);
        rtMe.anchorMax = new Vector2(0.5f, 0.5f);
        rtMe.anchoredPosition = new Vector2(0, 0);
        rtMe.sizeDelta = new Vector2(pTexture.width, pTexture.height);
        
        Vector2 v2NewSize = new Vector2(1, 1);
        if (ParentType == EM_ParentType.InWindow)
        {
            GetChildSizeInWindow(DesignWidth, DesignHeight, rtParent.rect.width, rtParent.rect.height, pTexture.width, pTexture.height, out v2NewSize);       
        }
        else if (ParentType == EM_ParentType.InPanel)
        {
            Rect rectNewUV = Rect.MinMaxRect(0, 0, 1, 1);
            GetChildSizeInPanel(DesignWidth, DesignHeight, rtParent.rect.width, rtParent.rect.height, pTexture.width, pTexture.height, out v2NewSize, out rectNewUV);
            pRawImageMe.uvRect = rectNewUV;
        }
        rtMe.sizeDelta = v2NewSize;
        // rtMe.anchoredPosition = new Vector2(0, 0);
        
    }
    public void GetChildSizeInWindow(float fWDesign, float fHDesign, float fWParent, float fHParent, float fWOriginal, float fHOriginal, out Vector2 v2NewSize)
    {
        //根据屏幕分辨率宽高比,计算出新的Canvas resolutionSize,得到UI需要显示的分辨率
        if ((fWParent / fHParent) > (fWDesign / fHDesign))
        {
            //屏幕长,对Canvas宽拉伸到屏幕比例
            fWDesign = fWParent / fHParent * fHDesign;
        }
        else
        {
            //屏幕高,对Canvas高拉伸到屏幕比例
            fHDesign = fHParent / fWParent * fWDesign;        
        }
        //对图片缩放,图片分辨率和设计分辨率相差大的边作为缩放比,等比例缩放图片
        float fScaleRate = 1.0f;
        if ((fWDesign / fHDesign) > (fWOriginal / fHOriginal))
        {
            fScaleRate = fWDesign / fWOriginal;
        }
        else
        {
            fScaleRate = fHDesign / fHOriginal;
        }
        v2NewSize = new Vector2(fWOriginal * fScaleRate, fHOriginal * fScaleRate);
    }
    public void GetChildSizeInPanel(float fWDesign, float fHDesign, float fWParent, float fHParent, float fWOriginal, float fHOriginal, out Vector2 v2NewSize, out Rect rectUVChildNew)
    {
        float fWNew = fWDesign;
        float fHNew = fHDesign;
        float fUMin = 0;
        float fUMax = 1;
        float fVMin = 0;
        float fVMax = 1;
        if (fWDesign > fWParent)
        {
            float fOverWidthHalf = (fWDesign - fWParent) * 0.5f;
            fUMin = fOverWidthHalf / fWDesign;
            fUMax = 1 - fUMin;
            fWNew = fWParent;
        }
        if (fHDesign > fHParent)
        {
            float fOverWidthHalf = (fHDesign - fHParent) * 0.5f;
            fVMin = fOverWidthHalf / fHDesign;
            fVMax = 1 - fVMin;
            fHNew = fHParent;
        }
        v2NewSize = new Vector2(fWNew, fHNew);
        rectUVChildNew = Rect.MinMaxRect(fUMin, fVMin, fUMax, fVMax);
    }
}