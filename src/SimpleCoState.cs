using System.Collections;
using FSM;
using UnityEngine;

namespace FSM {
	public abstract class SimpleCoState<TStateId> : StateBase<TStateId> {
		protected readonly MonoBehaviour container;
		
		private Coroutine coroutine;

		private bool done = true;

		protected bool interrupted = false;

		public SimpleCoState(MonoBehaviour container, bool needsExitTime=true)
			: base(needsExitTime: needsExitTime) {
			this.container = container;
		}

		protected abstract IEnumerator DoLogic();

		private IEnumerator Logic() {
			yield return DoLogic();
			done = true;
			fsm.StateCanExit();
		}

		public override void OnEnter() {
			Debug.Assert(coroutine == null);
			done = false;
			interrupted = false;
			coroutine = container.StartCoroutine(Logic());
		}

		public override bool TryInterrupt() {
			if(done)
				return true;
			
			interrupted = true;
			return false;
		}
		
		public override void OnExit() {
			if (coroutine != null) {
				container.StopCoroutine(coroutine);
				done = true;
				coroutine = null;
			}
		}

		public override void RequestExit() {
			if (!needsExitTime || done) {
				fsm.StateCanExit();
			}
		}
	}
}
