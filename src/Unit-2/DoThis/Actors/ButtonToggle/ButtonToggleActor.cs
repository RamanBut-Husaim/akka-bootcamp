using System.Windows.Forms;

using Akka.Actor;

using ChartApp.Actors.ButtonToggle.Messages;
using ChartApp.Actors.Messages;
using ChartApp.Actors.PerformanceCounterCoordinator.Messages;

namespace ChartApp.Actors.ButtonToggle
{
    public sealed class ButtonToggleActor : UntypedActor
    {
        private readonly CounterType myCounterType;
        private bool isToggledOn;
        private readonly Button myButton;
        private readonly IActorRef coordinatorActor;

        public ButtonToggleActor(
            IActorRef coordinatorActor,
            Button myButton,
            CounterType myCounterType,
            bool isToggledOn = false)
        {
            this.coordinatorActor = coordinatorActor;
            this.myButton = myButton;
            this.isToggledOn = isToggledOn;
            this.myCounterType = myCounterType;
        }

        protected override void OnReceive(object message)
        {
            if (message is Toggle && this.isToggledOn)
            {
                // toggle is currently on

                // stop watching this counter
                this.coordinatorActor.Tell(new Unwatch(this.myCounterType));

                this.FlipToggle();
            }
            else if (message is Toggle && !this.isToggledOn)
            {
                // toggle is currently off

                // start watching this counter
                this.coordinatorActor.Tell(new Watch(this.myCounterType));

                this.FlipToggle();
            }
            else
            {
                this.Unhandled(message);
            }
        }

        private void FlipToggle()
        {
            // flip the toggle
            this.isToggledOn = !this.isToggledOn;

            // change the text of the button
            this.myButton.Text = $"{this.myCounterType.ToString().ToUpperInvariant()} ({(this.isToggledOn ? "ON" : "OFF")})";
        }
    }
}
