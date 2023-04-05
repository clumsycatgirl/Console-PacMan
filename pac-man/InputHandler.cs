using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man {
	internal static class InputHandler {
		public static string Input { get; private set; } = "";
		public delegate void KeyInputHandler();
		private static readonly Dictionary<char, KeyInputHandler> _handleInputs = new();

		private static readonly Task _inputTask = new(() => {
			while (true) {
				Input = Console.ReadKey(true).KeyChar.ToString();
				if (Input[0] is char key && _handleInputs.ContainsKey(key))
					_handleInputs[key]?.Invoke();
			}
		});

		public static void Begin() {
			_inputTask.Start();
		}

		public static void End() {
			_inputTask.Wait();
		}

		public static void AddKeyInputHandler(char key, KeyInputHandler handler) {
			if (_handleInputs.ContainsKey(key))
				_handleInputs[key] += handler;
			else
				_handleInputs.Add(key, handler);
		}
	}
}
