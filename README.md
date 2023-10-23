# Advanced Local events

In Udon, it is recommended to not use any `Update` loops, like `Update` (locked to your FPS), `FixedUpdate` (locked to your screen's refresh rate, in Hz), `LateUpdate` etc. Because executing stuff every frame can be very costly.
If you need to execute things regularly, you should rather use events like `SendCustomEventDelayedSeconds` or `SendCustomEventDelayedFrames`

```
public void YourCustomLoop()
{
   //your code here
   SendCustomEventDelayedSeconds(nameof(YourCustomLoop), 5.0f); //executes YourCustomLoop every 5 seconds
}
```

The events `SendCustomEventDelayedSeconds` and `SendCustomEventDelayedFrames` have unfortunately a few issues, once an event is started it cannot be stopped or delayed, in my particular case that was really annoying.
That's why I decided to create this prefab, this prefab fixes a few issues `SendCustomEventDelayedSeconds` and `SendCustomEventDelayedFrames` have while creating a few other issues :

- Fixes : Events can be delayed, paused or removed
- Issues : More Update loops

You can use this prefab if :
- You have hundreds of GameObjects that sends events with `SendCustomEventDelayedSeconds` and `SendCustomEventDelayedFrames`
- You need to pause events from time to time, or delete events.

Do not use this prefab if:
- You don't need to delay events
- You only have a few GameObjects, this prefab has a little performance overhead, which is not worth it if you only have a few GameObjects in your world that need to send delayed events

# Installation

> **Note**
An example scene is included, feel free to check it out

This prefab uses UdonSharp, so you need U#, preferably the latest version that supports DataDictionaries. Then import the .unitypackage file

Once that's done, you have a few choices :

## Use the AdvancedEventHandler prefab
- Drag and drop the prefab into your scene
- In your script that needs to send a delayed event, add this line of code, then reference it in the Unity inspector
```
public AdvancedEventHandler AdvancedEventHandlerInstance; //you can name it however you want
```
- Then you can call custom events like this : `AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedSeconds(this, nameof(YourEventName), DelayInSeconds);`
- If possible, do not add so many prefabs in your world, since any of those prefabs have an update loop.

## Inheritance
- You can also create a script that inherits from `AdvancedEventHandler` : `public class YourClass : AdvancedEventHandler`
- Then, inside your script, you can call custom events like this : `AdvancedSendCustomEventDelayedSeconds(nameof(YourEventName), DelayInSeconds);`
- This would be the recommended solution if you only have a few GOs that need this that occasionally sends an event
