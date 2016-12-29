using BlinkStickDotNet;

namespace FuckingTrainsConsoleService
{
    internal class BlinkstickWrapper
    {
        public BlinkstickWrapper()
        {
           

            BlinkStick led = BlinkStick.FindFirst();

            led.OpenDevice();
            led.SetColor("red");
            led.Blink("blue");
        }

    }
}