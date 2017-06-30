using System;
using Discord;

namespace GuitarBot {

    public class Roles {
        public readonly IRole rock;
        public readonly IRole jazz;
        public readonly IRole blues;
        public readonly IRole metal;
        public readonly IRole bass;

        public Roles(IGuild guild) {
            rock = guild.GetRole((ulong)329559573793079298);
            jazz = guild.GetRole((ulong)329635378397052928);
            blues = guild.GetRole((ulong)329559336571502592);
            metal = guild.GetRole((ulong)329638014936678402);
            bass = guild.GetRole((ulong)329658079010488322);
        }

        public bool IsNull {
            get {
                return rock == null ||
                  jazz == null ||
                  blues == null ||
                  metal == null ||
                  bass == null;
            }
        }
    }
}