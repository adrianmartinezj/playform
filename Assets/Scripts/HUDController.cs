using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class HUDController : MonoBehaviour
{
    private GameObject m_AbilityIcon;
    private Image m_IconImage;
    private const string ABILITY_ICON_NAME = "AbilityIcon";
    private static HUDController m_Instance;
    public static HUDController Instance { get { return m_Instance; } }

    private void Awake()
    {
        // Singletons, baby!
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
        // Find our object and set the image to blank to begin with
        m_AbilityIcon = GameObject.Find(ABILITY_ICON_NAME);
        m_IconImage = m_AbilityIcon.GetComponent<Image>();
        m_IconImage.color = new Color(0, 0, 0, 0);
        DontDestroyOnLoad(this);
    }

    public void UpdateIcon(Sprite iconSprite)
    {
        m_IconImage.color = new Color(1, 1, 1, 1);
        m_IconImage.sprite = iconSprite;
    }

    public void ClearIcon()
    {
        m_IconImage.color = new Color(0, 0, 0, 0);
        m_IconImage.sprite = null;
    }
}
