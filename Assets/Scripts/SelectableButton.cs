using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableButton : MonoBehaviour, IPointerClickHandler
{
    public bool IsSelected { get; private set; }
    private Image image;
    private Color selectedColor = Color.blue;
    private Color deselectedColor = Color.red;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        DeselectButton();
    }

    private void OnButtonClick()
    {
        if (!IsSelected)
        {
            // Select the button
            SelectButton();
        }
        else
        {
            // Deselect the button
            DeselectButton();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            // Select the button
            SelectButton();
        }
    }

    private void SelectButton()
    {
        // Toggle the selection state
        IsSelected = true;

        // Change the appearance or behavior of the button based on the selection state
        // For example, you could change the color of the image or add/remove a checkmark icon

        image.color = selectedColor;
    }

    private void DeselectButton()
    {
        // Toggle the selection state
        IsSelected = false;

        // Change the appearance or behavior of the button based on the selection state
        // For example, you could change the color of the image or remove a checkmark icon

        image.color = deselectedColor;
    }
}
