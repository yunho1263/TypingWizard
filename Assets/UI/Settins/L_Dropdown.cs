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
        TMP_Dropdown m_Dropdown; // ��Ӵٿ� ������Ʈ�� ���� ������ �����մϴ�.
        AsyncOperationHandle m_InitializeOperation; // ��Ķ�� �ʱ�ȭ�Ǵ� ���� ��Ӵٿ��� ��Ȱ��ȭ�մϴ�.

        void Start()
        {
            // ���� ��Ӵٿ� ������Ʈ�� �����մϴ�.
            m_Dropdown = GetComponent<TMP_Dropdown>();
            m_Dropdown.onValueChanged.AddListener(OnSelectionChanged); // ��Ӵٿ��� ����� ������ ȣ��Ǵ� �ݹ��� ����մϴ�.

            // ����ȭ �ý����� �ʱ�ȭ�� ������ ��ٸ��� ���� �ε� �޽����� �߰��ϴ� �ɼ��� ���� ����մϴ�.
            m_Dropdown.ClearOptions();
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData("Loading..."));
            m_Dropdown.interactable = false;

            // SelectedLocaleAsync�� ��Ķ�� �ʱ�ȭ�ǰ� ��Ķ�� ���õǾ����� Ȯ���մϴ�.
            m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (m_InitializeOperation.IsDone) // ��Ķ�� �ʱ�ȭ�Ǿ����� Ȯ���մϴ�.
            {
                InitializeCompleted(m_InitializeOperation); // ��Ķ�� �ʱ�ȭ�Ǿ����� ��� ��Ӵٿ��� �����մϴ�.
            }
            else
            {
                m_InitializeOperation.Completed += InitializeCompleted; // ��Ķ�� �ʱ�ȭ�Ǹ� ��Ӵٿ��� �����մϴ�.
            }
        }

        void InitializeCompleted(AsyncOperationHandle obj) // ��Ķ�� �ʱ�ȭ�Ǹ� ȣ��˴ϴ�.
        {
            // �� ��Ķ�� ���� ��Ӵٿ�� �ɼ� �����
            var options = new List<string>();
            int selectedOption = 0;
            var locales = LocalizationSettings.AvailableLocales.Locales;
            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selectedOption = i;

                // ��Ķ�� ǥ�� �̸��� �����ɴϴ�.
                var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
                options.Add(displayName);
            }

            // ��Ķ�� ������ ������ �߻��� ���� �� �ֽ��ϴ�.
            if (options.Count == 0)
            {
                options.Add("No Locales Available");
                m_Dropdown.interactable = false;
            }
            else
            {
                m_Dropdown.interactable = true;
            }

            m_Dropdown.ClearOptions(); // ��Ӵٿ��� ����� �� �ɼ��� �߰��մϴ�.
            m_Dropdown.AddOptions(options); // ��Ӵٿ �ɼ��� �߰��մϴ�.
            m_Dropdown.SetValueWithoutNotify(selectedOption); // ��Ӵٿ��� ���� �׸��� �����մϴ�.

            // �ٸ� ��ũ��Ʈ�� ���� ����� �� �ִ� ������ ����ȭ�� �� �ֵ��� SelectedLocaleChanged�� �����մϴ�.
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void OnSelectionChanged(int index) // ��Ӵٿ��� ����� ������ ȣ��˴ϴ�.
        {
            // �����Ϸ��� ���� ���׿��� ���ʿ��� �ݹ��� ���� �ʵ��� SelectedLocaleChanged�� ������ ����մϴ�.
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

            // ��Ӵٿ�� ������ �׸� �ش��ϴ� ��Ķ�� �����ɴϴ�.
            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            LocalizationSettings.SelectedLocale = locale;

            // �ٸ� ��ũ��Ʈ�� ���� ����� �� �ִ� ������ ����ȭ�� �� �ֵ��� SelectedLocaleChanged�� �ٽ� �����մϴ�.
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void LocalizationSettings_SelectedLocaleChanged(Locale locale) // �ٸ� ��ũ��Ʈ���� ��Ķ�� ����Ǹ� ȣ��˴ϴ�.
        {
            // ��Ӵٿ� ���� �׸��� ��ġ�ϵ��� ������Ʈ�ؾ� �մϴ�.
            var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            m_Dropdown.SetValueWithoutNotify(selectedIndex);
        }
    }

}
