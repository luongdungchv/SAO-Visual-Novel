using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test2 : MonoBehaviour
{
    //public Object obj1;
    public GameObject obj2;
    public AnimationPlayer animPlayer;
    //public SpriteRenderer renderer;
    public Material mat;
    public Material mat1;
    public Sprite sp;

    private void Start()
    {
        //Data2 data = new Data2()
        //{
        //    groupId = 0,
        //    contentIndex = 0,
        //    saveDate = DateTime.Now.ToString(),
        //    imageData = new List<Character>()
        //};
        //data.imageData.Add(null);
        //data.imageData.Add(null);
        //data.imageData.Add(null);
        //string json = JsonUtility.ToJson(data);
        //PlayerPrefs.SetString("dataslot3", json);
        //Debug.Log(json);

        Sprite testsprite = sp;
        Texture2D btnImage = new Texture2D((int)testsprite.rect.width, (int)testsprite.rect.height);
        var pixels = testsprite.texture.GetPixels((int)testsprite.textureRect.x,
                                     (int)testsprite.textureRect.y,
                                     (int)testsprite.textureRect.width,
                                     (int)testsprite.textureRect.height);
        //Texture2D btnImage = new Texture2D(666, 469);
        //var pixels = testsprite.texture.GetPixels(4, 7421, 666, 469);
        btnImage.SetPixels(pixels, 0);
        //btnImage.Resize(btnImage.width, btnImage.width);
        Debug.Log(testsprite.textureRect.width.ToString() + " " + testsprite.textureRect.height.ToString());
        Debug.Log(testsprite.textureRect.y);
        btnImage.Apply();

        mat.mainTexture = sp.texture;
        mat1.mainTexture = btnImage;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Character c = new Character();
            c.name = "hoshi2 1";
            c.bodyIndex = 4;
            c.emotionIndex = 0;
            animPlayer.Animate2(c, 0);
            //animPlayer.Animate3();
        }
    }
    [Serializable]
    class Class1
    {
        public Class2 test;
    }
    [Serializable]
    class Class2
    {
        public string name;
        public int a;
        public int b;
        public Class2(string name, int a, int b)
        {
            this.name = name;
            this.a = a;
            this.b = b;
        }
    }
}
