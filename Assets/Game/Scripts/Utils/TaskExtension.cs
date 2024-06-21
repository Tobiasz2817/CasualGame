using System;
using System.Threading.Tasks;

namespace Game.Scripts.Utils {
    public static class TaskExtension {
        public static async Task While(Func<bool> condition, int exceptionTime) {
            while (condition.Invoke()) {
                await Task.Delay(exceptionTime);
            }
        }
    }
}
