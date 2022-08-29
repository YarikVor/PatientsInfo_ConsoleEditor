using System;
using System.Collections.Generic;

using PatientsInfo.Data;
using PatientsInfo.Entities;
using static ConsoleIO.Entering;

using ConsoleFormat;

using FileIO;

namespace ConsoleEditor {
	public interface IConsoleEditor {
		int Run();
	}
}
