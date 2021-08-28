namespace GorgaTech.Data {
	public class ConsoleMessage {

		internal string message;
		public string Message { get{ return message; } }

		public ConsoleMessage(Packet p) {
			message = p.String();
		}

	}
}
