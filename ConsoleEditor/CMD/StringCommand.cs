using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEditor.CMD {
	public class StringCommand {
		public string name { get; private set; }

		public Command command;

		public string textHelp;

		public StringCommand (string name, Command cMD, string textHelp) {
			this.name = name;
			command = cMD;
			this.textHelp = textHelp;
		}
	}
}