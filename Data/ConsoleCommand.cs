namespace GorgaTech.Data {
	public class ConsoleCommand {

		internal string command;
		public string Command { get { return command; } }

		public ConsoleCommand(Packet p) {
			command = p.String();
		}
	}
}
