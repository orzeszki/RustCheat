﻿namespace GorgaTech.Data {
	public class Auth {

		internal byte[] data;
		public byte[] Data { get { return data; } }

		public Auth(Packet p) {
			data = p.BytesWithSize();
		}
	}
}
