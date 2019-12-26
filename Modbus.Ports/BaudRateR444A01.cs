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
}
