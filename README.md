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

## Installation

> **Note**
An example scene is included, feel free to check it out

This prefab uses UdonSharp, so you need U#, preferably the latest version that supports DataDictionaries. Then import the .unitypackage file, or just import the source code.
Once that's done, you have a few choices :

### Use the AdvancedEventHandler prefab
- Drag and drop the `AdvancedEventHandler` prefab into your scene
- In your script that needs to send a delayed event, add this line of code, then reference it in the Unity inspector:
```
public AdvancedEventHandler AdvancedEventHandlerInstance; //you can name it however you want
```
- Then you can call custom events like this : `AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedSeconds(this, nameof(YourEventName), DelayInSeconds);`
- This is the solution I would recommend if you have hundreds of GOs that sends a lot of events. If possible, keep the number of `AdvancedEventHandler` prefabs as low as possible.

### Inheritance
- You can also create a script that inherits from `AdvancedEventHandler` : `public class YourClass : AdvancedEventHandler`
- Then, inside your script, you can call custom events like this : `AdvancedSendCustomEventDelayedSeconds(nameof(YourEventName), DelayInSeconds);`
- This would be the recommended solution if you only have a few GOs that need to occasionally sends an event

## Documentation

If events need to be delayed, just disable the `AdvancedEventHandler` script, either by disabling the GameObject or disabling the `AdvancedEventHandler` component

| Function Name                              | Parameters                                                                      | Return Type and Explanation | Description/Summary                                           |
|-------------------------------------------|---------------------------------------------------------------------------------|-----------------------------|---------------------------------------------------------------|
| `AdvancedSendCustomEventDelayedSeconds`    | `behaviour` (UdonSharpBehaviour), `eventName` (string), `delaySeconds` (float), `eventTiming` (EventTiming = EventTiming.Update) | `int` (Event ID) | Executes an event with a delay in seconds and returns the ID of the event created for later access. |
| `RemoveCustomEventDelayedSeconds`         | `id` (int), `eventTiming` (EventTiming = EventTiming.Update) | `void` | Removes an event to prevent its execution based on its ID and event timing. |
| `DelayCustomEventSeconds`                | `id` (int), `delaySeconds` (float), `eventTiming` (EventTiming = EventTiming.Update) | `void` | Delays an event based on its ID and event timing by a specified number of seconds. |
| `AdvancedSendCustomEventDelayedFrames`    | `behaviour` (UdonSharpBehaviour), `eventName` (string), `delayFrames` (int), `eventTiming` (EventTiming = EventTiming.Update) | `int` (Event ID) | Executes an event with a delay in frames and returns the ID of the event created for later access. |
| `RemoveCustomEventDelayedFrames`         | `id` (int), `eventTiming` (EventTiming = EventTiming.Update) | `void` | Removes an event to prevent its execution based on its ID and event timing. |
| `DelayCustomEventFrames`                | `id` (int), `delayFrames` (int), `eventTiming` (EventTiming = EventTiming.Update) | `void` | Delays an event based on its ID and event timing by a specified number of frames. |
| `AdvancedSendCustomEventDelayedSeconds` (protected) | `eventName` (string), `delaySeconds` (float), `eventTiming` (EventTiming = EventTiming.Update) | `int` (Event ID) | Executes an event with a delay in seconds, using the current object as the UdonBehaviour, and returns the ID of the event created for later access. |
| `AdvancedSendCustomEventDelayedFrames` (protected) | `eventName` (string), `delayFrames` (int), `eventTiming` (EventTiming = EventTiming.Update) | `int` (Event ID) | Executes an event with a delay in frames, using the current object as the UdonBehaviour, and returns the ID of the event created for later access. |


