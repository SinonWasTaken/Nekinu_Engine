using System.Collections.Generic;

namespace Nekinu.EngineDebug
{
    public class Debug
    { 
        private static List<DebugType> lines = new List<DebugType>();

        public static List<DebugType> all_lines => lines;
        
        public void Init()
        {
            lines.Clear();
        }

        public static void WriteLine(object value)
        {
            if (!check_if_line_exists(value.ToString(), DebugType.type.White))
            {
                lines.Add(new DebugType(value.ToString(), DebugType.type.White));
            }
        }

        public static void WriteError(object value)
        {
            if (!check_if_line_exists(value.ToString(), DebugType.type.Red))
            {
                lines.Add(new DebugType(value.ToString(), DebugType.type.Red));
            }
        }

        public static void Clear()
        {
            lines.Clear();
        }
        
        internal static bool check_if_line_exists(string line, DebugType.type type)
        {
            foreach (DebugType debug_line in lines)
            {
                if (debug_line.line == line)
                {
                    debug_line.increment();
                    return true;
                }
            }

            return false;
        } 
    }

    public class DebugType
    {
        public enum type
        {
            White, Red
        }

        public type debug_type { get; private set; }

        public string line { get; private set; }

        public int counter { get; private set; }

        public DebugType(string line, type type)
        {
            debug_type = type;
            this.line = line;
            counter++;
        }

        public void increment()
        {
            counter++;
        }
    }
}