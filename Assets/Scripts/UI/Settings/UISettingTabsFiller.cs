using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.UI.Settings
{
    public class UISettingTabsFiller : MonoBehaviour
    {
        [SerializeField] private UISettingTabFiller[] m_SettingTabsList = default;


        public void FillTabs(List<SettingTab> settingTabs)
        {
            for (int i = 0; i < settingTabs.Count; i++)
            {
                m_SettingTabsList[i].SetTab(settingTabs[i], i == 0);
            }
        }

        public void SelectTab(SettingTabType tabType)
        {
            for (int i = 0; i < m_SettingTabsList.Length; i++)
            {
                m_SettingTabsList[i].SetTab(tabType);
            }
        }
    }
}
