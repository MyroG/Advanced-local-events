
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	public class DelayFrames : UdonSharpBehaviour
	{
		public AdvancedEventHandler AdvancedEventHandlerInstance;
		public int FramesToDelay;
		public UnityEngine.UI.Text TxtField;
		private int _frames;
		private int _nbSend;
		private int _nbReceived;

		public override void Interact()
		{
			//We will execute the same event twice, for no particular reason
			_nbSend++;
			TxtField.text += $"Event send at frame {_frames}, send {_nbSend} times \n";
			AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedFrames(this, nameof(Result), FramesToDelay);

			_nbSend++;
			TxtField.text += $"Event send at frame {_frames}, send {_nbSend} times \n";
			AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedFrames(this, nameof(Result), FramesToDelay);

			//and another one that is delayed by 5 more frames
			_nbSend++;
			TxtField.text += $"Event send at frame {_frames}, send {_nbSend} times \n";
			int id = AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedFrames(this, nameof(Result), FramesToDelay);
			AdvancedEventHandlerInstance.DelayCustomEventFrames(id, 5);
		}

		public void Result()
		{
			_nbReceived++;
			TxtField.text += $"Event received at frame {_frames}, received {_nbReceived} times \n";
		}

		private void Update()
		{
			_frames++;
		}
	}
}
