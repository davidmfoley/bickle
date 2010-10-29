using System;
using System.Collections.Generic;

namespace Bickle.Listeners
{
    public class ColorConsoleListener : ConsoleListener
    {
        private readonly Dictionary<MessageType, ConsoleColor> _colors = new Dictionary<MessageType, ConsoleColor>();
        private readonly ConsoleColor _defaultColor;

        public ColorConsoleListener()
        {
            _defaultColor = Console.ForegroundColor;
            _colors.Add(MessageType.Failure, ConsoleColor.Red);
            _colors.Add(MessageType.Ignored, ConsoleColor.Gray);
            _colors.Add(MessageType.Pending, ConsoleColor.Yellow);

            _colors.Add(MessageType.Succeeded, ConsoleColor.Green);
        }

        protected override void Write(MessageType messageType, string message)
        {
            Console.ForegroundColor = GetColorForMessageType(messageType);
            Console.Write(message);
            Console.ForegroundColor = _defaultColor;
        }

        private ConsoleColor GetColorForMessageType(MessageType messageType)
        {
            if (_colors.ContainsKey(messageType))
                return _colors[messageType];

            return ConsoleColor.White;
        }
    }
}