using System;
using System.Threading.Tasks;

namespace GuitarBot {

    public class Program {

        private static void Main(string[] args) {
            string token = args[0];
            Bot bot = new Bot(token);
            Task.Delay(-1);
        }
    }
}