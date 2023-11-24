Singleton
===========

## Concept

This is a Unity Package Manager (UPM)-compatible repository for generating a Singleton MonoBehaviour (or 
ScriptableObject) in Unity.

## Requirements

- Tested in Unity 2019.4.0f1, should work in anything newer.

## Installation

Install it via the Unity Package Manager by:
- Opening your project in Unity
- Open the Package Manager window (`Window > Package Manager`)
- Click the `+` button in the top left corner of the window
- Select `Add package from git URL...`
- Enter the following url, and you'll be up to date: `https://github.com/RadialGames/Singleton.git`

## Usage

All files in this package are in the `Radial.Singleton` namespace. Access them by adding the following to the top of your
files:

```c#
using Radial.Singleton;
```

### Si1ngleton MonoBehaviours

Lets say you have a MonoBehaviour C# class that you want to make a Singleton. You can do this by inheriting from 
`Singleton<T>` where `T` is the name of your class. For example:

```C#
public class MyClass : MonoBehaviour { ... }
```

Would become:

```C#
public class MyClass : Singleton<MyClass> { ... }
```

Now you can access your singleton instance by calling `MyClass.Instance`.

This will automatically create a new GameObject to contain your singleton if one doesn't already exist; this 
GameObject will be marked `DontDestroyOnLoad` so it will survive scene changes.

Be aware this will not prevent a non singleton constructor, such as:

```c#
MyClass badActor = new MyClassName();
```

To prevent that, you can override the constructor in your class with this:

```c#
public class MyClass : Singleton<MyClass> {
    
    protected MyClassName() { }
}
```

### Singleton ScriptableObjects

Similar to the above, you can also create a Singleton ScriptableObject by inheriting from `SingletonScriptableObject<T>`
where `T` is the name of your class. For example:

```C#  
public class MyScriptableObject : SingletonScriptableObject<MyScriptableObject { ... }
```

Note that this is an unusual use case in Unity; ensuring this object is properly loaded into memory, you need to take
one of four extra steps:

1) `Project Settings > Player > Other > Preload Assets` (* only works in compiled versions, not in editor!)
2) Reference the ScriptableObject in your scene via a public reference in any inspector
3) Manually use a `Resources.Load` call to load the asset specifically

Those are all good for most development practices. The dangerous/tricksy one:
	
4) You can load this asset into memory, in the editor only, by simply looking at
the asset in the inspector. When you hit play, if the garbage collector hasn't come
around yet, it'll still be in memory. This will probably give you weird results you
aren't expecting unless you're making a fancy editor tool with this.

## Credits

Thanks of course to the Unity Wiki which first showcased code similar to this. The original URL
(https://wiki.unity3d.com/index.php/Singleton) is no longer accessible.

This code and its iterations were inspired by a 2016 Unite talk by Richard Fine:
https://www.youtube.com/watch?v=6vmRwLYWNRo