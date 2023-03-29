using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Button _upgradesButton;
    [SerializeField] private Button _employeesButton;
    [SerializeField] private Button _businessButton;

    private void Awake()
    {
        _upgradesButton.onClick.AddListener(() => SetActivePanel(0));
        _employeesButton.onClick.AddListener(() => SetActivePanel(1));
        _businessButton.onClick.AddListener(() => SetActivePanel(2));
    }

    private void Start()
    {
        SetActivePanel(0);
    }

    private void OnDisable()
    {
        _upgradesButton.onClick.RemoveListener(() => SetActivePanel(0));
        _employeesButton.onClick.RemoveListener(() => SetActivePanel(1));
        _businessButton.onClick.RemoveListener(() => SetActivePanel(2));
    }

    private void SetActivePanel(int index)
    {
        if (_panels[index].activeSelf)
            return;

        DeactivateAllPanels();

        _panels[index].SetActive(true);
    }

    private void DeactivateAllPanels()
    {
        foreach (GameObject panel in _panels)
            if (panel.activeSelf == true)
                panel.SetActive(false);
    }
}
