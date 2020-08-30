using FSM;

namespace OrbStates
{
    public class IdleState : State
    {
        private Orb _orb;

        public IdleState(Orb orb) : base("Idle State")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            _orb.ResetState();
        }

        public override void UpdateLogic()
        {

        }

        public override void UpdatePhysics()
        {
            _orb.UpdateController();
        }
    }

    public class RollingState : State
    {
        private Orb _orb;

        public RollingState(Orb orb) : base("Rolling State")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            _orb.ResetState();
        }

        public override void UpdateLogic()
        {
        }

        public override void UpdatePhysics()
        {
            _orb.Move();
            _orb.UpdateController();
            _orb.OrientateToCamera();
        }
    }

    public class MountedState : State
    {
        private Orb _orb;

        public MountedState(Orb orb) : base("Mounted State")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            _orb.ResetState();
            _orb.ResetVelocity();
            _orb.ExitGolem();
        }

        public override void UpdateLogic()
        {
            _orb.OrientateToGolem();
        }

        public override void UpdatePhysics()
        {
            _orb.StickToGolem();
        }
    }
}