using System;
using System.Linq;
using EasyModbus;


namespace Modbus.Ports
{
    /// <summary>
    /// Modbus Client for the Sensor R444A01
    /// e.g. https://www.amazon.de/Temperatur-Feuchtigkeitssensor-Ziffernanzeige-Feuchtigkeit-Temperatursensor-Externem/dp/B078NR8DDJ
    /// e.g. https://www.aliexpress.com/item/33054683552.html
    /// </summary>
    public class ModbusClientR444A01
    {
        private const int TemperatureAddressRegister = 0x00;
        private const int HumidityAddressRegister = 0x01;
        private const int SlaveAddressRegister = 0x02;
        private const int BaudRateAddressRegister = 0x03;
        
        private readonly ModbusClient _ModbusClient;
        private int _slaveAddress;

        public ModbusClientR444A01(string comPort)
        {
            comPort = string.IsNullOrWhiteSpace(comPort) ? throw new ArgumentNullException(nameof(comPort)) : comPort;
            _ModbusClient = new ModbusClient(comPort);
        }

        public ModbusClientR444A01(string ipAddress, int port)
        {
            throw  new NotSupportedException("Not supported yet");
        }

        private void SetupModbusClient()
        {
            //modbusClient.UnitIdentifier = 1; //Not necessary since default slaveID = 1;
            _ModbusClient.Baudrate = 9600;	// Not necessary since default baudrate = 9600
            _ModbusClient.Parity = System.IO.Ports.Parity.None;
            _ModbusClient.StopBits = System.IO.Ports.StopBits.One;
            _ModbusClient.ConnectionTimeout = 500;
        }

        public bool IsConnected => _ModbusClient.Connected;

        public int SlaveAddress => _slaveAddress;

        public int BaudRate => _ModbusClient.Baudrate;

        public void Connect()
        {
            _ModbusClient.Connect();
        }

        public void Reconnect()
        {
            _ModbusClient.Disconnect();
            SetupModbusClient();

            Connect();
        }

        /// <summary>
        /// Change Slave address of the connected device.
        /// Default slave address of this sensor is '1'
        /// </summary>
        /// <param name="slaveAddress">Slave address between 0 and 247 </param>
        /// <returns>Return the Set Slave Address</returns>
        public byte ChangeSlaveAddress(byte slaveAddress = 1)
        {
            if (slaveAddress > 247)
            {
                throw new ArgumentOutOfRangeException(nameof(slaveAddress), "Slave address has to be between 0 and 247.");
            }

            if (IsConnected)
            {
                _ModbusClient.WriteSingleRegister(SlaveAddressRegister, slaveAddress);

                _slaveAddress = slaveAddress;
                return slaveAddress;
            }

            throw new InvalidOperationException("No connection to the Modbus client was established");
        }

        public void SetBaudRate(BaudRateR444A01 baudRate)
        {
            if (IsConnected)
            {
                _ModbusClient.WriteSingleRegister(BaudRateAddressRegister, (byte) baudRate);
            }

            throw new InvalidOperationException("No connection to the Modbus client was established");
        }

        public float ReadTemperature()
        {
            var receivedValue = _ModbusClient.ReadInputRegisters(TemperatureAddressRegister, 1).First();
            
            return receivedValue == 0 ? receivedValue : receivedValue / 10;
        }

        public int ReadSingleRegister()
        {
            var receivedValue = _ModbusClient.ReadInputRegisters(HumidityAddressRegister, 1).First();

            return receivedValue == 0 ? receivedValue : receivedValue / 10;
        }

        public int[] ReadTemperatureAndHumidity()
        {
            var receivedValue = _ModbusClient.ReadInputRegisters(TemperatureAddressRegister, 2);

            for (int i = 0; i < receivedValue.Length; i++)
            {
                // make int to float and add floating point
                receivedValue[i] = receivedValue[i] == 0 ? receivedValue[i] : receivedValue[i] / 10;
            }

            return receivedValue;
        }
    }
}
