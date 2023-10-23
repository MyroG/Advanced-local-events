
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
			_nbSend++;
			TxtField.text += $"Event send at frame {_frames}, send {_nbSend} times \n";
			AdvancedEventHandlerInstance.AdvancedSendCustomEventDelayedFrames(this, nameof(Result), FramesToDelay);
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
