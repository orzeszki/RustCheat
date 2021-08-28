namespace GorgaTech.Data {
	public class GroupLeave {

		internal uint groupId;
		public uint GroupID { get { return groupId; } }

		public GroupLeave(Packet p) {
			groupId = p.GroupID();
		}
	}
}
