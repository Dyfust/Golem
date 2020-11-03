using FSM;
using UnityEngine;

namespace GolemStates
{
    public class DormantState : FSM.State
    {
		private Golem _golem;

		public DormantState(Golem golem) : base("Dormant State")
        {
			_golem = golem;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void UpdateLogic()
        {

        }

        public override void UpdatePhysics()
        {

        }
    }

    public class IdleState : FSM.State
	{
		private Golem _golem;

		public IdleState(Golem golem) : base("Idle State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
		}

		public override void OnExit()
		{

		}

		public override void UpdateLogic()
		{

		}

        public override void UpdatePhysics()
		{
			
		}
	}

	public class WalkingState : FSM.State
	{
		private Golem _golem;

		public WalkingState(Golem golem) : base("Walking State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
		}

		public override void OnExit()
		{

		}

		public override void UpdateLogic()
		{
		}

		public override void UpdatePhysics()
		{
			_golem.Move();
			_golem.OrientateToCamera();
		}
	}

	public class PushingState : FSM.State
	{
		private Golem _golem;

		public PushingState(Golem golem) : base("Pushing State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
			//_golem.SetAnimatorBool("Pushing", true);
		}

		public override void OnExit()
		{
			//_golem.SetAnimatorBool("Pushing", false);
		}

		public override void UpdateLogic()
		{
		}

		public override void UpdatePhysics()
		{
			_golem.Push();
		}
	}
}