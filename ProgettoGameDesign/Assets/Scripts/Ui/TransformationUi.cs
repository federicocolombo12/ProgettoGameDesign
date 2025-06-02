using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TransformationUi : MonoBehaviour
{
    [SerializeField] List<Sprite> imageList;

    [SerializeField] Image mainImage;
    [SerializeField] Image sideImage1;
    [SerializeField] Image sideImage2;


    private void Start()
    {
        mainImage.sprite = imageList[0];
        sideImage1.sprite = imageList[1];
        sideImage2.sprite = imageList[2];
    }
    // Update is called once per frame
    public void SetNextImage(int index)
{
    if (imageList == null || imageList.Count < 6)
    {
        Debug.LogWarning("Image list must have at least 6 sprites (3 transformations, each with selected and unselected).");
        return;
    }

    if (index < 0 || index > 2)
    {
        Debug.LogWarning("Index must be between 0 and 2.");
        return;
    }

    // Set main image (selected)
    mainImage.sprite = imageList[index * 2 + 1];

    // Determine which two indices to use for the sides, in visual order
    int leftIndex, rightIndex;

    switch (index)
    {
        case 0:
            leftIndex = 1;
            rightIndex = 2;
            break;
        case 1:
            leftIndex = 0;
            rightIndex = 2;
            break;
        case 2:
            leftIndex = 1;
            rightIndex = 0;
            break;
        default:
            leftIndex = 0;
            rightIndex = 0;
            break;
    }

    // Set side images (not selected versions)
    sideImage1.sprite = imageList[leftIndex * 2];   // unselected
    sideImage2.sprite = imageList[rightIndex * 2];  // unselected
}




}

