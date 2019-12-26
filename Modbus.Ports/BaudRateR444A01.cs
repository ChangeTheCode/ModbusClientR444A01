namespace Modbus.Ports
{
    /// <summary>
    /// Supported Baud rate for the sensor R444A01
    /// </summary>
    public enum BaudRateR444A01
    {
        B1200 = 0,
        B2400 = 1,
        B4800 = 2,
        /// <summary>
        /// Baud rate 9600 default setting 
        /// </summary>
        B9600 = 3, 

        B19200 = 4
    }

    public static class BaudRateHelper{

        public static int GetBaudRate(BaudRateR444A01 baudRate)
        {
            switch (baudRate)
            {
                case BaudRateR444A01.B1200:
                    return 1200;
                case BaudRateR444A01.B2400:
                    return 2400;
                case BaudRateR444A01.B4800:
                    return 4800;
                case BaudRateR444A01.B9600:
                    return 9600;
                case BaudRateR444A01.B19200:
                    return 19200;
                default:
                    return 9600;
            }
        }
    }

}
