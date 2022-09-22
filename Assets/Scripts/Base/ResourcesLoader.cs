using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ResourcesLoader : Singleton<ResourcesLoader>
{
    public Sprite[] Sprites;
    public GameObject[] Prefabs;

    [Button]
    private void SetEnun()
    {
        Sprites = Resources.LoadAll<Sprite>("Sprites");
        Prefabs = Resources.LoadAll<GameObject>("Prefabs");

        EnumCreator.CreateEnum("Sprites", Sprites.Select(x => x.name).ToArray());
        EnumCreator.CreateEnum("Prefabs", Prefabs.Select(x => x.name).ToArray());
    }
    
}

public static class ResourcesLoaderExtensions
{
    public static Sprite GetSprite(this Enum_Sprites theSprite)
    {
        return ResourcesLoader.Instance.Sprites.First(x => x.name.StripPunctuation() == theSprite.ToString().StripPunctuation());
    }
    
    public static GameObject GetPrefab(this Enum_Prefabs thePrefab)
    {
        return ResourcesLoader.Instance.Prefabs.First(x => x.name.StripPunctuation() == thePrefab.ToString().StripPunctuation());
    }
}