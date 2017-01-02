using FuckingTrains;

namespace FuckingTrainsLights
{
    public interface ITrainStateLights
    {
        void BlinkstickOff();
        void Hello();
        void SetBlinkstickState(TrainResult train);
    }
}