# Advanced Local events, a VRChat script to delay local events

In Udon, it is not recommended to implement `Update` methods, like `Update` (locked to your FPS), `FixedUpdate` (locked to your screen's refresh rate, in Hz), `LateUpdate` etc. Because executing stuff every frame can be very costly.
If you need to execute things regularly, but not every frame, you should rather use events like `SendCustomEventDelayedSeconds` or `SendCustomEventDelayedFrames`, for instance the example below shows how to create a custom Update loop that only gets executed every 5 seconds :

```
void Start()
{
   YourCustomLoop();
}

public void YourCustomLoop()
{
   if (enabled) //To ensure that the loops break if the script gets disabled
   {
      //your code here
      SendCustomEventDelayedSeconds(nameof(YourCustomLoop), 5.0f); //executes YourCustomLoop every 5 seconds
   }
}
```

The events `SendCustomEventDelayedSeconds` and `SendCustomEventDelayedFrames` have unfortunately a few issues, once an event is started it cannot be stopped or delayed, in my particular case that was really annoying.
That's why I decided to create this prefab, this prefab fixes a few issues `SendCustomEventDelayedSeconds` and `SendCustomEventDelayedFrames` have while creating a few other issues :

- Fixes : Events can be delayed, paused or removed
- Issues : My script uses an Update loop, which makes it a bit less performant than the original VRC version, it is also less accurate due to a few optimisations I implemented (The script disables itself once the event stack is empty, which can cause newly sent events to be slightly delayed by a few frames)

To summarize, do not use this prefab if:
- You don't need to delay events
- You cannot find a workaround to delay events without writing your own custom event sender.

Basically, this prefab implements new methods `AdvancedSendCustomEventDelayedSeconds` and  `AdvancedSendCustomEventDelayedFrames`, which returns an ID, that ID can later be used to break the event, or delay it.
During my tests, I was able to queue 200 events without noticing a huge performance impact, so the preformance inpact is not too bad, but it really depends on how you want to use it. 
The script automatically disables itself if the event stack is clear, which also disables the Update events.

## Installation

> **Note**
An example scene is included, feel free to check it out.

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

If events need to be paused, just disable the `AdvancedEventHandler` script, either by disabling the GameObject or disabling the `AdvancedEventHandler` component.

| Function Name                              | Parameters                                                                      | Return Type and Explanation                                | Description/Summary                                           |
|-------------------------------------------|---------------------------------------------------------------------------------|--------------------------------------------------------------|---------------------------------------------------------------|
| `AdvancedSendCustomEventDelayedSeconds`    | `behaviour` (UdonSharpBehaviour): The UdonBehaviour that has the event,<br>`eventName` (string): The name of the event,<br>`delaySeconds` (float): The delay in seconds,<br>`eventTiming` (EventTiming = EventTiming.Update): If the event should be executed in an Update or a LateUpdate event | `int` (Event ID): Executes an event with a delay in seconds and returns the ID of the event created for later access. | Executes an event with a delay in seconds. |
| `RemoveCustomEventDelayedSeconds`         | `id` (int): The ID of the event,<br>`eventTiming` (EventTiming = EventTiming.Update): Event timing used by the event | `void`: Removes an event to prevent its execution based on its ID and event timing. | Removes an event to prevent it from being executed. |
| `DelayCustomEventSeconds`                | `id` (int): ID of the event that needs to be delayed,<br>`delaySeconds` (float): Time it needs to be delayed,<br>`eventTiming` (EventTiming = EventTiming.Update): Event timing used by the event | `void`: Delays an event based on its ID and event timing by a specified number of seconds. | Delays an event. |
| `AdvancedSendCustomEventDelayedFrames`    | `behaviour` (UdonSharpBehaviour): The UdonBehaviour that has the event,<br>`eventName` (string): The name of the event,<br>`delayFrames` (int): The number of frames the event needs to be delayed,<br>`eventTiming` (EventTiming = EventTiming.Update): If the event should be executed in an Update or a LateUpdate event | `int` (Event ID): Executes an event with a delay in frames and returns the ID of the event created for later access. | Executes an event with a delay in frames. |
| `RemoveCustomEventDelayedFrames`         | `id` (int): The ID of the event,<br>`eventTiming` (EventTiming = EventTiming.Update): Event timing used by the event | `void`: Removes an event to prevent its execution based on its ID and event timing. | Removes an event to prevent it from being executed. |
| `DelayCustomEventFrames`                | `id` (int): ID of the event that needs to be delayed,<br>`delayFrames` (int): Number of frames it needs to be delayed,<br>`eventTiming` (EventTiming = EventTiming.Update): Event timing used by the event | `void`: Delays an event based on its ID and event timing by a specified number of frames. | Delays an event. |
| `AdvancedSendCustomEventDelayedSeconds` (protected) | `eventName` (string): The name of the event,<br>`delaySeconds` (float): The delay in seconds,<br>`eventTiming` (EventTiming = EventTiming.Update): If the event should be executed in an Update or a LateUpdate event | `int` (Event ID): Executes an event with a delay in seconds, using the current object as the UdonBehaviour, and returns the ID of the event created for later access. | Executes an event with a delay in seconds. |
| `AdvancedSendCustomEventDelayedFrames` (protected) | `eventName` (string): The name of the event,<br>`delayFrames` (int): The number of frames the event needs to be delayed,<br>`eventTiming` (EventTiming = EventTiming.Update): If the event should be executed in an Update or a LateUpdate event | `int` (Event ID): Executes an event with a delay in frames, using the current object as the UdonBehaviour, and returns the ID of the event created for later access. | Executes an event with a delay in frames. |

> **Warning**
Some important things to note

Each ID is unique to the type of event that got created (Update, Late update, Delayed by frames, or delayed by seconds), which means that there are  a few limitations, events created by one type cannot be modified or deleted by methods used to modify or delete different kind of events :
- Events created with `AdvancedSendCustomEventDelayedSeconds` cannot be deleted with `RemoveCustomEventDelayedFrames` , 
- Events created with `AdvancedSendCustomEventDelayedFrames` cannot be delayed with `DelayCustomEventSeconds`
- This also applies to the event timing (Update vs. Late Update)
- etc.
