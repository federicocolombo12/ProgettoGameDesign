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
    if (imageList == null || imageList.Count < 3)
    {
        Debug.LogWarning("Image list must have at least 3 sprites.");
        return;
    }

    if (index < 0 || index >= imageList.Count)
    {
        Debug.LogWarning("Index out of bounds for imageList.");
        return;
    }

    mainImage.sprite = imageList[index];

    // Default: side images are 1 (left) and 2 (right)
    // But if selected index is 1 or 2, show 0 in that side slot

    sideImage1.sprite = (index == 1) ? imageList[0] : imageList[1];
    sideImage2.sprite = (index == 2) ? imageList[0] : imageList[2];
}


}
