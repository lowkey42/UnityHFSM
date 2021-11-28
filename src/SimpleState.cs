using System;

namespace FSM {
	public class SimpleState<TStateId> : StateBase<TStateId> {
		private readonly Action onLogic;
		private bool done = false;

		public SimpleState(Action onLogic, bool needsExitTime=true)
			: base(needsExitTime: needsExitTime) {
			this.onLogic = onLogic;
		}

		public override void OnEnter() {
			done = false;
		}

		public override void OnLogic() {
			onLogic();
			done = true;
			fsm.StateCanExit();
		}

		public override void RequestExit() {
			if (!needsExitTime || done) {
				fsm.StateCanExit();
			}
		}
	}
}
