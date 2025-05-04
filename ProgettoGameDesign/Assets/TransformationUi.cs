using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TransformationUi : MonoBehaviour
{
    [SerializeField] List<Sprite> imageList;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = imageList[0];
    }

    // Update is called once per frame
    public void SetNextImage(int index)
    {
       
        image.sprite = imageList[index];
    }
}
