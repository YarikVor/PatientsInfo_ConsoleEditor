using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCMD {
	public delegate void SCMD();
	public class StringCommand {
		public string name { get; private set; }

		public SCMD command;

		public StringCommand (string name, SCMD cMD) {
			this.name = name;
			command = cMD;
		}
	}
}
