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

    // Set the selected image for main
    mainImage.sprite = imageList[index * 2 + 1]; // selected version

    // Get the other two indices
    List<int> otherIndices = new List<int> { 0, 1, 2 };
    otherIndices.Remove(index);

    // Set the unselected images for the side images
    sideImage1.sprite = imageList[otherIndices[0] * 2]; // unselected version
    sideImage2.sprite = imageList[otherIndices[1] * 2]; // unselected version
}



}
