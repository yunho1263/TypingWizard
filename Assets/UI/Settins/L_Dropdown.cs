using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace TypingWizard.UI
{
    public class L_Dropdown : MonoBehaviour
    {
        TMP_Dropdown m_Dropdown; // 드롭다운 컴포넌트에 대한 참조를 보유합니다.
        AsyncOperationHandle m_InitializeOperation; // 로캘이 초기화되는 동안 드롭다운을 비활성화합니다.

        void Start()
        {
            // 먼저 드롭다운 컴포넌트를 설정합니다.
            m_Dropdown = GetComponent<TMP_Dropdown>();
            m_Dropdown.onValueChanged.AddListener(OnSelectionChanged); // 드롭다운이 변경될 때마다 호출되는 콜백을 등록합니다.

            // 현지화 시스템이 초기화될 때까지 기다리는 동안 로딩 메시지를 추가하는 옵션을 선택 취소합니다.
            m_Dropdown.ClearOptions();
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData("Loading..."));
            m_Dropdown.interactable = false;

            // SelectedLocaleAsync는 로캘이 초기화되고 로캘이 선택되었는지 확인합니다.
            m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (m_InitializeOperation.IsDone) // 로캘이 초기화되었는지 확인합니다.
            {
                InitializeCompleted(m_InitializeOperation); // 로캘이 초기화되었으면 즉시 드롭다운을 설정합니다.
            }
            else
            {
                m_InitializeOperation.Completed += InitializeCompleted; // 로캘이 초기화되면 드롭다운을 설정합니다.
            }
        }

        void InitializeCompleted(AsyncOperationHandle obj) // 로캘이 초기화되면 호출됩니다.
        {
            // 각 로캘에 대한 드롭다운에서 옵션 만들기
            var options = new List<string>();
            int selectedOption = 0;
            var locales = LocalizationSettings.AvailableLocales.Locales;
            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selectedOption = i;

                // 로캘의 표시 이름을 가져옵니다.
                var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
                options.Add(displayName);
            }

            // 로캘이 없으면 문제가 발생한 것일 수 있습니다.
            if (options.Count == 0)
            {
                options.Add("No Locales Available");
                m_Dropdown.interactable = false;
            }
            else
            {
                m_Dropdown.interactable = true;
            }

            m_Dropdown.ClearOptions(); // 드롭다운을 지우고 새 옵션을 추가합니다.
            m_Dropdown.AddOptions(options); // 드롭다운에 옵션을 추가합니다.
            m_Dropdown.SetValueWithoutNotify(selectedOption); // 드롭다운의 선택 항목을 설정합니다.

            // 다른 스크립트에 의해 변경될 수 있는 사항을 동기화할 수 있도록 SelectedLocaleChanged를 구독합니다.
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void OnSelectionChanged(int index) // 드롭다운이 변경될 때마다 호출됩니다.
        {
            // 변경하려는 변경 사항에서 불필요한 콜백을 받지 않도록 SelectedLocaleChanged의 구독을 취소합니다.
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

            // 드롭다운에서 선택한 항목에 해당하는 로캘을 가져옵니다.
            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            LocalizationSettings.SelectedLocale = locale;

            // 다른 스크립트에 의해 변경될 수 있는 사항을 동기화할 수 있도록 SelectedLocaleChanged를 다시 구독합니다.
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void LocalizationSettings_SelectedLocaleChanged(Locale locale) // 다른 스크립트에서 로캘이 변경되면 호출됩니다.
        {
            // 드롭다운 선택 항목이 일치하도록 업데이트해야 합니다.
            var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            m_Dropdown.SetValueWithoutNotify(selectedIndex);
        }
    }

}
