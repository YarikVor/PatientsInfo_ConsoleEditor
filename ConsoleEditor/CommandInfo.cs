namespace ConsoleEditor {
	public delegate void Command();
	struct CommandInfo {
		public string title;
		public Command command;

		public CommandInfo(string title, Command command) {
			this.title = title;
			this.command = command;
		}
	}
}
