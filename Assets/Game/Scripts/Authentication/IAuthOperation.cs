using System.Threading.Tasks;

namespace Game.Scripts.Authentication {
    public interface IAuthOperation {
        Task ExecuteAsync();
    }
}