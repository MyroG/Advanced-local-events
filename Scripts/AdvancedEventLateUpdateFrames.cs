
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class AdvancedEventLateUpdateFrames : IAdvancedEvent
	{
		void LateUpdate()
		{
			_elapsedTime++;
			Loop();
		}
	}
}
