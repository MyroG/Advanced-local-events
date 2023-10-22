
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace myro
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class AdvancedEventLateUpdateSeconds : IAdvancedEvent
	{
		private void LateUpdate()
		{
			Loop();

			_elapsedTime += Time.deltaTime;
		}
	}
}
