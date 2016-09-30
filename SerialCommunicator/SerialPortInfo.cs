using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication {
	public class SerialPortInfo {
		public string DeviceID { get; set; }
		public string Name { get; set; }

		public override string ToString() {
			return Name;
		}
	}
}
