using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandCollapse : MonoBehaviour
{
    [SerializeField] GameObject collapseThis;
    [SerializeField] GameObject expandBtn;
    [SerializeField] GameObject collapseBtn;

    public void Expand()
    {
        collapseThis.SetActive(true);
        expandBtn.SetActive(false);
        collapseBtn.SetActive(true);
    }
    public void Collapse()
    {
        collapseThis.SetActive(false);
        expandBtn.SetActive(true);
        collapseBtn.SetActive(false);
    }
}