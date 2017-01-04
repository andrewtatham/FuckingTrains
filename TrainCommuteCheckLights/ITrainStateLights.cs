using TrainCommuteCheck;

namespace TrainCommuteCheckLights
{
    public interface ITrainStateLights
    {
        void BlinkstickOff();
        void Hello();
        void SetBlinkstickState(TrainResult train);
    }
}