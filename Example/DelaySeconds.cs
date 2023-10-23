
using myro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DelaySeconds : UdonSharpBehaviour
{
	public AdvancedEventHandler AdvancedEventHandlerInstance;
	public float DelayInSeconds;
	public UnityEngine.UI.Text TxtField;

	private int _nbSend;
	private int _nbReceived;

	public override void Interact()
	{
		//We will execute the same event twice, for no particular reason
		_nbSend++;
		TxtField.text += $"Event send at time {Time.time}, send {_nbSend} times \n";
		AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedSeconds(this, nameof(Result), DelayInSeconds);

		_nbSend++;
		TxtField.text += $"Event send at time {Time.time}, send {_nbSend} times \n";
		AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedSeconds(this, nameof(Result), DelayInSeconds);

		//and another one that is delayed by 5 more seconds
		_nbSend++;
		TxtField.text += $"Event send at time {Time.time}, send {_nbSend} times \n";
		int id = AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedSeconds(this, nameof(Result), DelayInSeconds);
		AdvancedEventHandlerInstance.DelayCustomEventSeconds(id, 5.0f);
	}

	public void Result()
	{
		_nbReceived++;
		TxtField.text += $"Rec. @ {Time.time}, received {_nbReceived} times, FT {Time.deltaTime} \n";
	}
}
