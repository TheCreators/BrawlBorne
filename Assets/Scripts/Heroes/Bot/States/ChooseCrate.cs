using Misc.StateMachine;

namespace Heroes.Bot.States
{
    public class ChooseCrate : IState
    
    {
        private readonly Bot _bot;
        private readonly BotSensor _botSensor;

        public ChooseCrate(Bot bot, BotSensor botSensor)
        {
            _bot = bot;
            _botSensor = botSensor;
        }

        public void Tick()
        {
            _bot.Crate = _botSensor.ClosestCrateInDetectionRange;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}