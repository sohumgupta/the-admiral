using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the HowToPlay page.
/// </summary>
public class HowToPlay : MonoBehaviour
{
    public List<GameObject> pages;
    public Button prevButton;
    public Button nextButton;

    private int currentPage;

    void Start()
    {
        currentPage = 0;
        pages[currentPage].SetActive(true);
    }

    /// <summary>
    /// Method to go to next page.
    /// </summary>
    public void nextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }

        nextButton.gameObject.SetActive(currentPage < pages.Count - 1);
        prevButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to go to previous page.
    /// </summary>
    public void previousPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }

        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive(true);
    }
}
